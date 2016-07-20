using System;
using System.Collections.Generic;

namespace domain.uic_etl.xml
{
    public class FacilityDetail
    {
        public string Xmlns = "http://www.exchangenetwork.net/schema/uic/2";

        public FacilityDetail()
        {
            FacilityViolationDetail = new List<ViolationDetail>();
        }

        public int FacilityIdentifier { get; set; }
        public Guid Guid { get; set; }
        public string LocalityName { get; set; }
        public string FacilitySiteName { get; set; }
        public string FacilityPetitionStatusCode { get; set; }
        public string LocationAddressStateCode { get; set; }
        public string FacilityStateIdentifier { get; set; }
        public string LocationAddressText { get; set; }
        public string FacilitySiteTypeCode { get; set; }
        public string LocationAddressPostalCode { get; set; }
        public List<ViolationDetail> FacilityViolationDetail { get; set; }
    }
}