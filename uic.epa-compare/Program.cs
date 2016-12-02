using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using uic.epa_compare.models;
using uic.epa_compare.services;

namespace uic.epa_compare
{
    internal class Program
    {
        private static readonly XNamespace Uic = "http://www.exchangenetwork.net/schema/uic/2";

        private static void Main(string[] args)
        {
            CompareOptions options;

            try
            {
                options = ArgParserService.Parse(args);
                if (options == null)
                {
                    Console.WriteLine("uic compare: was run with invalid options");
                    Console.ReadKey();

                    return;
                }
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("uic compare: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("press any key to continue");
                Console.ReadKey();

                return;
            }

            XDocument epa;
            XDocument uic;
            var sort = new Func<XElement, string>(element => element.Elements().First().Elements().First().Value);

            using (var reader = File.OpenText(options.EpaFile))
            using (var reader2 = File.OpenText(options.UicEtlFile))
            {
                epa = XDocument.Load(reader);
                uic = XDocument.Load(reader2);
            }

            var epaContainerNode = epa.Root.Descendants(Uic + "UIC");
            var epaFacilities = epaContainerNode.Elements(Uic + "FacilityList").OrderBy(sort);
            var doc = new XDocument(new XElement("Root", epaFacilities));
            doc.Save("C:\\Users\\agrc-arcgis\\Desktop\\epa-ordered.xml");

            var uicContainerNode = uic.Root.Descendants(Uic + "UIC");
            var uicFacilities = uicContainerNode.Elements(Uic + "FacilityList").OrderBy(sort);

            doc = new XDocument(new XElement("Root", uicFacilities));
            doc.Save("C:\\Users\\agrc-arcgis\\Desktop\\uic-ordered.xml");
        }
    }
}