using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class FacilityViolationDetail
    {
        public string ViolationIdentifier { get; set; }
        public string ViolationContaminationCode { get; set; }
        public string ViolationEndangeringCode { get; set; }
        public string ViolationReturnComplianceDate { get; set; }
        public string ViolationSignificantCode { get; set; }
        public string ViolationDeterminedDate { get; set; }
        public string ViolationTypeCode { get; set; }
        public string ViolationFacilityIdentifier { get; set; }
        public List<FacilityResponseDetail> FacilityResponseDetails { get; set; } 
    }
}