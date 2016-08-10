using System;

namespace domain.uic_etl.sde
{
    public class WellStatusSdeModel
    {
        public static string[] Fields = {"Guid", "OperatingStatusDate", "OperatingStatusType", "Well_FK"};

        public Guid Guid { get; set; }
        public DateTime OperatingStatusDate { get; set; }
        public string OperatingStatusType { get; set; }
        public Guid WellGuid { get; set; }
    }
}