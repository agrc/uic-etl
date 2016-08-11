using System.Collections.Generic;
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

        public static XElement AppendPayloadElements(ref XElement payload, IEnumerable<ContactDetail> contacts, IEnumerable<PermitDetail> permits)
        {
            var node = payload.Descendants(Uic + "UIC").SingleOrDefault();

            foreach (var contact in contacts)
            {
                var contactDetail = new XElement(Uic + "ContactDetail",
                    new XElement(Uic + "", contact.ContactIdentifier),
                    new XElement(Uic + "", contact.TelephoneNumberText),
                    new XElement(Uic + "", contact.IndividualFullName),
                    new XElement(Uic + "", contact.ContactCityName),
                    new XElement(Uic + "", contact.ContactAddressStateCode),
                    new XElement(Uic + "", contact.ContactAddressText),
                    new XElement(Uic + "", contact.ContactAddressPostalCode));

                node.Add(contactDetail);
            }

            foreach (var permit in permits)
            {
                var permitDetail = new XElement(Uic + "PermitDetail",
                    new XElement(Uic + "", permit.PermitAorWellNumberNumeric),
                    new XElement(Uic + "", permit.PermitAuthorizedStatusCode),
                    new XElement(Uic + "", permit.PermitOwnershipTypeCode),
                    new XElement(Uic + "", permit.PermitAuthorizedIdentifier),
                    new XElement(Uic + "", permit.PermitIdentifier));

                foreach (var activity in permit.PermitActivityDetail)
                {
                    var activityDetail = new XElement(Uic + "PermitActivityDetail",
                        new XElement(Uic + "", activity.PermitActivityIdentifier),
                        new XElement(Uic + "", activity.PermitActivityActionTypeCode),
                        new XElement(Uic + "", activity.PermitActivityDate),
                        new XElement(Uic + "", activity.PermitActivityPermitIdentifier));

                    permitDetail.Add(activityDetail);
                }

                node.Add(permitDetail);
            }

            return node;
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

            foreach (var violationModel in model.FacilityViolationDetail)
            {
                var violationDetail = new XElement(Uic + "FacilityViolationDetail",
                    new XElement(Uic + "ViolationIdentifier", violationModel.ViolationIdentifier),
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

                foreach (var responseModel in violationModel.ResponseDetail)
                {
                    var responseDetail = new XElement(Uic + "FacilityResponseDetail",
                        new XElement(Uic + "ResponseEnforcementIdentifier", responseModel.ResponseEnforcementIdentifier),
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

            if (model.LocationDetail != null)
            {
                wellDetail.Add(new XElement(Uic + "LocationDetail",
                    new XElement(Uic + "LocationIdentifier", model.LocationDetail.LocationIdentifier),
                    new XElement(Uic + "LocationAddressCounty", model.LocationDetail.LocationAddressCounty),
                    new XElement(Uic + "LocationAccuracyValueMeasure", model.LocationDetail.LocationAccuracyValueMeasure),
                    new XElement(Uic + "GeographicReferencePointCode", model.LocationDetail.GeographicReferencePointCode),
                    new XElement(Uic + "HorizontalCoordinateReferenceSystemDatumCode", model.LocationDetail.HorizontalCoordinateReferenceSystemDatumCode),
                    new XElement(Uic + "HorizontalCollectionMethodCode", model.LocationDetail.HorizontalCollectionMethodCode),
                    new XElement(Uic + "LocationPointLineAreaCode", model.LocationDetail.LocationPointLineAreaCode),
                    new XElement(Uic + "SourceMapScaleNumeric", model.LocationDetail.SourceMapScaleNumeric),
                    new XElement(Uic + "LocationWellIdentifier", model.LocationDetail.LocationWellIdentifier),
                    new XElement(Uic + "LatitudeMeasure", model.LocationDetail.LatitudeMeasure),
                    new XElement(Uic + "LongitudeMeasure", model.LocationDetail.LongitudeMeasure)));
            }

            foreach (var violation in model.WellViolationDetail)
            {
                var violationDetail = new XElement(Uic + "WellViolationDetail",
                    new XElement(Uic + "ViolationIdentifier", violation.ViolationIdentifier),
                    new XElement(Uic + "ViolationContaminationCode", violation.ViolationContaminationCode),
                    new XElement(Uic + "ViolationEndangeringCode", violation.ViolationEndangeringCode),
                    new XElement(Uic + "ViolationReturnComplianceDate", violation.ViolationReturnComplianceDate),
                    new XElement(Uic + "ViolationSignificantCode", violation.ViolationSignificantCode),
                    new XElement(Uic + "ViolationDeterminedDate", violation.ViolationDeterminedDate),
                    new XElement(Uic + "ViolationTypeCode", violation.ViolationTypeCode),
                    new XElement(Uic + "ViolationWellIdentifier", violation.ViolationWellIdentifier));

                foreach (var response in violation.ResponseDetail)
                {
                    var responseDetail = new XElement(Uic + "WellResponseDetail",
                        new XElement(Uic + "ResponseEnforcementIdentifier", response.ResponseEnforcementIdentifier),
                        new XElement(Uic + "ResponseViolationIdentifier", response.ResponseViolationIdentifier));

                    violationDetail.Add(responseDetail);
                }

                wellDetail.Add(violationDetail);
            }

            foreach (var inspection in model.WellInspectionDetail)
            {
                var inspectionDetail = new XElement(Uic + "WellInspectionDetail",
                    new XElement(Uic + "InspectionIdentifier", inspection.InspectionIdentifier),
                    new XElement(Uic + "InspectionAssistanceCode", inspection.InspectionAssistanceCode),
                    new XElement(Uic + "InspectionDeficiencyCode", inspection.InspectionDeficiencyCode),
                    new XElement(Uic + "InspectionActionDate", inspection.InspectionActionDate),
                    new XElement(Uic + "InspectionIdisComplianceMonitoringReasonCode", inspection.InspectionIdisComplianceMonitoringReasonCode),
                    new XElement(Uic + "InspectionIcisComplianceMonitoringTypeCode", inspection.InspectionIcisComplianceMonitoringTypeCode),
                    new XElement(Uic + "InspectionIcisComplianceActivityTypeCode", inspection.InspectionIcisComplianceActivityTypeCode),
                    new XElement(Uic + "InspectionIcisMoaName", inspection.InspectionIcisMoaName),
                    new XElement(Uic + "InspectionIcisRegionalPriorityName", inspection.InspectionIcisRegionalPriorityName),
                    new XElement(Uic + "InspectionTypeActionCode", inspection.InspectionTypeActionCode),
                    new XElement(Uic + "InspectionWellIdentifier", inspection.InspectionWellIdentifier));

                foreach (var correction in inspection.CorrectionDetail)
                {
                    var correctionDetail = new XElement(Uic + "CorrectionDetail",
                        new XElement(Uic + "CorrectionIdentifier", correction.CorrectionIdentifier),
                        new XElement(Uic + "CorrectiveActionTypeCode", correction.CorrectiveActionTypeCode),
                        new XElement(Uic + "CorrectionCommentText", correction.CorrectionCommentText),
                        new XElement(Uic + "CorrectionInspectionIdentifier", correction.CorrectionInspectionIdentifier));

                    inspectionDetail.Add(correctionDetail);
                }

                wellDetail.Add(inspectionDetail);
            }

            foreach (var mitest in model.MitTestDetail)
            {
                var mitestDetail = new XElement(Uic + "MITestDetail",
                    new XElement(Uic + "MechanicalIntegrityTestIdentifier", mitest.MechanicalIntegrityTestIdentifier),
                    new XElement(Uic + "MechanicalIntegrityTestCompletedDate", mitest.MechanicalIntegrityTestCompletedDate),
                    new XElement(Uic + "MechanicalIntegrityTestResultCode", mitest.MechanicalIntegrityTestResultCode),
                    new XElement(Uic + "MechanicalIntegrityTestTypeCode", mitest.MechanicalIntegrityTestTypeCode),
                    new XElement(Uic + "MechanicalIntegrityTestRemedialActionDate", mitest.MechanicalIntegrityTestRemedialActionDate),
                    new XElement(Uic + "MechanicalIntegrityTestRemedialActionTypeCode", mitest.MechanicalIntegrityTestRemedialActionTypeCode),
                    new XElement(Uic + "MechanicalIntegrityTestWellIdentifier", mitest.MechanicalIntegrityTestWellIdentifier));

                wellDetail.Add(mitestDetail);
            }

            foreach (var engineering in model.EngineeringDetail)
            {
                var engineeringDetail = new XElement(Uic + "EngineeringDetail",
                    new XElement(Uic + "EngineeringIdentifier", engineering.EngineeringIdentifier),
                    new XElement(Uic + "EngineeringMaximumFlowRateNumeric", engineering.EngineeringMaximumFlowRateNumeric),
                    new XElement(Uic + "EngineeringPermittedOnsiteInjectionVolumeNumeric", engineering.EngineeringPermittedOnsiteInjectionVolumeNumeric),
                    new XElement(Uic + "EngineeringPermittedOffsiteInjectionVolumeNumeric", engineering.EngineeringPermittedOffsiteInjectionVolumeNumeric),
                    new XElement(Uic + "EngineeringWellIdentifier", engineering.EngineeringWellIdentifier));

                wellDetail.Add(engineeringDetail);
            }

            foreach (var waste in model.WasteDetail)
            {
                var wasteDetail = new XElement(Uic + "WasteDetail",
                    new XElement(Uic + "WasteIdentifier", waste.WasteIdentifier),
                    new XElement(Uic + "WasteCode", waste.WasteCode),
                    new XElement(Uic + "WasteStreamClassificationCode", waste.WasteStreamClassificationCode),
                    new XElement(Uic + "WasteWellIdentifier", waste.WasteWellIdentifier));

                foreach (var constituent in waste.ConstituentDetail)
                {
                    var constituentDetail = new XElement(Uic + "ConstituentDetail",
                        new XElement(Uic + "ConstituentIdentifier", constituent.ConstituentIdentifier),
                        new XElement(Uic + "MeasureValue", constituent.MeasureValue),
                        new XElement(Uic + "MeasureUnitCode", constituent.MeasureUnitCode),
                        new XElement(Uic + "ConstituentWasteIdentifier", constituent.ConstituentWasteIdentifier));

                    wasteDetail.Add(constituentDetail);
                }

                wellDetail.Add(wasteDetail);
            }

            facilityList.Add(wellDetail);

            return facilityList;
        }
    }
}