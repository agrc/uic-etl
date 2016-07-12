namespace domain.uic_etl.sde
{
    public class ConstituentClassISdeModel
    {
        public static string[] Fields = { "GUID", "Concentration", "Unit", "ConstituentCode" };

        public int Concentration { get; set; }
        public int Unit { get; set; }
        public string ConstituentCode { get; set; }
    }
}