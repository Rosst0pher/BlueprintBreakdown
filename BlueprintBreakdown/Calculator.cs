using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace BlueprintBreakdown
{
    sealed class Calculator
    {
        public IReadOnlyDictionary<Block, Recipe<Component>> BlockCosts => this.blockcosts;
        public IReadOnlyDictionary<Component, Recipe<Resource>> ComponentCosts => this.componentcosts;

        private readonly Dictionary<Block, Recipe<Component>> blockcosts;
        private readonly Dictionary<Component, Recipe<Resource>> componentcosts;

        public Calculator(Dictionary<Block, Recipe<Component>> blockcosts, Dictionary<Component, Recipe<Resource>> componentcosts)
        {
            this.blockcosts = blockcosts;
            this.componentcosts = componentcosts;
        }

        public Recipe<Component> CalculateComponentCosts(Blueprint blueprint)
        {
            var recipe = new Recipe<Component>();
            
            foreach(var kvp in blueprint.Blocks)
            {
                var block = kvp.Key;
                var count = kvp.Value;

                if(this.BlockCosts.TryGetValue(block, out var blockrecipe))
                {
                    recipe.Combine(blockrecipe, count);
                }
            }

            return recipe;
        }

        public Recipe<Resource> CalculateResourceCosts(Blueprint blueprint)
        {
            var components = this.CalculateComponentCosts(blueprint);
            return this.CalculateResourceCosts(components);
        }

        public Recipe<Resource> CalculateResourceCosts(Recipe<Component> components)
        {
            var recipe = new Recipe<Resource>();

            foreach (var component in components.GetParts())
            {
                var count = components.GetPartAmount(component);

                recipe.Combine(this.ComponentCosts[component], Convert.ToInt32(count));
            }

            return recipe;
        }

        public static Calculator CreateFromInstallation(Installation installation)
        {
            if (installation.SpaceEngineersInstalled)
            {
                var datadir = new DirectoryInfo(installation.InstallDirectory.FullName + "\\Content\\Data\\");
                var blueprintsSbc = new FileInfo(datadir.FullName + "\\Blueprints.sbc");
                var blocksdir = new DirectoryInfo(installation.InstallDirectory.FullName + "\\Content\\Data\\CubeBlocks");

                var componentcosts = Calculator.GetComponentCostsFromSbcFile(blueprintsSbc);
                var blockcosts = Calculator.GetBlockCostsFromSbcDirectory(blocksdir);

                return new Calculator(blockcosts, componentcosts);
            }
            else 
            {
                throw new Exception("Space Engineers not installed");
            }
        }

        private static List<Component> GetComponentsFromSbcFile(FileInfo sbcfile)
        {
            if (sbcfile.Exists)
            {
                try
                {
                    var components = new List<Component>();

                    var xdoc = new XmlDocument();
                    xdoc.Load(sbcfile.FullName);

                    foreach (XmlNode comnode in xdoc.GetElementsByTagName("Component"))
                    {
                        components.Add(new Component(comnode["Id"]["TypeId"].InnerText, comnode["Id"]["SubtypeId"].InnerText));
                    }
                    return components;
                }
                catch(Exception e)
                {
                    throw new Exception("Unexpected Space Engineers data format", e);
                }   
            }
            else 
            {
                throw new Exception("Space Engineers data missing");
            }
        }

        private static Dictionary<Component, Recipe<Resource>> GetComponentCostsFromSbcFile(FileInfo sbcfile)
        {
            if (sbcfile.Exists)
            {
                try
                {
                    var componentCosts = new Dictionary<Component, Recipe<Resource>>();
                    var xdoc = new XmlDocument();
                    xdoc.Load(sbcfile.FullName);

                    foreach (XmlNode bpnode in xdoc.GetElementsByTagName("Blueprint"))
                    {
                        var resultsNode = bpnode["Results"];
                        var resultNode = bpnode["Result"];

                        if (resultNode == null && resultsNode != null && resultsNode.ChildNodes.Count == 1)
                        {
                            var rn = resultsNode["Item"];
                            if(int.Parse(rn.Attributes["Amount"].Value) == 1 && rn.Attributes["TypeId"].Equals("Component"))
                            {
                                resultNode = rn;
                            }
                        }

                        if(resultNode != null)
                        {

                            var comtype = resultNode.Attributes["TypeId"].Value;
                            var comsubtype = resultNode.Attributes["SubtypeId"].Value;

                            if(comtype.Equals("Component"))
                            {
                                var recipe = new Recipe<Resource>();

                                foreach (XmlNode reqnode in bpnode["Prerequisites"].GetElementsByTagName("Item"))
                                {
                                    var attributes = reqnode.Attributes;
                                    recipe.Add(new Resource(attributes["TypeId"].Value, attributes["SubtypeId"].Value), float.Parse(attributes["Amount"].Value));
                                }

                                componentCosts.Add(new Component(comtype, comsubtype), recipe);
                            }
                        }
                    }
                    return componentCosts;
                }
                catch (Exception e)
                {
                    throw new Exception("Unexpected Space Engineers data format", e);
                }
            }
            else 
            {
                throw new Exception("Space Engineers data missing");
            }
        }

        private static Dictionary<Block, Recipe<Component>> GetBlockCostsFromSbcDirectory(DirectoryInfo sbcdir)
        {
            var blockcosts = new Dictionary<Block, Recipe<Component>>();

            foreach (var sbcfile in sbcdir.GetFiles("*.sbc"))
            {
                try
                {
                    var xdoc = new XmlDocument();
                    xdoc.Load(sbcfile.FullName);

                    foreach(XmlNode defnode in xdoc.GetElementsByTagName("Definition"))
                    {
                        var block = new Block(defnode["Id"]["TypeId"].InnerText, defnode["Id"]["SubtypeId"].InnerText);

                        var recipe = new Recipe<Component>();

                        foreach(XmlNode comnode in defnode["Components"].GetElementsByTagName("Component"))
                        {
                            recipe.Add(new Component("Component", comnode.Attributes["Subtype"].Value), float.Parse(comnode.Attributes["Count"].Value));
                        }
                        blockcosts.Add(block, recipe);
                    }
                }
                catch(Exception e)
                {
                    throw new Exception("Unexpected Space Engineers data format", e);
                }
            }
            return blockcosts;
        }
    }
}
