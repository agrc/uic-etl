using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;
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

            var uicContainerNode = uic.Root.Descendants(Uic + "UIC");
            var uicFacilities = uicContainerNode.Elements(Uic + "FacilityList").OrderBy(sortFacilities);

            var skips = new[] { "LatitudeMeasure", "LongitudeMeasure", "LocationAccuracyValueMeasure", "WellContactIdentifier" };
            var selector = ElementSelectors.ConditionalBuilder()
                        .WhenElementIsNamed("WellInspectionDetail").ThenUse(ByNameAndTextRecSelector.CanBeCompared)
                        .ElseUse(ElementSelectors.ByNameAndText)
                        .Build();

            var nodeMatcher = new DefaultNodeMatcher(selector);

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
                    
                    var myDiff = DiffBuilder.Compare(element)
                        .WithTest(uicElement)
                        .CheckForSimilar()
                        .WithNodeMatcher(nodeMatcher)
                        .WithNodeFilter(x => !skips.Contains(x.LocalName))
                        .IgnoreComments()
                        .IgnoreWhitespace()
                        .NormalizeWhitespace()
                        .Build();

                    if (myDiff.HasDifferences())
                    {
                        Debug.WriteLine(id);
                        foreach (var item in myDiff.Differences)
                        {
                            Debug.WriteLine(item.Comparison);
                        }
                    }
                }
            }

            Console.WriteLine("Done comparing");
            Console.ReadKey();
        }
    }
}