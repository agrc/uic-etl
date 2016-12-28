using System;

namespace domain.uic_etl.sde
{
    public class EnforcementSdeModel
    {
        public static string[] Fields =
        {
            "Guid", "EnforcementType", "EnforcementDate"
        };

        public Guid Guid { get; set; }
        public string EnforcementType { get; set; }
        public DateTime? EnforcementDate { get; set; }
    }
}