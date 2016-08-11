using System;

namespace domain.uic_etl.sde
{
    public class ViolationSdeModel
    {
        public static string[] Fields =
        {
            "GUID", "USDWContamination", "ENDANGER", "ReturnToComplianceDate",
            "SignificantNonCompliance", "ViolationType", "ViolationDate", "Well_FK", "Facility_FK"
        };

        public Guid Guid { get; set; }
        public string UsdwContamination { get; set; }
        public string Endanger { get; set; }
        public DateTime? ReturnToComplianceDate { get; set; }
        public string SignificantNonCompliance { get; set; }
        public DateTime? ViolationDate { get; set; }
        public string ViolationType { get; set; }
        public Guid FacilityId { get; set; }
        public Guid WellId { get; set; }
    }
}