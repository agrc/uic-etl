using System;

namespace domain.uic_etl.xml
{
    public class WellStatusDetail
    {
        public string WellStatusIdentifier { get; set; }
        public string WellStatusDate { get; set; }
        public string WellStatusOperatingStatusCode { get; set; }
        public string WellStatusWellIdentifier { get; set; }
    }
}