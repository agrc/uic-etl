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

            // loop over facility list items
            foreach (var epaFacilityListItem in epaFacilityList)
            {
                // loop over facility and well details
                foreach (var element in epaFacilityListItem.Elements())
                {
                    var type = element.Name;
                    var tags = element.Elements().OrderBy(x => x.Name.LocalName);
                    var epaCursor = tags.GetEnumerator();
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

                    var uicCursor = uicElement.Elements().OrderBy(x => x.Name.LocalName).GetEnumerator();

                    epaCursor.MoveNext();
                    uicCursor.MoveNext();

                    do
                    {
                        var testMatch = epaCursor.Current;
                        var match = uicCursor.Current;

                        if (testMatch.Name != match.Name)
                        {
                            Console.WriteLine("{0} != {1}", testMatch.Name, match.Name);
                        }

                        if (skipTags.Contains(testMatch.Name.LocalName))
                        {
                            continue;
                        }

                        if (testMatch.Value != match.Value)
                        {
                            Console.WriteLine("{0} != {1} for {2}", testMatch.Value, match.Value, testMatch.Name);
                        }
                    } while (uicCursor.MoveNext() && epaCursor.MoveNext());
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