using System;

namespace domain.uic_etl.sde
{
    public class WellSdeModel
    {
        public static string[] Fields =
        {
            "GUID", "InjectionAquiferExempt", "EventType", "HighPriority", "WellID",
            "WellSWPZ", "WellName", "WellSubClass"
        };

        public Guid Guid { get; set; }
    }
}