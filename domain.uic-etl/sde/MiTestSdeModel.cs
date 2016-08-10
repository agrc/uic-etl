using System;

namespace domain.uic_etl.sde
{
    public class MiTestSdeModel
    {
        public static string[] Fields =
        {
            "GUID", "MITDate", "MITResult", "MITType", "MITRemActDate",
            "MITRemediationAction", "Well_FK"
        };

        public Guid Guid { get; set; }
        public DateTime MitDate { get; set; }
        public string MitResult { get; set; }
        public string MitType { get; set; }
        public DateTime MitRemActDate { get; set; }
        public string MitRemediationAction { get; set; }
        public Guid WellFk { get; set; }
    }
}