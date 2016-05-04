using System;
using uic_etl.models;
using uic_etl.services;

namespace uic_etl
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            try
            {
                options = ArgParserService.Parse(args);
            }
            catch (InvalidOperationException e)
            {
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);

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