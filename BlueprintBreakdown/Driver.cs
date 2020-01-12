using System;
using System.Linq;

namespace BlueprintBreakdown
{
    static class Driver
    {
        private const int padding = 40;

        private static Installation installation;
        private static BlueprintFolder blueprints;
        private static Calculator calculator;

        static void WriteError(string message)
        {
            var previousColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(message);
            Console.ForegroundColor = previousColour;
        }

        static void WriteLineError(string message)
        {
            var previousColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = previousColour;
        }

        static void Main(string[] args)
        {
            WriteHeader();

            Driver.installation = Installation.CreateFromWinRegistry();

            if (installation.SpaceEngineersInstalled)
            {
                Driver.blueprints = BlueprintFolder.CreateFromInstallation(Driver.installation);
                Driver.calculator = Calculator.CreateFromInstallation(Driver.installation);

                WriteInstallation();
                WriteCommands();

                Prompt();
            }
            else 
            { 
                WriteLineError("Space Engineers installation missing or not accessible");
                Console.Read();
            }
        }

        static void Prompt()
        {
            while(true)
            {
                Console.WriteLine();
                var previouscolour = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                var input = Console.ReadLine().Trim();
                Console.ForegroundColor = previouscolour;

                var command = input.Contains(" ") ? input.Split(' ')[0] : input;

                switch(command)
                {
                    case "exit": return;

                    case "clear": Console.Clear(); WriteHeader(); break;

                    case "help": WriteCommands(); break;

                    case "blueprints": WriteBlueprints(); break;

                    case "breakdown": LoadBlueprint(input.Replace(command, "")); break;

                    case "": break;

                    default: WriteLineError("unknown command"); break;
                }
                
            }
        }

        static void LoadBlueprint(string input)
        {
            input = input.TrimStart(' ');

            var blueprint = blueprints.Load(input);

            if (blueprint == null)
            {
                WriteError("could not locate blueprint ");
                WriteLineError(input);
            }
            else 
            {
                WriteBlueprint(blueprint);
            }
        }

        static void WriteHeader()
        {
            Console.WriteLine("- Blueprint Breakdown (BPD) For Space Engineers -");
            Console.WriteLine();
        }

        static void WriteCommands()
        {
            Console.WriteLine("- Commands -");

            Console.Write("blueprints".PadRight(padding));
            Console.WriteLine("lists local and downloaded blueprints");

            Console.Write("breakdown {blueprint}".PadRight(padding));
            Console.WriteLine("displays blocks, components and resource breakdown of specified blueprint");

            Console.Write("exit".PadRight(padding));
            Console.WriteLine("exits blueprint breakdown");

            Console.Write("clear".PadRight(padding));
            Console.WriteLine("clears console output");

            Console.Write("help".PadRight(padding));
            Console.WriteLine("lists available commands");
        }

        static void WriteInstallation()
        {
            Console.WriteLine("SE Path".PadRight(padding) + installation.InstallDirectory.FullName);
            Console.WriteLine("SE Blueprint Path".PadRight(padding) + installation.BlueprintsDirectory.FullName);
            Console.WriteLine();
        }

        static void WriteBlueprints()
        {
            Console.WriteLine();
            Console.WriteLine("- Local -");
            Console.WriteLine();
            foreach (var blueprint in blueprints.Local)
            {
                Console.WriteLine(blueprint);
            }

            Console.WriteLine();
            Console.WriteLine("- Workshop -");
            Console.WriteLine();
            foreach (var blueprint in blueprints.Workshop)
            {
                Console.WriteLine(blueprint);
            }
        }

        static void WriteBlueprint(Blueprint blueprint)
        {
            Console.WriteLine();
            Console.WriteLine("Blueprint");
            Console.WriteLine("Name:".PadRight(padding) + blueprint.Name);
            Console.WriteLine("Author:".PadRight(padding) + blueprint.Author);

            Console.WriteLine();
            Console.Write("- Blocks - ".PadRight(padding));
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Total Block Count ".PadRight(padding) + blueprint.BlockCount);
            Console.WriteLine();

            var blocklist = blueprint.Blocks.ToList();
            blocklist.Sort((a, b) => b.Value.CompareTo(a.Value));

            foreach (var keyvalue in blocklist)
            {
                Console.Write(keyvalue.Key.ToString().PadRight(padding));
                Console.WriteLine(keyvalue.Value);
            }


            var componentcosts = Driver.calculator.CalculateComponentCosts(blueprint);
            Console.WriteLine();
            Console.Write("- Components - ".PadRight(padding));
            Console.WriteLine();
            Console.WriteLine();
            var comlist = componentcosts.GetPartAmounts().ToList();
            comlist.Sort((a, b) => b.Value.CompareTo(a.Value));

            foreach (var component in comlist)
            {
                Console.Write(component.Key.DisplayName.PadRight(padding));
                Console.WriteLine(component.Value.ToString("N0"));
            }

            var columnpadding = 30;
            var resourcecosts = Driver.calculator.CalculateResourceCosts(componentcosts);

            Console.WriteLine();
            Console.Write("- Resources - ".PadRight(padding));
            Console.Write("Realistic".PadRight(columnpadding));
            Console.Write("x3".PadRight(columnpadding));
            Console.Write("x10".PadRight(columnpadding));
            Console.WriteLine();
            Console.WriteLine();
            var resourcelist = resourcecosts.GetPartAmounts().ToList();
            resourcelist.Sort((a, b) => b.Value.CompareTo(a.Value));

            foreach (var resource in resourcelist)
            {
                Console.Write(resource.Key.DisplayName.PadRight(padding));

                var x1 = Math.Ceiling(resource.Value).ToString("N0");
                var x3 = Math.Ceiling(resource.Value / 3).ToString("N0");
                var x10 = Math.Ceiling(resource.Value / 10).ToString("N0");

                Console.Write(x1.PadRight(columnpadding));
                Console.Write(x3.PadRight(columnpadding));
                Console.Write(x10.PadRight(columnpadding));
                Console.WriteLine();
            }
        }

    }
}
