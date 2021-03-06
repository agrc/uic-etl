﻿using System;

namespace domain.uic_etl.sde
{
    public class FacilitySdeModel
    {
        public static string[] Fields =
        {
            "GUID", "FacilityID", "FacilityName", "FacilityAddress", "FacilityCity",
            "FacilityState", "FacilityZip", "FacilityType", "NoMigrationPetStatus", "CountyFIPS", "NAICSPrimary",
            "FacilityMilePost"
        };

        private string _facilityAddress;

        public Guid Guid { get; set; }
        public string FacilityName { get; set; }

        public string FacilityAddress
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_facilityAddress))
                {
                    return FacilityMilePost;
                }
                
                if (_facilityAddress.ToLower().Trim() == "see facility mile post")
                {
                    return FacilityMilePost;
                }

                return _facilityAddress;
            }
            set { _facilityAddress = value; }
        }

        public string FacilityCity { get; set; }
        public string FacilityId { get; set; }
        public string FacilityZip { get; set; }
        public string FacilityType { get; set; }
        public string NoMigrationPetStatus { get; set; }
        public string NaicsPrimary { get; set; }
        public int CountyFips { get; set; }
        public string FacilityMilePost { get; set; }
    }
}