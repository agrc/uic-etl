using System;

namespace domain.uic_etl.xml
{
    public class WellStatusDetail
    {
        public int WellStatusIdentifier { get; set; }
        public string WellStatusDate { get; set; }
        public string WellStatusOperatingStatusCode { get; set; }
        public Guid WellStatusWellIdentifier { get; set; }
    }
}