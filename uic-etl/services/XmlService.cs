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
        private static readonly XNamespace Exchange = "http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd";

        public static XDocument CreateDocument()
        {
            var schemaLocation = XNamespace.Get("xmlns http://www.exchangenetwork.net/schema/v1.0/ExchangeNetworkDocument.xsd");

            var doc = new XDocument
            {
                Declaration = new XDeclaration("1.0", "UTF-8", null)
            };

            var root = new XElement(Exchange + "Document",
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

            doc.Root.Add(new XElement(Exchange + "Header",
                new XElement(Exchange + "Author", "Candace Cady"),
                new XElement(Exchange + "Organization", "UDEQ"),
                new XElement(Exchange + "Title", "Data Submittal for UT 1422 UIC Program"),
                new XElement(Exchange + "CreationTime", model.CreationTime),
                new XElement(Exchange + "Comment", model.Comments),
                new XElement(Exchange + "DataService", "UIC"),
                new XElement(Exchange + "ContactInfo", "ccady@utah.gov"),
                new XElement(Exchange + "Notification", "ccady@utah.gov"),
                new XElement(Exchange + "Sensitivity", "Unclasified")));
        }

        public static XElement CreatePayloadElements()
        {
            XNamespace xmlns = "http://www.exchangenetwork.net/schema/uic/2";

            var payload = new XElement(Exchange + "Payload",
                new XAttribute("Operation", "Delete - Insert"),
                new XElement(xmlns + "UIC", new XAttribute(XNamespace.Xmlns + "xsi", Xsi),
                    new XElement(xmlns + "PrimacyAgencyCode", "UTEQ")));

            return payload;
        }

        public static XElement AppendPayloadElements(ref XElement payload, IEnumerable<ContactDetail> contacts, IEnumerable<PermitDetail> permits)
        {
            var node = payload.Descendants(Uic + "UIC").SingleOrDefault();

            foreach (var contact in contacts)
            {
                var contactDetail = new XElement(Uic + "ContactDetail",
                    new XElement(Uic + "ContactIdentifier", contact.ContactIdentifier),
                    new XElement(Uic + "TelephoneNumberText", contact.TelephoneNumberText),
                    new XElement(Uic + "IndividualFullName", contact.IndividualFullName),
                    new XElement(Uic + "ContactCityName", contact.ContactCityName),
                    new XElement(Uic + "ContactAddressStateCode", contact.ContactAddressStateCode),
                    new XElement(Uic + "ContactAddressText", contact.ContactAddressText),
                    new XElement(Uic + "ContactAddressPostalCode", contact.ContactAddressPostalCode));

                node.Add(contactDetail);
            }

            foreach (var permit in permits)
            {
                var permitDetail = new XElement(Uic + "PermitDetail",
                    new XElement(Uic + "PermitIdentifier", permit.PermitIdentifier),
                    new XElement(Uic + "PermitAORWellNumberNumeric", permit.PermitAorWellNumberNumeric),
                    new XElement(Uic + "PermitAuthorizedStatusCode", permit.PermitAuthorizedStatusCode),
                    new XElement(Uic + "PermitOwnershipTypeCode", permit.PermitOwnershipTypeCode),
                    new XElement(Uic + "PermitAuthorizedIdentifier", permit.PermitAuthorizedIdentifier));

                foreach (var activity in permit.PermitActivityDetail)
                {
                    var activityDetail = new XElement(Uic + "PermitActivityDetail",
                        new XElement(Uic + "PermitActivityIdentifier", activity.PermitActivityIdentifier),
                        new XElement(Uic + "PermitActivityActionTypeCode", activity.PermitActivityActionTypeCode),
                        new XElement(Uic + "PermitActivityDate", activity.PermitActivityDate),
                        new XElement(Uic + "PermitActivityPermitIdentifier", activity.PermitActivityPermitIdentifier));

                    permitDetail.Add(activityDetail);
                }

                node.Add(permitDetail);
            }

            return node;
        }

        public static XElement AddFacility(ref XElement payload, FacilityDetail model)
        {
            var facilityDetail = new XElement(Uic + "FacilityList");
            var facility = new XElement(Uic + "FacilityDetail",
                new XElement(Uic + "FacilityIdentifier", model.FacilityIdentifier),
                new XElement(Uic + "LocalityName", model.LocalityName),
                new XElement(Uic + "FacilitySiteName", model.FacilitySiteName),
                new XElement(Uic + "FacilityPetitionStatusCode", model.FacilityPetitionStatusCode),
                new XElement(Uic + "LocationAddressStateCode", model.LocationAddressStateCode),
                new XElement(Uic + "FacilityStateIdentifier", model.FacilityStateIdentifier),
                new XElement(Uic + "LocationAddressText", model.LocationAddressText));

            if (model.FacilitySiteTypeCode != "U")
            {
                facility.Add(new XElement(Uic + "FacilitySiteTypeCode", model.FacilitySiteTypeCode));
            }
            facility.Add(new XElement(Uic + "NAICSCode", model.NaicsCode),
                new XElement(Uic + "LocationAddressPostalCode", model.LocationAddressPostalCode));

            facilityDetail.Add(facility);

            foreach (var inspection in model.FacilityInspectionDetail)
            {
                var inspectionDetail = new XElement(Uic + "FacilityInspectionDetail",
                    new XElement(Uic + "InspectionIdentifier", inspection.InspectionIdentifier));

                inspectionDetail.Add(new XElement(Uic + "InspectionActionDate", inspection.InspectionActionDate));

                if (!string.IsNullOrEmpty(inspection.InspectionIcisComplianceMonitoringReasonCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISComplianceMonitoringReasonCode",
                        inspection.InspectionIcisComplianceMonitoringReasonCode));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisComplianceMonitoringTypeCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISComplianceMonitoringTypeCode", inspection.InspectionIcisComplianceMonitoringTypeCode));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisComplianceActivityTypeCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISComplianceActivityTypeCode", inspection.InspectionIcisComplianceActivityTypeCode));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisMoaName))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISMOAName", inspection.InspectionIcisMoaName));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisRegionalPriorityName))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISRegionalPriorityName", inspection.InspectionIcisRegionalPriorityName));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionTypeActionCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionTypeActionCode", inspection.InspectionTypeActionCode));
                }

                inspectionDetail.Add(new XElement(Uic + "InspectionFacilityIdentifier", model.FacilityIdentifier));

                facility.Add(inspectionDetail);
            }

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
                new XElement(Uic + "WellAquiferExemptionInjectionCode", model.WellAquiferExemptionInjectionCode));

            if (model.WellTotalDepthNumeric != "empty")
            {
                wellDetail.Add(new XElement(Uic + "WellTotalDepthNumeric", model.WellTotalDepthNumeric));
            }

            wellDetail.Add(
                new XElement(Uic + "WellHighPriorityDesignationCode", model.WellHighPriorityDesignationCode),
                new XElement(Uic + "WellContactIdentifier", model.WellContactIdentifier),
                new XElement(Uic + "WellFacilityIdentifier", model.WellFacilityIdentifier),
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

            foreach (var wellType in model.WellTypeDetail)
            {
                var wellTypeDetail = new XElement(Uic + "WellTypeDetail",
                    new XElement(Uic + "WellTypeIdentifier", wellType.WellTypeIdentifier),
                    new XElement(Uic + "WellTypeCode", wellType.WellTypeCode),
                    new XElement(Uic + "WellTypeDate", wellType.WellTypeDate),
                    new XElement(Uic + "WellTypeWellIdentifier", wellType.WellTypeWellIdentifier));

                wellDetail.Add(wellTypeDetail);
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
                    new XElement(Uic + "LatitudeMeasure", model.LocationDetail.LatitudeMeasure.ToString("###.######")),
                    new XElement(Uic + "LongitudeMeasure", model.LocationDetail.LongitudeMeasure.ToString("###.######"))));
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
                    new XElement(Uic + "InspectionIdentifier", inspection.InspectionIdentifier));
                
                if (!string.IsNullOrEmpty(inspection.InspectionAssistanceCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionAssistanceCode", inspection.InspectionAssistanceCode));
                }
                
                if (!string.IsNullOrEmpty(inspection.InspectionDeficiencyCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionDeficiencyCode", inspection.InspectionDeficiencyCode));
                }  
                    
                inspectionDetail.Add(new XElement(Uic + "InspectionActionDate", inspection.InspectionActionDate));

                if (!string.IsNullOrEmpty(inspection.InspectionIcisComplianceMonitoringReasonCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISComplianceMonitoringReasonCode",
                        inspection.InspectionIcisComplianceMonitoringReasonCode));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisComplianceMonitoringTypeCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISComplianceMonitoringTypeCode", inspection.InspectionIcisComplianceMonitoringTypeCode));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisComplianceActivityTypeCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISComplianceActivityTypeCode", inspection.InspectionIcisComplianceActivityTypeCode));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisMoaName))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISMOAName", inspection.InspectionIcisMoaName));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionIcisRegionalPriorityName))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionICISRegionalPriorityName", inspection.InspectionIcisRegionalPriorityName));
                }

                if (!string.IsNullOrEmpty(inspection.InspectionTypeActionCode))
                {
                    inspectionDetail.Add(new XElement(Uic + "InspectionTypeActionCode", inspection.InspectionTypeActionCode));
                }
                
                inspectionDetail.Add(new XElement(Uic + "InspectionWellIdentifier", inspection.InspectionWellIdentifier));

                foreach (var correction in inspection.CorrectionDetail)
                {
                    var correctionDetail = new XElement(Uic + "CorrectionDetail",
                        new XElement(Uic + "CorrectionIdentifier", correction.CorrectionIdentifier),
                        new XElement(Uic + "CorrectionActionTypeCode", correction.CorrectiveActionTypeCode));

                    if (!string.IsNullOrEmpty(correction.CorrectionCommentText))
                    {
                        correctionDetail.Add(new XElement(Uic + "CorrectionCommentText", correction.CorrectionCommentText));
                    }
                    
                    correctionDetail.Add(new XElement(Uic + "CorrectionInspectionIdentifier", correction.CorrectionInspectionIdentifier));

                    inspectionDetail.Add(correctionDetail);
                }

                wellDetail.Add(inspectionDetail);
            }

            foreach (var mitest in model.MitTestDetail)
            {
                var mitestDetail = new XElement(Uic + "MITestDetail",
                    new XElement(Uic + "MechanicalIntegrityTestIdentifier", mitest.MechanicalIntegrityTestIdentifier));

                if (!string.IsNullOrEmpty(mitest.MechanicalIntegrityTestCompletedDate))
                {
                    mitestDetail.Add(new XElement(Uic + "MechanicalIntegrityTestCompletedDate", mitest.MechanicalIntegrityTestCompletedDate));
                }

                mitestDetail.Add(new XElement(Uic + "MechanicalIntegrityTestResultCode", mitest.MechanicalIntegrityTestResultCode));
                mitestDetail.Add(new XElement(Uic + "MechanicalIntegrityTestTypeCode", mitest.MechanicalIntegrityTestTypeCode));

                if (!string.IsNullOrEmpty(mitest.MechanicalIntegrityTestRemedialActionDate))
                {
                    mitestDetail.Add(new XElement(Uic + "MechanicalIntegrityTestRemedialActionDate", mitest.MechanicalIntegrityTestRemedialActionDate));
                }

                if (!string.IsNullOrEmpty(mitest.MechanicalIntegrityTestRemedialActionTypeCode))
                {
                    mitestDetail.Add(new XElement(Uic + "MechanicalIntegrityTestRemedialActionTypeCode", mitest.MechanicalIntegrityTestRemedialActionTypeCode));
                }
                
                mitestDetail.Add(new XElement(Uic + "MechanicalIntegrityTestWellIdentifier", mitest.MechanicalIntegrityTestWellIdentifier));

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