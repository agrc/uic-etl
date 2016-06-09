using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using domain.uic_etl.xml;
using uic_etl.services;
using Xunit;

namespace tests.uic_etl
{
    public class XmlWritingTests
    {
        [Fact]
        public void CreateDocument()
        {
            const string documentXml =
                "<Document xsi:schemaLocation=\"xmlns http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\" " +
                "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "Id=\"6a43a7c5-0510-41ff-a15d-39657d55153d\" " +
                "xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\" />";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreateDocument();

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void CreateHeader()
        {
            const string documentXml = "<Header><Author>CANDACE CADY</Author>" +
                                       "<Organization>UDEQ -- UDAH DEPARTMENT OF ENVIRONMENTAL QUALITY</Organization>" +
                                       "<Title>DATA SUBMISSION FOR QUARTER #1, FY 2010</Title><CreationTime>2010-03-24T16:22:08</CreationTime>" +
                                       "<Comment>THIS IS A SAMPLE XML DOC V.2</Comment><DataService>UIC</DataService>" +
                                       "<ContactInfo>CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352</ContactInfo>" +
                                       "<Notification>AGRC@UTAH.GOV</Notification>" +
                                       "<Sensitivity>UNCLASSIFIED</Sensitivity></Header>";

            var expected = XDocument.Parse(documentXml);

            var root = new XElement(XName.Get("Header"),
                new XElement(XName.Get("Author"), "CANDACE CADY"),
                new XElement(XName.Get("Organization"), "UDEQ -- UDAH DEPARTMENT OF ENVIRONMENTAL QUALITY"),
                new XElement(XName.Get("Title"), "DATA SUBMISSION FOR QUARTER #1, FY 2010"),
                new XElement(XName.Get("CreationTime"), "2010-03-24T16:22:08"),
                new XElement(XName.Get("Comment"), "THIS IS A SAMPLE XML DOC V.2"),
                new XElement(XName.Get("DataService"), "UIC"),
                new XElement(XName.Get("ContactInfo"),
                    "CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352"),
                new XElement(XName.Get("Notification"), "AGRC@UTAH.GOV"),
                new XElement(XName.Get("Sensitivity"), "UNCLASSIFIED"));

            Assert.Equal(expected.ToString(), root.ToString());
        }

        [Fact]
        public void CreatePayload()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UDEQ</PrimacyAgencyCode></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void AddFacilityListItemPayload()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UDEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<FacilityIdentifier>0</FacilityIdentifier>" +
                                       "<LocalityName>LocalityName</LocalityName>" +
                                       "<FacilitySiteName>FacilitySiteName</FacilitySiteName>" +
                                       "<FacilityPetitionStatusCode>FacilityPetitionStatusCode</FacilityPetitionStatusCode>" +
                                       "<LocationAddressStateCode>LocationAddressStateCode</LocationAddressStateCode>" +
                                       "<FacilityStateIdentifier>FacilityStateIdentifier</FacilityStateIdentifier>" +
                                       "<LocationAddressText>LocationAddressText</LocationAddressText>" +
                                       "<FacilitySiteTypeCode>FacilitySiteTypeCode</FacilitySiteTypeCode>" +
                                       "<LocationAddressPostalCode>LocationAddressPostalCode</LocationAddressPostalCode>" +
                                       "</FacilityDetail></FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();
            var model = new FacilityDetailModel
            {
                FacilityIdentifier = 0,
                FacilityPetitionStatusCode = "FacilityPetitionStatusCode",
                FacilitySiteName = "FacilitySiteName",
                FacilitySiteTypeCode = "FacilitySiteTypeCode",
                FacilityStateIdentifier = "FacilityStateIdentifier",
                FacilityViolationDetail = new List<FacilityViolationDetail>(),
                LocalityName = "LocalityName",
                LocationAddressPostalCode = "LocationAddressPostalCode",
                LocationAddressStateCode = "LocationAddressStateCode",
                LocationAddressText = "LocationAddressText"
            };

            XmlService.AddFacility(ref doc, model);

            Assert.Equal(expected.ToString(), doc.ToString());
        }
    }
}