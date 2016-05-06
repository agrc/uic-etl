using System.Xml.Linq;
using domain.uic_etl.xml;
using uic_etl.models.dtos;

namespace uic_etl.services
{
    public static class XmlService
    {
        private static readonly XNamespace Xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

        public static XDocument CreateDocument()
        {
            var xmlns = XNamespace.Get("http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd");

            var schemaLocation =
                XNamespace.Get("xmlns http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd");

            var doc = new XDocument
            {
                Declaration = new XDeclaration("1.0", "UTF-8", null)
            };

            var root = new XElement(xmlns + "Document",
                new XAttribute(Xsi + "schemaLocation", schemaLocation),
                new XAttribute(XNamespace.Xmlns + "xsi", Xsi),
                new XAttribute(XName.Get("Id"), "6a43a7c5-0510-41ff-a15d-39657d55153d"));

            doc.Add(root);

            return doc;
        }

        public static void AppendHeader(ref XDocument doc, HeaderInformation model)
        {
            if (doc.Root == null)
            {
                return;
            }

            doc.Root.Add(new XElement(XName.Get("Header"),
                new XElement(XName.Get("Author"), "CANDACE CADY"),
                new XElement(XName.Get("Organization"), "UDEQ -- UDAH DEPARTMENT OF ENVIRONMENTAL QUALITY"),
                new XElement(XName.Get("Title"), model.Title),
                new XElement(XName.Get("CreationTime"), model.CreationTime),
                new XElement(XName.Get("Comment"), model.Comments),
                new XElement(XName.Get("DataService"), "UIC"),
                new XElement(XName.Get("ContactInfo"),
                    "CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352"),
                new XElement(XName.Get("Notification"), "AGRC@UTAH.GOV"),
                new XElement(XName.Get("Sensitivity"), "UNCLASSIFIED")));
        }

        public static XElement CreatePayloadElements()
        {
            var xmlns = XNamespace.Get("http://www.exchangenetwork.net/schema/uic/2");

            var payload = new XElement(XName.Get("Payload"),
                new XAttribute(XName.Get("Operation"), "Delete - Insert"),
                new XElement(xmlns + "UIC", new XAttribute(XNamespace.Xmlns + "xsi", Xsi),
                    new XElement(xmlns + "PrimacyAgencyCode", "UDEQ"),
                    new XElement(xmlns + "FacilityList")));

            return payload;
        }

        public static void AddFacility(ref XDocument doc, FacilityDetailModel model)
        {
            
        }
    }
}