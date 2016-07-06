using System;

namespace domain.uic_etl.sde
{
    public class WellInspectionSdeModel
    {
        public static string[] Fields =
        {
            "GUID", "InspectionAssistance", "InspectionDeficiency", "InspectionDate",
            "ICISCompMonActReason", "ICISCompMonType", "ICISCompActType", "ICISMOAPriority",
            "ICISRegionalPriority", "InspectionType", "Well_FK"
        };

        public string InspectionAssistance { get; set; }
        public string InspectionDeficiency { get; set; }
        public DateTime InspectionDate { get; set; }
        public string IcisCompMonActReason { get; set; }
        public string IcisCompMonType { get; set; }
        public string IcisCompActType { get; set; }
        public string IcisMoaPriority { get; set; }
        public string IcisRegionalPriority { get; set; }
        public string InspectionType { get; set; }
        public Guid WellFk { get; set; }
    }
}