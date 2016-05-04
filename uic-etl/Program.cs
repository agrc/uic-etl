using System;
using System.Diagnostics;
using Mono.Options;
using uic_etl.models;

namespace uic_etl
{
    internal class Program
    {
        static int _verbosity;

        private static void Main(string[] args)
        {
            var sdePath = "";
            var config = Configruation.Dev;
            string outputPath = null;
            var showHelp = false;

            var p = new OptionSet
            {
                {
                    "c|connection=", "the path to the .sde connection file for the UDEQ database. eg: c:\\udeq.sde",
                    v => sdePath = v
                },
                {
                    "e|environment=", "Modifiy the environment to run the tool. dev, stage, prod. Defaults to dev.",
                    (Configruation v) => config = v
                },
                {
                    "o|output=", "The location and filename to save the output of this tool. eg: c:\\udeq.xml. Defaults to current working directory\\uic.xml",
                    v => outputPath = v
                },
                {
                    "v", "increase debug message verbosity.",
                    v =>
                    {
                        if (v != null)
                        {
                            ++_verbosity;
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

                return;
            }

            if (string.IsNullOrEmpty(sdePath))
            {
                throw new InvalidOperationException("Missing required option -c for the location fo the sde connection file.");
            }

            if (showHelp)
            {
                ShowHelp(p);
            }
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