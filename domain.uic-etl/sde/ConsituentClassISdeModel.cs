using System;

namespace domain.uic_etl.sde
{
    public class ConstituentClassISdeModel
    {
        public static string[] Fields = { "GUID", "Concentration", "Unit", "ConstituentCode", "ClassIWaste_FK" };

        public int Concentration { get; set; }
        public int Unit { get; set; }
        public string ConstituentCode { get; set; }
        public Guid WasteGuid { get; set; }
    }
}