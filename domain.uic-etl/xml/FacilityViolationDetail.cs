using System;
using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class FacilityViolationDetail
    {
        public FacilityViolationDetail()
        {
            FacilityResponseDetails = new List<FacilityResponseDetail>();
        }

        public Guid Guid { get; set; }
        public int ViolationIdentifier { get; set; }
        public string ViolationContaminationCode { get; set; }
        public string ViolationEndangeringCode { get; set; }
        public string ViolationReturnComplianceDate { get; set; }
        public string ViolationSignificantCode { get; set; }
        public string ViolationDeterminedDate { get; set; }
        public string ViolationTypeCode { get; set; }
        public Guid ViolationFacilityIdentifier { get; set; }
        public List<FacilityResponseDetail> FacilityResponseDetails { get; set; }
        public Guid FacilityId { get; set; }
        public Guid WellId { get; set; }
    }
}