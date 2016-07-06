using System;

namespace domain.uic_etl.xml
{
    public class WellInspectionDetail
    {
        public int InspectionIdentifier { get; set; }
        public string InspectionAssistanceCode { get; set; }
        public string InspectionDeficiencyCode { get; set; }
        public string InspectionActionDate { get; set; }
        public string InspectionIdisComplianceMonitoringReasonCode { get; set; }
        public string InspectionIcisComplianceMonitoringTypeCode { get; set; }
        public string InspectionIcisComplianceActivityTypeCode { get; set; }
        public string InspectionIcisMoaName { get; set; }
        public string InspectionIcisRegionalPriorityName { get; set; }
        public string InspectionTypeActionCode { get; set; }
        public Guid InspectionWellIdentifier { get; set; }
    }
}