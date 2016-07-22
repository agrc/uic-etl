using System;
using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class ViolationDetail
    {
        public ViolationDetail()
        {
            ResponseDetail = new List<ResponseDetail>();
        }

        public Guid Guid { get; set; }
        public string ViolationIdentifier { get; set; }
        public string ViolationContaminationCode { get; set; }
        public string ViolationEndangeringCode { get; set; }
        public string ViolationReturnComplianceDate { get; set; }
        public string ViolationSignificantCode { get; set; }
        public string ViolationDeterminedDate { get; set; }
        public string ViolationTypeCode { get; set; }
        public string ViolationFacilityIdentifier { get; set; }
        public List<ResponseDetail> ResponseDetail { get; set; }
        public string FacilityId { get; set; }
        public string ViolationWellIdentifier { get; set; }
    }
}