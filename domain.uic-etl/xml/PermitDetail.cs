using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class PermitDetail
    {
        public PermitDetail()
        {
            PermitActivityDetail = new List<PermitActivityDetail>();
        }

        public string PermitIdentifier { get; set; }
        public string PermitAuthorizedStatusCode { get; set; }
        public string PermitOwnershipTypeCode { get; set; }
        public string PermitAuthorizedIdentifier { get; set; }
        public double PermitAorWellNumberNumeric { get; set; }
        public List<PermitActivityDetail> PermitActivityDetail { get; set; }
    }
}