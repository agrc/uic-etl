using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using domain.uic_etl.xml;
using uic_etl.services;
using Xunit;
using Xunit.Abstractions;

namespace tests.uic_etl
{
    public class XmlWritingTests
    {
        private readonly ITestOutputHelper _output;

        public XmlWritingTests(ITestOutputHelper output)
        {
            _output = output;
        }

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
                                       "<FacilityDetail>" +
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
            var model = new FacilityDetail
            {
                FacilityIdentifier = 0,
                FacilityPetitionStatusCode = "FacilityPetitionStatusCode",
                FacilitySiteName = "FacilitySiteName",
                FacilitySiteTypeCode = "FacilitySiteTypeCode",
                FacilityStateIdentifier = "FacilityStateIdentifier",
                FacilityViolationDetail = new List<ViolationDetail>(),
                LocalityName = "LocalityName",
                LocationAddressPostalCode = "LocationAddressPostalCode",
                LocationAddressStateCode = "LocationAddressStateCode",
                LocationAddressText = "LocationAddressText"
            };

            XmlService.AddFacility(ref doc, model);

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void AddFacilityListItemPayloadWithViolation()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UDEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>0</FacilityIdentifier>" +
                                       "<LocalityName>LocalityName</LocalityName>" +
                                       "<FacilitySiteName>FacilitySiteName</FacilitySiteName>" +
                                       "<FacilityPetitionStatusCode>FacilityPetitionStatusCode</FacilityPetitionStatusCode>" +
                                       "<LocationAddressStateCode>LocationAddressStateCode</LocationAddressStateCode>" +
                                       "<FacilityStateIdentifier>FacilityStateIdentifier</FacilityStateIdentifier>" +
                                       "<LocationAddressText>LocationAddressText</LocationAddressText>" +
                                       "<FacilitySiteTypeCode>FacilitySiteTypeCode</FacilitySiteTypeCode>" +
                                       "<LocationAddressPostalCode>LocationAddressPostalCode</LocationAddressPostalCode>" +
                                       "<FacilityViolationDetail>" +
                                       "<ViolationIdentifier>0</ViolationIdentifier>" +
                                       "<ViolationContaminationCode>[UICViolation].USDWContamination</ViolationContaminationCode>" +
                                       "<ViolationEndangeringCode>[UICViolation].ENDANGER</ViolationEndangeringCode>" +
                                       "<ViolationReturnComplianceDate>20160104</ViolationReturnComplianceDate>" +
                                       "<ViolationSignificantCode>[UICViolation].SignificantNonCompliance</ViolationSignificantCode>" +
                                       "<ViolationDeterminedDate>20160101</ViolationDeterminedDate>" +
                                       "<ViolationTypeCode>[UICViolation].ViolationType</ViolationTypeCode>" +
                                       "<ViolationFacilityIdentifier>45c1be51-c4e3-4159-95fb-36f7e9a95585</ViolationFacilityIdentifier>" +
                                       "</FacilityViolationDetail></FacilityDetail></FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();
            var model = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = 0,
                FacilityPetitionStatusCode = "FacilityPetitionStatusCode",
                FacilitySiteName = "FacilitySiteName",
                FacilitySiteTypeCode = "FacilitySiteTypeCode",
                FacilityStateIdentifier = "FacilityStateIdentifier",
                FacilityViolationDetail = new List<ViolationDetail>
                {
                  new ViolationDetail
                  {
                      ViolationContaminationCode = "[UICViolation].USDWContamination",
                      ViolationDeterminedDate = "20160101",
                      ViolationEndangeringCode = "[UICViolation].ENDANGER",
                      ViolationFacilityIdentifier = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                      ViolationIdentifier = 0,
                      ViolationReturnComplianceDate = "20160104",
                      ViolationSignificantCode = "[UICViolation].SignificantNonCompliance",
                      ViolationTypeCode = "[UICViolation].ViolationType"
                  }  
                },
                LocalityName = "LocalityName",
                LocationAddressPostalCode = "LocationAddressPostalCode",
                LocationAddressStateCode = "LocationAddressStateCode",
                LocationAddressText = "LocationAddressText"
            };

            XmlService.AddFacility(ref doc, model);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void AddFacilityListItemPayloadWithViolationAndResponse()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UDEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>0</FacilityIdentifier>" +
                                       "<LocalityName>LocalityName</LocalityName>" +
                                       "<FacilitySiteName>FacilitySiteName</FacilitySiteName>" +
                                       "<FacilityPetitionStatusCode>FacilityPetitionStatusCode</FacilityPetitionStatusCode>" +
                                       "<LocationAddressStateCode>LocationAddressStateCode</LocationAddressStateCode>" +
                                       "<FacilityStateIdentifier>FacilityStateIdentifier</FacilityStateIdentifier>" +
                                       "<LocationAddressText>LocationAddressText</LocationAddressText>" +
                                       "<FacilitySiteTypeCode>FacilitySiteTypeCode</FacilitySiteTypeCode>" +
                                       "<LocationAddressPostalCode>LocationAddressPostalCode</LocationAddressPostalCode>" +
                                       "<FacilityViolationDetail>" +
                                       "<ViolationIdentifier>0</ViolationIdentifier>" +
                                       "<ViolationContaminationCode>[UICViolation].USDWContamination</ViolationContaminationCode>" +
                                       "<ViolationEndangeringCode>[UICViolation].ENDANGER</ViolationEndangeringCode>" +
                                       "<ViolationReturnComplianceDate>20160104</ViolationReturnComplianceDate>" +
                                       "<ViolationSignificantCode>[UICViolation].SignificantNonCompliance</ViolationSignificantCode>" +
                                       "<ViolationDeterminedDate>20160101</ViolationDeterminedDate>" +
                                       "<ViolationTypeCode>[UICViolation].ViolationType</ViolationTypeCode>" +
                                       "<ViolationFacilityIdentifier>45c1be51-c4e3-4159-95fb-36f7e9a95585</ViolationFacilityIdentifier>" +
                                       " <FacilityResponseDetail>" +
                                       "<ResponseEnforcementIdentifier>0</ResponseEnforcementIdentifier>" +
                                       "<ResponseViolationIdentifier>0001be51-c4e3-4159-95fb-36f7e9a95585</ResponseViolationIdentifier>" +
                                       "</FacilityResponseDetail></FacilityViolationDetail></FacilityDetail></FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();
            var model = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = 0,
                FacilityPetitionStatusCode = "FacilityPetitionStatusCode",
                FacilitySiteName = "FacilitySiteName",
                FacilitySiteTypeCode = "FacilitySiteTypeCode",
                FacilityStateIdentifier = "FacilityStateIdentifier",
                FacilityViolationDetail = new List<ViolationDetail>
                {
                  new ViolationDetail
                  {
                      Guid = new Guid("0001be51-c4e3-4159-95fb-36f7e9a95585"),
                      ViolationContaminationCode = "[UICViolation].USDWContamination",
                      ViolationDeterminedDate = "20160101",
                      ViolationEndangeringCode = "[UICViolation].ENDANGER",
                      ViolationFacilityIdentifier = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                      ViolationIdentifier = 0,
                      ViolationReturnComplianceDate = "20160104",
                      ViolationSignificantCode = "[UICViolation].SignificantNonCompliance",
                      ViolationTypeCode = "[UICViolation].ViolationType",
                      ResponseDetails = new List<ResponseDetail>
                      {
                          new ResponseDetail
                          {
                              ResponseViolationIdentifier = new Guid("0001be51-c4e3-4159-95fb-36f7e9a95585"),
                              ResponseEnforcementIdentifier = 0
                          }
                      }
                  }  
                },
                LocalityName = "LocalityName",
                LocationAddressPostalCode = "LocationAddressPostalCode",
                LocationAddressStateCode = "LocationAddressStateCode",
                LocationAddressText = "LocationAddressText"
            };

            XmlService.AddFacility(ref doc, model);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }
    }
}