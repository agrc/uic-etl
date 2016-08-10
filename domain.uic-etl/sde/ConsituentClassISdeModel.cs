using System;

namespace domain.uic_etl.sde
{
    public class ConstituentClassISdeModel
    {
        public static string[] Fields = { "GUID", "Concentration", "Unit", "Constituent", "ClassIWaste_FK" };

        public double Concentration { get; set; }
        public string Unit { get; set; }
        public string Constituent { get; set; }
        public Guid WasteGuid { get; set; }
        public Guid Guid { get; set; }
    }
}