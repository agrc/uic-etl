using System;

namespace domain.uic_etl.sde
{
    public class WellSdeModel
    {
        public static string[] Fields =
        {
            "GUID", "InjectionAquiferExempt", "HighPriority", "WellID",
            "WellSWPZ", "WellName", "WellSubClass", "Facility_FK", "Authorization_FK",
            "LocationMethod", "LocationAccuracy"
        };

        public Guid Guid { get; set; }
        public string WellId { get; set; }
        public Guid FacilityGuid { get; set; }
        public Guid AuthorizationGuid { get; set; }
        public string InjectionAquiferExempt { get; set; }
        public string HighPriority { get; set; }
        public string WellSwpz { get; set; }
        public string WellName { get; set; }
        public string WellSubClass { get; set; }
        public string LocationAccuracy { get; set; }
        public string LocationMethod { get; set; }
    }
}