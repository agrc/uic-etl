using System;
using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class WasteDetail
    {
        public WasteDetail()
        {
            ConstituentDetail = new List<ConstituentDetail>();
        }

        public string WasteIdentifier { get; set; }
        public string WasteCode { get; set; }
        public string WasteStreamClassificationCode { get; set; }
        public Guid WasteWellIdentifier { get; set; }
        public List<ConstituentDetail> ConstituentDetail { get; set; }
    }
}