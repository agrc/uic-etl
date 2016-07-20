namespace domain.uic_etl.sde
{
    public class AreaOfReviewSdeModel
    {
        public static string[] Fields = { "CA_Abandon", "CA_Repair", "CA_Replug", "CA_Other" };

        public double CaAbandon { get; set; }
        public double CaRepair { get; set; }
        public double CaReplug { get; set; }
        public double CaOther { get; set; }

        public double PermitAorWellNumberNumeric
        {
            get { return CaAbandon + CaRepair + CaReplug + CaOther; }
        }
    }
}