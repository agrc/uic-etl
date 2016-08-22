using System;
using System.IO;
using Mono.Options;
using uic_etl.models;

namespace uic_etl.services
{
    internal static class ArgParserService
    {
        internal static EtlOptions Parse(string[] args)
        {
            var options = new EtlOptions();
            var showHelp = false;

            var p = new OptionSet
            {
                {
                    "c|connection=", "the path to the .sde connection file for the UDEQ database. eg: c:\\udeq.sde",
                    v => options.SdeConnectionPath = v
                },
                {
                    "e|environment=", "Modifiy the environment to run the tool. dev, stage, prod. Defaults to dev.",
                    (Configruation v) => options.Configruation = v
                },
                {
                    "o|output=",
                    "The location and filename to save the output of this tool. eg: c:\\udeq.xml. Defaults to current working directory\\uic.xml",
                    v => options.OutputXmlPath = v
                },
                {
                    "v", "increase debug message verbosity.",
                    v =>
                    {
                        if (v != null)
                        {
                            options.Verbose = true;
                        }
                    }
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
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);
                ShowHelp(p);

                return null;
            }

            if (string.IsNullOrEmpty(options.SdeConnectionPath))
            {
                throw new InvalidOperationException(
                    "Missing required option -c for the location of the sde connection file.");
            }

            if (!new FileInfo(options.SdeConnectionPath).Exists)
            {
                var cwd = Directory.GetCurrentDirectory();
                var location = Path.Combine(cwd, options.SdeConnectionPath.TrimStart('\\'));

                if (!new FileInfo(location).Exists)
                {
                    throw new InvalidOperationException("The location for the sde file path is not found.");
                }

                options.SdeConnectionPath = location;
            }

            if (showHelp)
            {
                ShowHelp(p);
            }

            return options;
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: uicetl [OPTIONS]+");
            Console.WriteLine();
            Console.WriteLine("Options:");

            p.WriteOptionDescriptions(Console.Out);
        }
    }
}