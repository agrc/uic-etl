using System;

namespace domain.uic_etl.xml
{
    public class WasteDetail
    {
        public string WasteCode { get; set; }
        public string WasteStreamClassificationCode { get; set; }
        public Guid WasteWellIdentifier { get; set; }
    }
}