using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Org.XmlUnit.Builder;
using Org.XmlUnit.Diff;
using uic.epa_compare.services;
using CompareOptions = uic.epa_compare.models.CompareOptions;

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
            var nestedIdentifierSort = new Func<XElement, string>(element => element.Elements().First().Elements().First().Value);
            var directIdentifierSort = new Func<XElement, string>(element => element.Elements().First().Value);

            using (var reader = File.OpenText(options.EpaFile))
            using (var reader2 = File.OpenText(options.UicEtlFile))
            {
                epa = XDocument.Load(reader);
                uic = XDocument.Load(reader2);
            }

            var headingNode = Uic + "UIC";
            var contactNode = Uic + "ContactDetail";
            var permitNode = Uic + "PermitDetail";
            var enforcementNode = Uic + "EnforcementDetail";
            var facilityListNode = Uic + "FacilityList";
            
            var epaContainerNode = epa.Root.Descendants(headingNode);
            var epaContacts = epaContainerNode.Elements(contactNode);
            var epaPermits = epaContainerNode.Elements(permitNode);
            var epaEnforcements = epaContainerNode.Elements(enforcementNode);
            var epaFacilityList = epaContainerNode.Elements(facilityListNode);

            var uicContainerNode = uic.Root.Descendants(headingNode);
            var uicContacts = uicContainerNode.Elements(contactNode);
            var uicPermits = uicContainerNode.Elements(permitNode);
            var uicEnforcements = uicContainerNode.Elements(enforcementNode);
            var uicFacilities = uicContainerNode.Elements(facilityListNode);

            var skips = new[]
            {
                "LatitudeMeasure", "LongitudeMeasure", "WellContactIdentifier"
            };

            var selector = ElementSelectors.ConditionalBuilder()
                        .WhenElementIsNamed("WellInspectionDetail").ThenUse(ByNameAndTextRecSelector.CanBeCompared)
                        .WhenElementIsNamed("MITestDetail").ThenUse(ByNameAndTextRecSelector.CanBeCompared)
                        .ElseUse(ElementSelectors.ByNameAndText)
                        .Build();

            var nodeMatcher = new DefaultNodeMatcher(selector);

            foreach (var epaElement in epaContacts)
            {
                var element = epaElement.Elements().First();
                var id = element.Value;
                var idTag = element.Name;

                var uicMatch = uicContacts
                    .SingleOrDefault(x => x.Element(idTag).Value == id);

                if (uicMatch == null)
                {
                    Console.WriteLine("Missing {1} element: {0}", id, idTag.LocalName);
                    continue;
                }

                var myDiff = DiffBuilder.Compare(epaElement)
                        .WithTest(uicMatch)
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
            Console.WriteLine("Done comparing global contacts");

            Console.WriteLine("Comparing Permits");
            foreach (var epaElement in epaPermits)
            {
                var element = epaElement.Elements().First();
                var id = element.Value;
                var idTag = element.Name;

                var uicMatch = uicPermits
                    .SingleOrDefault(x => x.Element(idTag).Value == id);

                if (uicMatch == null)
                {
                    Console.WriteLine("Missing {1} element: {0}", id, idTag.LocalName);
                    continue;
                }

                var myDiff = DiffBuilder.Compare(epaElement)
                        .WithTest(uicMatch)
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
            Console.WriteLine("Done comparing global permits");

            Console.WriteLine("Comparing Enforcements");
            foreach (var epaElement in epaEnforcements)
            {
                var element = epaElement.Elements().First();
                var id = element.Value;
                var idTag = element.Name;

                var uicMatch = uicEnforcements
                    .SingleOrDefault(x => x.Element(idTag).Value == id);

                if (uicMatch == null)
                {
                    Console.WriteLine("Missing {1} element: {0}", id, idTag.LocalName);
                    continue;
                }

                var myDiff = DiffBuilder.Compare(epaElement)
                        .WithTest(uicMatch)
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
            Console.WriteLine("Done comparing global enforcements");
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