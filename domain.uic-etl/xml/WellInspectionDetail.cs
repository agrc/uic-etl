using System;
using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class WellInspectionDetail
    {
        public WellInspectionDetail()
        {
            CorrectionDetail = new List<CorrectionDetail>();
        }
        public string InspectionIdentifier { get; set; }
        public string InspectionAssistanceCode { get; set; }
        public string InspectionDeficiencyCode { get; set; }
        public string InspectionActionDate { get; set; }
        public string InspectionIcisComplianceMonitoringReasonCode { get; set; }
        public string InspectionIcisComplianceMonitoringTypeCode { get; set; }
        public string InspectionIcisComplianceActivityTypeCode { get; set; }
        public string InspectionIcisMoaName { get; set; }
        public string InspectionIcisRegionalPriorityName { get; set; }
        public string InspectionTypeActionCode { get; set; }
        public string InspectionWellIdentifier { get; set; }
        public List<CorrectionDetail> CorrectionDetail { get; set; }
    }
}