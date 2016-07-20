using System.Diagnostics.Contracts.Internal;
using System.Linq;
using System.Xml.Linq;
using domain.uic_etl.xml;
using uic_etl.models.dtos;

namespace uic_etl.services
{
    public static class XmlService
    {
        private static readonly XNamespace Xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
        private static readonly XNamespace Uic = "http://www.exchangenetwork.net/schema/uic/2";

        public static XDocument CreateDocument()
        {
            var xmlns = XNamespace.Get("http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd");

            var schemaLocation = XNamespace.Get("xmlns http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd");

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
                new XElement(XName.Get("Organization"), "UDEQ -- UTAH DEPARTMENT OF ENVIRONMENTAL QUALITY"),
                new XElement(XName.Get("Title"), model.Title),
                new XElement(XName.Get("CreationTime"), model.CreationTime),
                new XElement(XName.Get("Comment"), model.Comments),
                new XElement(XName.Get("DataService"), "UIC"),
                new XElement(XName.Get("ContactInfo"), "CANDACE CADY 195 NORTH 1950 WEST, SALT LAKE CITY UT 84114, (801) 536-4352"),
                new XElement(XName.Get("Notification"), "AGRC@UTAH.GOV"),
                new XElement(XName.Get("Sensitivity"), "UNCLASSIFIED")));
        }

        public static XElement CreatePayloadElements()
        {
            XNamespace xmlns = "http://www.exchangenetwork.net/schema/uic/2";

            var payload = new XElement(XName.Get("Payload"),
                new XAttribute(XName.Get("Operation"), "Delete - Insert"),
                new XElement(xmlns + "UIC", new XAttribute(XNamespace.Xmlns + "xsi", Xsi),
                    new XElement(xmlns + "PrimacyAgencyCode", "UDEQ")));

            return payload;
        }

        public static XElement AddFacility(ref XElement payload, FacilityDetail model)
        {
            var facilityDetail = new XElement(Uic + "FacilityList",
                new XElement(Uic + "FacilityDetail",
                new XElement(Uic + "FacilityIdentifier", model.FacilityIdentifier),
                new XElement(Uic + "LocalityName", model.LocalityName),
                new XElement(Uic + "FacilitySiteName", model.FacilitySiteName),
                new XElement(Uic + "FacilityPetitionStatusCode", model.FacilityPetitionStatusCode),
                new XElement(Uic + "LocationAddressStateCode", model.LocationAddressStateCode),
                new XElement(Uic + "FacilityStateIdentifier", model.FacilityStateIdentifier),
                new XElement(Uic + "LocationAddressText", model.LocationAddressText),
                new XElement(Uic + "FacilitySiteTypeCode", model.FacilitySiteTypeCode),
                new XElement(Uic + "LocationAddressPostalCode", model.LocationAddressPostalCode)));

            var violationIdentifier = 0;
            foreach (var violationModel in model.FacilityViolationDetail)
            {
                var violationDetail = new XElement(Uic + "FacilityViolationDetail",
                    new XElement(Uic + "ViolationIdentifier", violationIdentifier++),
                    new XElement(Uic + "ViolationContaminationCode", violationModel.ViolationContaminationCode),
                    new XElement(Uic + "ViolationEndangeringCode", violationModel.ViolationEndangeringCode),
                    new XElement(Uic + "ViolationReturnComplianceDate", violationModel.ViolationReturnComplianceDate),
                    new XElement(Uic + "ViolationSignificantCode", violationModel.ViolationSignificantCode),
                    new XElement(Uic + "ViolationDeterminedDate", violationModel.ViolationDeterminedDate),
                    new XElement(Uic + "ViolationTypeCode", violationModel.ViolationTypeCode),
                    new XElement(Uic + "ViolationFacilityIdentifier", violationModel.ViolationFacilityIdentifier));

                var facilityDetailElement = facilityDetail.Element(Uic + "FacilityDetail");
                if (facilityDetailElement != null)
                {
                    facilityDetailElement.Add(violationDetail);
                }

                var enforcementIdentfier = 0;
                foreach (var responseModel in violationModel.ResponseDetails)
                {
                    var responseDetail = new XElement(Uic + "FacilityResponseDetail",
                        new XElement(Uic + "ResponseEnforcementIdentifier", enforcementIdentfier++),
                        new XElement(Uic + "ResponseViolationIdentifier", responseModel.ResponseViolationIdentifier));

                    violationDetail.Add(responseDetail);
                }
            }

            var node = payload.Descendants(Uic + "UIC").SingleOrDefault();
            if (node != null)
            {
                node.Add(facilityDetail);
            }

            return facilityDetail;
        }

        public static XElement AddWell(ref XElement facilityList, WellDetail model)
        {
            var wellDetail = new XElement(Uic + "WellDetail",
                new XElement(Uic + "WellIdentifier", model.WellIdentifier),
                new XElement(Uic + "WellAquiferExemptionInjectionCode", model.WellAquiferExemptionInjectionCode),
                new XElement(Uic + "WellTotalDepthNumeric", model.WellTotalDepthNumeric),
                new XElement(Uic + "WellHighPriorityDesignationCode", model.WellHighPriorityDesignationCode),
                new XElement(Uic + "WellContactIdentifier", model.WellContactIdentifier),
                new XElement(Uic + "WellFacilityIdentifier", model.WellFacilityIdentifier),
                new XElement(Uic + "WellGeologyIdentifier", model.WellGeologyIdentifier),
                new XElement(Uic + "WellSiteAreaNameText", model.WellSiteAreaNameText),
                new XElement(Uic + "WellPermitIdentifier", model.WellPermitIdentifier),
                new XElement(Uic + "WellStateIdentifier", model.WellStateIdentifier),
                new XElement(Uic + "WellStateTribalCode", model.WellStateTribalCode),
                new XElement(Uic + "WellInSourceWaterAreaLocationText", model.WellInSourceWaterAreaLocationText),
                new XElement(Uic + "WellName", model.WellName));

            foreach (var wellStatus in model.WellStatusDetail)
            {
                var wellStatusDetail = new XElement(Uic + "WellStatusDetail",
                    new XElement(Uic + "WellStatusIdentifier", wellStatus.WellStatusIdentifier),
                    new XElement(Uic + "WellStatusDate", wellStatus.WellStatusDate),
                    new XElement(Uic + "WellStatusOperatingStatusCode", wellStatus.WellStatusOperatingStatusCode),
                    new XElement(Uic + "WellStatusWellIdentifier", wellStatus.WellStatusWellIdentifier));

                wellDetail.Add(wellStatusDetail);
            }

            facilityList.Add(wellDetail);

            return facilityList;
        }
    }
}