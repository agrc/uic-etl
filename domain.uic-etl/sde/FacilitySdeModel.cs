using System;

namespace domain.uic_etl.sde
{
    public class FacilitySdeModel
    {
        public static string[] Fields =
        {
            "GUID", "FacilityID", "FacilityName", "FacilityAddress", "FacilityCity",
            "FacilityState", "FacilityZip", "FacilityType", "NoMigrationPetStatus"
        };

        public Guid Guid { get; set; }
        public string FacilityName { get; set; }
        public string FacilityAddress { get; set; }
        public string FacilityCity { get; set; }
        public string FacilityId { get; set; }
        public string FacilityZip { get; set; }
        public string FacilityType { get; set; }
        public string NoMigrationPetStatus { get; set; }
    }
}