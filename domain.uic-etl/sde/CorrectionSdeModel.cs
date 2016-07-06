using System;

namespace domain.uic_etl.sde
{
    public class CorrectionSdeModel
    {
        public static string[] Fields = { "GUID", "CorrectiveAction", "Comments" };

        public string CorrectiveAction { get; set; }
        public string Comments { get; set; }
        public Guid Guid { get; set; }
    }
}