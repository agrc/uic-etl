using System;

namespace domain.uic_etl.sde
{
    public class WasteClassISdeModel
    {
        public static string[] Fields = { "GUID", "WasteCode", "WasteStream", "Well_FK" };

        public string WasteCode { get; set; }
        public string WasteStream { get; set; }
        public Guid WellFk { get; set; }
        public Guid Guid { get; set; }
    }
}