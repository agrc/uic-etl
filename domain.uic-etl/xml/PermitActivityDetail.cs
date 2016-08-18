namespace domain.uic_etl.xml
{
    public class PermitActivityDetail
    {
        private string _permitActivityActionTypeCode;
        public string PermitActivityIdentifier { get; set; }
        private const string Unknown = "FR";

        public string PermitActivityActionTypeCode
        {
            get {
                if (string.IsNullOrEmpty(_permitActivityActionTypeCode))
                {
                    return Unknown;
                }

                switch (_permitActivityActionTypeCode.ToUpper())
                {
                    case "AR":
                    {
                        return "AM";
                    }
                    case "NR":
                    {
                        return Unknown;
                    }
                    case "AW":
                    {
                        return "PD";
                    }
                    case "PT":
                    case "TP":
                    {
                        return "PN";
                    }
                }

                return _permitActivityActionTypeCode; 
            }
            set { _permitActivityActionTypeCode = value; }
        }

        public string PermitActivityDate { get; set; }
        public string PermitActivityPermitIdentifier { get; set; }
    }
}