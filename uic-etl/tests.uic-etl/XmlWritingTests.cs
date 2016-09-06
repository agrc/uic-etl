using System;
using System.Collections.Generic;
using System.Xml.Linq;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using uic_etl.models.dtos;
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

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void CreateHeader()
        {
            const string documentXml = "<Header><Author>CANDACE CADY</Author>" +
                                       "<Organization>UTEQ -- UDAH DEPARTMENT OF ENVIRONMENTAL QUALITY</Organization>" +
                                       "<Title>DATA SUBMISSION FOR QUARTER #1, FY 2010</Title><CreationTime>2010-03-24T16:22:08</CreationTime>" +
                                       "<Comment>THIS IS A SAMPLE XML DOC V.2</Comment><DataService>UIC</DataService>" +
                                       "<ContactInfo>CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352</ContactInfo>" +
                                       "<Notification>AGRC@UTAH.GOV</Notification>" +
                                       "<Sensitivity>UNCLASSIFIED</Sensitivity></Header>";

            var expected = XDocument.Parse(documentXml);

            var root = new XElement(XName.Get("Header"),
                new XElement(XName.Get("Author"), "CANDACE CADY"),
                new XElement(XName.Get("Organization"), "UTEQ -- UDAH DEPARTMENT OF ENVIRONMENTAL QUALITY"),
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
        public void CreateHeaderInDocument()
        {
            const string documentXml =
                "<Document xsi:schemaLocation=\"xmlns http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Id=\"6a43a7c5-0510-41ff-a15d-39657d55153d\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                "<Header>" +
                "<Author>CANDACE CADY</Author>" +
                "<Organization>UTEQ -- UTAH DEPARTMENT OF ENVIRONMENTAL QUALITY</Organization>" +
                "<Title>data submission for quarter #1, fy 2010</Title>" +
                "<CreationTime>2016-08-15T16:20:32</CreationTime>" +
                "<Comment>This is a sample</Comment>" +
                "<DataService>UIC</DataService>" +
                "<ContactInfo>CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352</ContactInfo>" +
                "<Notification>AGRC@UTAH.GOV</Notification>" +
                "<Sensitivity>UNCLASSIFIED</Sensitivity>" +
                "</Header></Document>";

            var doc = XmlService.CreateDocument();
            var headerModel = new HeaderInformation
            {
                Title = "data submission for quarter #1, fy 2010",
                CreationTime = new DateTime(2016, 8, 15, 16, 20, 32).ToString("s"),
                Comments = "This is a sample"
            };

            XmlService.AppendHeader(ref doc, headerModel);

            var expected = XDocument.Parse(documentXml);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void CreatePayload()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void CreatePayloadInDocument()
        {
            const string documentXml =
                 "<Document xsi:schemaLocation=\"xmlns http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" Id=\"6a43a7c5-0510-41ff-a15d-39657d55153d\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                 "<Header>" +
                 "<Author>CANDACE CADY</Author>" +
                 "<Organization>UTEQ -- UTAH DEPARTMENT OF ENVIRONMENTAL QUALITY</Organization>" +
                 "<Title>data submission for quarter #1, fy 2010</Title>" +
                 "<CreationTime>2016-08-15T16:20:32</CreationTime>" +
                 "<Comment>This is a sample</Comment>" +
                 "<DataService>UIC</DataService>" +
                 "<ContactInfo>CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352</ContactInfo>" +
                 "<Notification>AGRC@UTAH.GOV</Notification>" +
                 "<Sensitivity>UNCLASSIFIED</Sensitivity>" +
                 "</Header><Payload Operation=\"Delete - Insert\">" +
                 "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                 "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode></UIC></Payload>" +
                 "</Document>";

            var doc = XmlService.CreateDocument();
            var headerModel = new HeaderInformation
            {
                Title = "data submission for quarter #1, fy 2010",
                CreationTime = new DateTime(2016, 8, 15, 16, 20, 32).ToString("s"),
                Comments = "This is a sample"
            };

            XmlService.AppendHeader(ref doc, headerModel);
            var payload = XmlService.CreatePayloadElements();
            doc.Root.Add(payload);

            var expected = XDocument.Parse(documentXml);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString()); 
        }

        [Fact]
        public void AddFacilityListItemPayload()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
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
                FacilityIdentifier = "FacilityIdentifier",
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
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
                                       "<LocalityName>LocalityName</LocalityName>" +
                                       "<FacilitySiteName>FacilitySiteName</FacilitySiteName>" +
                                       "<FacilityPetitionStatusCode>FacilityPetitionStatusCode</FacilityPetitionStatusCode>" +
                                       "<LocationAddressStateCode>LocationAddressStateCode</LocationAddressStateCode>" +
                                       "<FacilityStateIdentifier>FacilityStateIdentifier</FacilityStateIdentifier>" +
                                       "<LocationAddressText>LocationAddressText</LocationAddressText>" +
                                       "<FacilitySiteTypeCode>FacilitySiteTypeCode</FacilitySiteTypeCode>" +
                                       "<LocationAddressPostalCode>LocationAddressPostalCode</LocationAddressPostalCode>" +
                                       "<FacilityViolationDetail>" +
                                       "<ViolationIdentifier>ViolationIdentifier</ViolationIdentifier>" +
                                       "<ViolationContaminationCode>ViolationContaminationCode</ViolationContaminationCode>" +
                                       "<ViolationEndangeringCode>ViolationEndangeringCode</ViolationEndangeringCode>" +
                                       "<ViolationReturnComplianceDate>ViolationReturnComplianceDate</ViolationReturnComplianceDate>" +
                                       "<ViolationSignificantCode>ViolationSignificantCode</ViolationSignificantCode>" +
                                       "<ViolationDeterminedDate>ViolationDeterminedDate</ViolationDeterminedDate>" +
                                       "<ViolationTypeCode>ViolationTypeCode</ViolationTypeCode>" +
                                       "<ViolationFacilityIdentifier>ViolationFacilityIdentifier</ViolationFacilityIdentifier>" +
                                       "</FacilityViolationDetail></FacilityDetail></FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();
            var model = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = "FacilityIdentifier",
                FacilityPetitionStatusCode = "FacilityPetitionStatusCode",
                FacilitySiteName = "FacilitySiteName",
                FacilitySiteTypeCode = "FacilitySiteTypeCode",
                FacilityStateIdentifier = "FacilityStateIdentifier",
                FacilityViolationDetail = new List<ViolationDetail>
                {
                    new ViolationDetail
                    {
                        ViolationContaminationCode = "ViolationContaminationCode",
                        ViolationDeterminedDate = "ViolationDeterminedDate",
                        ViolationEndangeringCode = "ViolationEndangeringCode",
                        ViolationFacilityIdentifier = "ViolationFacilityIdentifier",
                        ViolationIdentifier = "ViolationIdentifier",
                        ViolationReturnComplianceDate = "ViolationReturnComplianceDate",
                        ViolationSignificantCode = "ViolationSignificantCode",
                        ViolationTypeCode = "ViolationTypeCode"
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
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
                                       "<LocalityName>LocalityName</LocalityName>" +
                                       "<FacilitySiteName>FacilitySiteName</FacilitySiteName>" +
                                       "<FacilityPetitionStatusCode>FacilityPetitionStatusCode</FacilityPetitionStatusCode>" +
                                       "<LocationAddressStateCode>LocationAddressStateCode</LocationAddressStateCode>" +
                                       "<FacilityStateIdentifier>FacilityStateIdentifier</FacilityStateIdentifier>" +
                                       "<LocationAddressText>LocationAddressText</LocationAddressText>" +
                                       "<FacilitySiteTypeCode>FacilitySiteTypeCode</FacilitySiteTypeCode>" +
                                       "<LocationAddressPostalCode>LocationAddressPostalCode</LocationAddressPostalCode>" +
                                       "<FacilityViolationDetail>" +
                                       "<ViolationIdentifier>ViolationIdentifier</ViolationIdentifier>" +
                                       "<ViolationContaminationCode>[UICViolation].USDWContamination</ViolationContaminationCode>" +
                                       "<ViolationEndangeringCode>[UICViolation].ENDANGER</ViolationEndangeringCode>" +
                                       "<ViolationReturnComplianceDate>20160104</ViolationReturnComplianceDate>" +
                                       "<ViolationSignificantCode>[UICViolation].SignificantNonCompliance</ViolationSignificantCode>" +
                                       "<ViolationDeterminedDate>20160101</ViolationDeterminedDate>" +
                                       "<ViolationTypeCode>[UICViolation].ViolationType</ViolationTypeCode>" +
                                       "<ViolationFacilityIdentifier>ViolationFacilityIdentifier</ViolationFacilityIdentifier>" +
                                       " <FacilityResponseDetail>" +
                                       "<ResponseEnforcementIdentifier>ResponseEnforcementIdentifier</ResponseEnforcementIdentifier>" +
                                       "<ResponseViolationIdentifier>ResponseViolationIdentifier</ResponseViolationIdentifier>" +
                                       "</FacilityResponseDetail></FacilityViolationDetail></FacilityDetail></FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);

            var doc = XmlService.CreatePayloadElements();
            var model = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = "FacilityIdentifier",
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
                        ViolationFacilityIdentifier = "ViolationFacilityIdentifier",
                        ViolationIdentifier = "ViolationIdentifier",
                        ViolationReturnComplianceDate = "20160104",
                        ViolationSignificantCode = "[UICViolation].SignificantNonCompliance",
                        ViolationTypeCode = "[UICViolation].ViolationType",
                        ResponseDetail = new List<ResponseDetail>
                        {
                            new ResponseDetail
                            {
                                ResponseViolationIdentifier = "ResponseViolationIdentifier",
                                ResponseEnforcementIdentifier = "ResponseEnforcementIdentifier"
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

        [Fact]
        public void AddWellDetailToFacilityList()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
                                       "<LocalityName /><FacilitySiteName /><FacilityPetitionStatusCode /><LocationAddressStateCode />" +
                                       "<FacilityStateIdentifier /><LocationAddressText /><FacilitySiteTypeCode /><LocationAddressPostalCode />" +
                                       "</FacilityDetail>" +
                                       "<WellDetail>" +
                                       "<WellIdentifier>WellIdentifier</WellIdentifier>" +
                                       "<WellAquiferExemptionInjectionCode>WellAquiferExemptionInjectionCode</WellAquiferExemptionInjectionCode>" +
                                       "<WellTotalDepthNumeric>WellTotalDepthNumeric</WellTotalDepthNumeric>" +
                                       "<WellHighPriorityDesignationCode>WellHighPriorityDesignationCode</WellHighPriorityDesignationCode>" +
                                       "<WellContactIdentifier>WellContactIdentifier</WellContactIdentifier>" +
                                       "<WellFacilityIdentifier>WellFacilityIdentifier</WellFacilityIdentifier>" +
                                       "<WellGeologyIdentifier>WellGeologyIdentifier</WellGeologyIdentifier>" +
                                       "<WellSiteAreaNameText>WellSiteAreaNameText</WellSiteAreaNameText>" +
                                       "<WellPermitIdentifier>WellPermitIdentifier</WellPermitIdentifier>" +
                                       "<WellStateIdentifier>WellStateIdentifier</WellStateIdentifier>" +
                                       "<WellStateTribalCode>WellStateTribalCode</WellStateTribalCode>" +
                                       "<WellInSourceWaterAreaLocationText>WellInSourceWaterAreaLocationText</WellInSourceWaterAreaLocationText>" +
                                       "<WellName>WellName</WellName></WellDetail>" +
                                       "</FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);
            var doc = XmlService.CreatePayloadElements();

            var facility = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = "FacilityIdentifier"
            };

            var facilityList = XmlService.AddFacility(ref doc, facility);

            var well = new WellDetail
            {
                WellIdentifier = "WellIdentifier",
                WellAquiferExemptionInjectionCode = "WellAquiferExemptionInjectionCode",
                WellTotalDepthNumeric = "WellTotalDepthNumeric",
                WellHighPriorityDesignationCode = "WellHighPriorityDesignationCode",
                WellContactIdentifier = "WellContactIdentifier",
                WellFacilityIdentifier = "WellFacilityIdentifier",
                WellGeologyIdentifier = "WellGeologyIdentifier",
                WellSiteAreaNameText = "WellSiteAreaNameText",
                WellPermitIdentifier = "WellPermitIdentifier",
                WellStateIdentifier = "WellStateIdentifier",
                WellStateTribalCode = "WellStateTribalCode",
                WellInSourceWaterAreaLocationText = "WellInSourceWaterAreaLocationText",
                WellName = "WellName"
            };

            XmlService.AddWell(ref facilityList, well);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void AddWellDetailToFacilityListWIthWellStatus()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
                                       "<LocalityName /><FacilitySiteName /><FacilityPetitionStatusCode /><LocationAddressStateCode />" +
                                       "<FacilityStateIdentifier /><LocationAddressText /><FacilitySiteTypeCode /><LocationAddressPostalCode />" +
                                       "</FacilityDetail>" +
                                       "<WellDetail>" +
                                       "<WellIdentifier>WellIdentifier</WellIdentifier>" +
                                       "<WellAquiferExemptionInjectionCode>WellAquiferExemptionInjectionCode</WellAquiferExemptionInjectionCode>" +
                                       "<WellTotalDepthNumeric>WellTotalDepthNumeric</WellTotalDepthNumeric>" +
                                       "<WellHighPriorityDesignationCode>WellHighPriorityDesignationCode</WellHighPriorityDesignationCode>" +
                                       "<WellContactIdentifier>WellContactIdentifier</WellContactIdentifier>" +
                                       "<WellFacilityIdentifier>WellFacilityIdentifier</WellFacilityIdentifier>" +
                                       "<WellGeologyIdentifier>WellGeologyIdentifier</WellGeologyIdentifier>" +
                                       "<WellSiteAreaNameText>WellSiteAreaNameText</WellSiteAreaNameText>" +
                                       "<WellPermitIdentifier>WellPermitIdentifier</WellPermitIdentifier>" +
                                       "<WellStateIdentifier>WellStateIdentifier</WellStateIdentifier>" +
                                       "<WellStateTribalCode>WellStateTribalCode</WellStateTribalCode>" +
                                       "<WellInSourceWaterAreaLocationText>WellInSourceWaterAreaLocationText</WellInSourceWaterAreaLocationText>" +
                                       "<WellName>WellName</WellName>" +
                                       "<WellStatusDetail>" +
                                       "<WellStatusIdentifier>WellStatusIdentifier</WellStatusIdentifier>" +
                                       "<WellStatusDate>WellStatusDate</WellStatusDate>" +
                                       "<WellStatusOperatingStatusCode>WellStatusOperatingStatusCode</WellStatusOperatingStatusCode>" +
                                       "<WellStatusWellIdentifier>WellStatusWellIdentifier</WellStatusWellIdentifier>" +
                                       "</WellStatusDetail><WellStatusDetail>" +
                                       "<WellStatusIdentifier>WellStatusIdentifier</WellStatusIdentifier>" +
                                       "<WellStatusDate>WellStatusDate</WellStatusDate>" +
                                       "<WellStatusOperatingStatusCode>WellStatusOperatingStatusCode</WellStatusOperatingStatusCode>" +
                                       "<WellStatusWellIdentifier>WellStatusWellIdentifier</WellStatusWellIdentifier>" +
                                       "</WellStatusDetail></WellDetail>" +
                                       "</FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);
            var doc = XmlService.CreatePayloadElements();

            var facility = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = "FacilityIdentifier"
            };

            var facilityList = XmlService.AddFacility(ref doc, facility);

            var well = new WellDetail
            {
                WellIdentifier = "WellIdentifier",
                WellAquiferExemptionInjectionCode = "WellAquiferExemptionInjectionCode",
                WellTotalDepthNumeric = "WellTotalDepthNumeric",
                WellHighPriorityDesignationCode = "WellHighPriorityDesignationCode",
                WellContactIdentifier = "WellContactIdentifier",
                WellFacilityIdentifier = "WellFacilityIdentifier",
                WellGeologyIdentifier = "WellGeologyIdentifier",
                WellSiteAreaNameText = "WellSiteAreaNameText",
                WellPermitIdentifier = "WellPermitIdentifier",
                WellStateIdentifier = "WellStateIdentifier",
                WellStateTribalCode = "WellStateTribalCode",
                WellInSourceWaterAreaLocationText = "WellInSourceWaterAreaLocationText",
                WellName = "WellName",
                WellStatusDetail = new List<WellStatusDetail>
                {
                    new WellStatusDetail
                    {
                        WellStatusIdentifier = "WellStatusIdentifier",
                        WellStatusDate = "WellStatusDate",
                        WellStatusOperatingStatusCode = "WellStatusOperatingStatusCode",
                        WellStatusWellIdentifier = "WellStatusWellIdentifier"
                    },
                     new WellStatusDetail
                    {
                        WellStatusIdentifier = "WellStatusIdentifier",
                        WellStatusDate = "WellStatusDate",
                        WellStatusOperatingStatusCode = "WellStatusOperatingStatusCode",
                        WellStatusWellIdentifier = "WellStatusWellIdentifier"
                    }
                }
            };

            XmlService.AddWell(ref facilityList, well);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void AddWellDetailToFacilityListWithLocation()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
                                       "<LocalityName /><FacilitySiteName /><FacilityPetitionStatusCode /><LocationAddressStateCode />" +
                                       "<FacilityStateIdentifier /><LocationAddressText /><FacilitySiteTypeCode /><LocationAddressPostalCode />" +
                                       "</FacilityDetail>" +
                                       "<WellDetail>" +
                                       "<WellIdentifier>WellIdentifier</WellIdentifier>" +
                                       "<WellAquiferExemptionInjectionCode>WellAquiferExemptionInjectionCode</WellAquiferExemptionInjectionCode>" +
                                       "<WellTotalDepthNumeric>WellTotalDepthNumeric</WellTotalDepthNumeric>" +
                                       "<WellHighPriorityDesignationCode>WellHighPriorityDesignationCode</WellHighPriorityDesignationCode>" +
                                       "<WellContactIdentifier>WellContactIdentifier</WellContactIdentifier>" +
                                       "<WellFacilityIdentifier>WellFacilityIdentifier</WellFacilityIdentifier>" +
                                       "<WellGeologyIdentifier>WellGeologyIdentifier</WellGeologyIdentifier>" +
                                       "<WellSiteAreaNameText>WellSiteAreaNameText</WellSiteAreaNameText>" +
                                       "<WellPermitIdentifier>WellPermitIdentifier</WellPermitIdentifier>" +
                                       "<WellStateIdentifier>WellStateIdentifier</WellStateIdentifier>" +
                                       "<WellStateTribalCode>WellStateTribalCode</WellStateTribalCode>" +
                                       "<WellInSourceWaterAreaLocationText>WellInSourceWaterAreaLocationText</WellInSourceWaterAreaLocationText>" +
                                       "<WellName>WellName</WellName>" +
                                       "<LocationDetail>" +
                                       "<LocationIdentifier>UTEQA6EC842436CD9606</LocationIdentifier>" +
                                       "<LocationAddressCounty>1</LocationAddressCounty>" +
                                       "<LocationAccuracyValueMeasure>U</LocationAccuracyValueMeasure>" +
                                       "<GeographicReferencePointCode>026</GeographicReferencePointCode>" +
                                       "<HorizontalCoordinateReferenceSystemDatumCode>002</HorizontalCoordinateReferenceSystemDatumCode>" +
                                       "<HorizontalCollectionMethodCode>Well.LocationMethod</HorizontalCollectionMethodCode>" +
                                       "<LocationPointLineAreaCode>001</LocationPointLineAreaCode>" +
                                       "<SourceMapScaleNumeric>Well.LocationAccuracy</SourceMapScaleNumeric>" +
                                       "<LocationWellIdentifier>UTEQA6EC842436CD9606</LocationWellIdentifier>" +
                                       "<LatitudeMeasure>50</LatitudeMeasure>" +
                                       "<LongitudeMeasure>40</LongitudeMeasure>" +
                                       "</LocationDetail></WellDetail>" +
                                       "</FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);
            var doc = XmlService.CreatePayloadElements();

            var facility = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = "FacilityIdentifier"
            };
            var wellGuid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95581");

            var facilityList = XmlService.AddFacility(ref doc, facility);

            var well = new WellDetail
            {
                WellIdentifier = "WellIdentifier",
                WellAquiferExemptionInjectionCode = "WellAquiferExemptionInjectionCode",
                WellTotalDepthNumeric = "WellTotalDepthNumeric",
                WellHighPriorityDesignationCode = "WellHighPriorityDesignationCode",
                WellContactIdentifier = "WellContactIdentifier",
                WellFacilityIdentifier = "WellFacilityIdentifier",
                WellGeologyIdentifier = "WellGeologyIdentifier",
                WellSiteAreaNameText = "WellSiteAreaNameText",
                WellPermitIdentifier = "WellPermitIdentifier",
                WellStateIdentifier = "WellStateIdentifier",
                WellStateTribalCode = "WellStateTribalCode",
                WellInSourceWaterAreaLocationText = "WellInSourceWaterAreaLocationText",
                WellName = "WellName",
                LocationDetail = new LocationDetail(new WellSdeModel
                {
                    Guid = wellGuid,
                    LocationAccuracy = "Well.LocationAccuracy",
                    LocationMethod = "Well.LocationMethod"
                }, new FacilitySdeModel
                {
                    CountyFips = 1
                }, 40, 50, guid => "UTEQA6EC842436CD9606")
            };

            XmlService.AddWell(ref facilityList, well);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

        [Fact]
        public void AddWellDetailToFacilityListWithWellViolation()
        {
            const string documentXml = "<Payload Operation=\"Delete - Insert\" xmlns=\"http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd\">" +
                                       "<UIC xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.exchangenetwork.net/schema/uic/2\">" +
                                       "<PrimacyAgencyCode>UTEQ</PrimacyAgencyCode>" +
                                       "<FacilityList>" +
                                       "<FacilityDetail>" +
                                       "<FacilityIdentifier>FacilityIdentifier</FacilityIdentifier>" +
                                       "<LocalityName /><FacilitySiteName /><FacilityPetitionStatusCode /><LocationAddressStateCode />" +
                                       "<FacilityStateIdentifier /><LocationAddressText /><FacilitySiteTypeCode /><LocationAddressPostalCode />" +
                                       "</FacilityDetail>" +
                                       "<WellDetail>" +
                                       "<WellIdentifier>WellIdentifier</WellIdentifier>" +
                                       "<WellAquiferExemptionInjectionCode>WellAquiferExemptionInjectionCode</WellAquiferExemptionInjectionCode>" +
                                       "<WellTotalDepthNumeric>WellTotalDepthNumeric</WellTotalDepthNumeric>" +
                                       "<WellHighPriorityDesignationCode>WellHighPriorityDesignationCode</WellHighPriorityDesignationCode>" +
                                       "<WellContactIdentifier>WellContactIdentifier</WellContactIdentifier>" +
                                       "<WellFacilityIdentifier>WellFacilityIdentifier</WellFacilityIdentifier>" +
                                       "<WellGeologyIdentifier>WellGeologyIdentifier</WellGeologyIdentifier>" +
                                       "<WellSiteAreaNameText>WellSiteAreaNameText</WellSiteAreaNameText>" +
                                       "<WellPermitIdentifier>WellPermitIdentifier</WellPermitIdentifier>" +
                                       "<WellStateIdentifier>WellStateIdentifier</WellStateIdentifier>" +
                                       "<WellStateTribalCode>WellStateTribalCode</WellStateTribalCode>" +
                                       "<WellInSourceWaterAreaLocationText>WellInSourceWaterAreaLocationText</WellInSourceWaterAreaLocationText>" +
                                       "<WellName>WellName</WellName>" +
                                       "<WellViolationDetail>" +
                                       "<ViolationIdentifier>ViolationIdentifier</ViolationIdentifier>" +
                                       "<ViolationContaminationCode>ViolationContaminationCode</ViolationContaminationCode>" +
                                       "<ViolationEndangeringCode>ViolationEndangeringCode</ViolationEndangeringCode>" +
                                       "<ViolationReturnComplianceDate>ViolationReturnComplianceDate</ViolationReturnComplianceDate>" +
                                       "<ViolationSignificantCode>ViolationSignificantCode</ViolationSignificantCode>" +
                                       "<ViolationDeterminedDate>ViolationDeterminedDate</ViolationDeterminedDate>" +
                                       "<ViolationTypeCode>ViolationTypeCode</ViolationTypeCode>" +
                                       "<ViolationWellIdentifier>ViolationWellIdentifier</ViolationWellIdentifier>" +
                                       "<WellResponseDetail>" +
                                       "<ResponseEnforcementIdentifier>ResponseEnforcementIdentifier</ResponseEnforcementIdentifier>" +
                                       "<ResponseViolationIdentifier>ResponseViolationIdentifier</ResponseViolationIdentifier>" +
                                       "</WellResponseDetail></WellViolationDetail></WellDetail></FacilityList></UIC></Payload>";

            var expected = XDocument.Parse(documentXml);
            var doc = XmlService.CreatePayloadElements();

            var facility = new FacilityDetail
            {
                Guid = new Guid("45c1be51-c4e3-4159-95fb-36f7e9a95585"),
                FacilityIdentifier = "FacilityIdentifier" 
            };

            var facilityList = XmlService.AddFacility(ref doc, facility);

            var well = new WellDetail
            {
                WellIdentifier = "WellIdentifier",
                WellAquiferExemptionInjectionCode = "WellAquiferExemptionInjectionCode",
                WellTotalDepthNumeric = "WellTotalDepthNumeric",
                WellHighPriorityDesignationCode = "WellHighPriorityDesignationCode",
                WellContactIdentifier = "WellContactIdentifier",
                WellFacilityIdentifier = "WellFacilityIdentifier",
                WellGeologyIdentifier = "WellGeologyIdentifier",
                WellSiteAreaNameText = "WellSiteAreaNameText",
                WellPermitIdentifier = "WellPermitIdentifier",
                WellStateIdentifier = "WellStateIdentifier",
                WellStateTribalCode = "WellStateTribalCode",
                WellInSourceWaterAreaLocationText = "WellInSourceWaterAreaLocationText",
                WellName = "WellName",
                WellViolationDetail = new List<ViolationDetail>
                {
                    new ViolationDetail
                    {
                        ViolationIdentifier = "ViolationIdentifier",
                        ViolationContaminationCode = "ViolationContaminationCode",
                        ViolationEndangeringCode = "ViolationEndangeringCode",
                        ViolationReturnComplianceDate = "ViolationReturnComplianceDate",
                        ViolationSignificantCode = "ViolationSignificantCode",
                        ViolationDeterminedDate = "ViolationDeterminedDate",
                        ViolationTypeCode = "ViolationTypeCode",
                        ViolationWellIdentifier = "ViolationWellIdentifier",
                        ResponseDetail = new List<ResponseDetail>
                        {
                            new ResponseDetail
                            {
                                ResponseEnforcementIdentifier = "ResponseEnforcementIdentifier",
                                ResponseViolationIdentifier = "ResponseViolationIdentifier"
                            }
                        }
                    }
                }
            };

            XmlService.AddWell(ref facilityList, well);

            _output.WriteLine(doc.ToString());
            _output.WriteLine(expected.ToString());

            Assert.Equal(expected.ToString(), doc.ToString());
        }

    }
}