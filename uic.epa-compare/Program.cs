using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NetBike.XmlUnit;
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
            var sortFacilities = new Func<XElement, string>(element => element.Elements().First().Elements().First().Value);

            using (var reader = File.OpenText(options.EpaFile))
            using (var reader2 = File.OpenText(options.UicEtlFile))
            {
                epa = XDocument.Load(reader);
                uic = XDocument.Load(reader2);
            }

            

            var epaContainerNode = epa.Root.Descendants(Uic + "UIC");
            var epaFacilityList = epaContainerNode.Elements(Uic + "FacilityList").OrderBy(sortFacilities);
//            var epaWells = epaFacilities.Elements(Uic + "WellDetail").OrderBy(x => x.Elements().First().Value);
//
//            var doc = new XDocument(new XElement("Root", epaWells));
//            doc.Save("C:\\Users\\agrc-arcgis\\Desktop\\epa-ordered.xml");
//
            var uicContainerNode = uic.Root.Descendants(Uic + "UIC");
            var uicFacilities = uicContainerNode.Elements(Uic + "FacilityList").OrderBy(sortFacilities);
//            var uicWells = uicFacilities.Elements(Uic + "WellDetail").OrderBy(x => x.Elements().First().Value);
//
//
//            doc = new XDocument(new XElement("Root", uicWells));
//            doc.Save("C:\\Users\\agrc-arcgis\\Desktop\\uic-ordered.xml");

            var skipTags = new[] {"LatitudeMeasure", "LongitudeMeasure", "LocationAccuracy", "LocationIdentifier"};
            var comparer = new XmlComparer
            {
                NormalizeText = true,
                Analyzer = XmlAnalyzer.Custom()
                    .SetEqual(XmlComparisonType.NodeListSequence)
                    .SetEqual(XmlComparisonType.TextValue)
                    .SetSimilar(XmlComparisonType.NamespacePrefix),
                Handler = XmlCompareHandling.Limit(50)
            };

            // loop over facility list items
            foreach (var epaFacilityListItem in epaFacilityList)
            {
                // loop over facility and well details
                foreach (var element in epaFacilityListItem.Elements())
                {
                    var type = element.Name;
                    var idElement = element.Elements().First();
                    var idTag = idElement.Name;
                    var id = idElement.Value;

                    // try find uic equivalent
                    var uicElement = uicFacilities
                        .Elements(type)
                        .SingleOrDefault(x => x.Element(idTag).Value == id);

                    if (uicElement == null)
                    {
                        Console.WriteLine("Missing {1} element: {0}", id, type.LocalName);
                        continue;
                    }

                    var result = comparer.Compare(element, uicElement);

                    if (!result.IsEqual)
                    {
                        foreach (var item in result.Differences)
                        {
                            Console.WriteLine("State: {0}", item.State);
                            Console.WriteLine("Comparison: {0}", item.Difference);
                        }
                    }
                }

//                try
//                {
//                    var uicFacility = uicFacilities.Single(x => x.Element(Uic + "FacilityDetail")
//                        .Element(Uic + "FacilityIdentifier")
//                        .Value == facilityId);
//                }
//                catch (Exception)
//                {
//                    Console.WriteLine("UIC missing facilitiy {0}", facilityId);
//                    continue;
//                }


            }

            Console.Write("Done comparing");
            Console.ReadKey();
            
        }
    }
}