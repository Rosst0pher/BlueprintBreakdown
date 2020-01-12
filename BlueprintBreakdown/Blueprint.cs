using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Xml;

namespace BlueprintBreakdown
{
    sealed class Blueprint
    {
        public string Name { get; private set; }

        public string Author { get; private set; }

        public Dictionary<Block, int> Blocks { get; private set; }

        public int BlockCount
        {
            get
            {
                var count = 0;
                foreach(var value in this.Blocks.Values)
                {
                    count += value;
                }
                return count;
            }
        }

        public static Blueprint LoadFromSbbFile(FileInfo sbbfile)
        {
            using (var sbbzip = ZipFile.OpenRead(sbbfile.FullName))
            using (var sbcstream = sbbzip.GetEntry("bp.sbc").Open())
            {
                return Blueprint.LoadFromSbcStrean(sbcstream);
            }
        }

        public static Blueprint LoadFromSbcFile(FileInfo sbcfile)
        {
            using(var filestream = File.OpenRead(sbcfile.FullName))
            {
                return Blueprint.LoadFromSbcStrean(filestream);
            }
        }
        
        public static Blueprint LoadFromSbcStrean(Stream sbc)
        {
            var xdoc = new XmlDocument();
            xdoc.Load(sbc);

            var shipnode = xdoc["Definitions"]["ShipBlueprints"]["ShipBlueprint"];
            var name = shipnode["Id"].GetAttribute("Subtype");

            if(string.IsNullOrEmpty(name))
            {
                name = shipnode["Id"]["SubtypeId"].InnerText;
            }

            var author = shipnode["DisplayName"].InnerText;

            var blueprint = new Blueprint()
            {
                Name = name,
                Author = author,
                Blocks = new Dictionary<Block, int>()
            };

            foreach (XmlNode grid in shipnode["CubeGrids"].GetElementsByTagName("CubeGrid"))
            {
                var blocks = grid["CubeBlocks"];

                foreach(XmlNode blockNode in blocks.GetElementsByTagName("MyObjectBuilder_CubeBlock"))
                {
                    var blocktype = blockNode.Attributes["xsi:type"].Value.Replace("MyObjectBuilder_", string.Empty);
                    var blocksubtype = blockNode["SubtypeName"].InnerText;

                    var block = new Block(blocktype, blocksubtype);

                    if (blueprint.Blocks.TryGetValue(block, out var blockcount))
                    {
                        blueprint.Blocks[block] = blockcount + 1;
                    }
                    else 
                    {
                        blueprint.Blocks.Add(block, 1);
                    }
                }
            }
            return blueprint;
        }

        public static string LoadNameFromSbbFile(FileInfo sbbfile)
        {
            using (var sbbzip = ZipFile.OpenRead(sbbfile.FullName))
            using (var sbcstream = sbbzip.GetEntry("bp.sbc").Open())
            {
                return Blueprint.LoadNameFromSbcStream(sbcstream);
            }
        }

        public static string LoadNameFromSbcStream(Stream sbc)
        {
            var xdoc = new XmlDocument();
            xdoc.Load(sbc);

            var shipnode = xdoc["Definitions"]["ShipBlueprints"]["ShipBlueprint"];
            var name = shipnode["Id"].GetAttribute("Subtype");

            if (string.IsNullOrEmpty(name))
            {
                name = shipnode["Id"]["SubtypeId"].InnerText;
            }
            return name;
        }
    }
}
