using System;

namespace domain.uic_etl.xml
{
    public class ConstituentDetail
    {
        public string ConstituentIdentifier { get; set; }
        public string MeasureValue { get; set; }
        public string MeasureUnitCode { get; set; }
        public string ConstituentNameText { get; set; }
        public string ConstituentWasteIdentifier { get; set; }
    }
}