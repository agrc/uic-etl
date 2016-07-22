using System;

namespace domain.uic_etl.xml
{
    public class MiTestDetail
    {
        public string MechanicalIntegrityTestIdentifier { get; set; }
        public string MechanicalIntegrityTestCompletedDate { get; set; }
        public string MechanicalIntegrityTestResultCode { get; set; }
        public string MechanicalIntegrityTestTypeCode { get; set; }
        public string MechanicalIntegrityTestRemedialActionDate { get; set; }
        public string MechanicalIntegrityTestRemedialActionTypeCode { get; set; }
        public string MechanicalIntegrityTestWellIdentifier { get; set; }
    }
}