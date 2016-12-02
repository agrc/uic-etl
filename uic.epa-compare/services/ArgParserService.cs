using System;
using System.IO;
using Mono.Options;
using uic.epa_compare.models;

namespace uic.epa_compare.services
{
    internal static class ArgParserService
    {
        internal static CompareOptions Parse(string[] args)
        {
            var options = new CompareOptions();
            var showHelp = false;

            var p = new OptionSet
            {
                {
                    "e|epa=", "REQUIRED. the path to the epa generated xml. eg: c:\\epa.xml",
                    v => options.EpaFile = v
                },
                {
                    "u|uic=",
                    "REQUIRED. the path to the uic-etl tool generated xml. eg: c:\\uic.xml",
                    v => options.UicEtlFile = v
                },
                {
                    "h|help", "show this message and exit",
                    v => showHelp = v != null
                }
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("uic compare: ");
                Console.WriteLine(e.Message);
                ShowHelp(p);

                return null;
            }

            if (showHelp)
            {
                ShowHelp(p);
                
                return null;
            }

            if (string.IsNullOrEmpty(options.EpaFile) || string.IsNullOrEmpty(options.UicEtlFile))
            {
                ShowHelp(p);

                return null;
            }

            options.EpaFile = FileExists(options.EpaFile);
            options.UicEtlFile = FileExists(options.UicEtlFile);

            if (showHelp)
            {
                ShowHelp(p);
            }

            return options;
        }

        private static string FileExists(string filepath)
        {
            if (new FileInfo(filepath).Exists)
            {
                return filepath;
            }

            var cwd = Directory.GetCurrentDirectory();
            var location = Path.Combine(cwd, filepath.TrimStart('\\'));

            if (!new FileInfo(location).Exists)
            {
                throw new InvalidOperationException("The location for the xml file path is not found.");
            }

            return location;
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: uic epa compare [OPTIONS]+");
            Console.WriteLine();
            Console.WriteLine("Options:");

            p.WriteOptionDescriptions(Console.Out);
        }
    }
}