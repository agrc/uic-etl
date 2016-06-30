using System;

namespace domain.uic_etl.xml
{
    public class WellTypeDetail
    {
        public int WellTypeIdentifier { get; set; }
        public string WellTypeCode { get; set; }
        public DateTime WellTypeDate { get; set; }
        public Guid WellTypeWellIdentifier { get; set; }
    }
}