using System;
using System.Linq;

namespace domain.uic_etl.sde
{
    public class WellStatusSdeModel
    {
        public static string[] Fields = {"GUID", "OperatingStatusDate", "OperatingStatusType", "Well_FK"};
        private string _operatingStatusType;

        public Guid Guid { get; set; }
        public DateTime? OperatingStatusDate { get; set; }

        public string OperatingStatusType
        {
            get
            {
                if (new[] {"ot", "pr"}.Contains(_operatingStatusType.ToLower()))
                {
                    return "UC";
                }

                return _operatingStatusType;
            }
            set { _operatingStatusType = value; }
        }

        public Guid WellGuid { get; set; }
    }
}