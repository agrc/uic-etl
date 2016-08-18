namespace domain.uic_etl.xml
{
    public class PermitActivityDetail
    {
        private string _permitActivityActionTypeCode;
        public string PermitActivityIdentifier { get; set; }

        public string PermitActivityActionTypeCode
        {
            get {
                if (string.IsNullOrEmpty(_permitActivityActionTypeCode))
                {
                    return "";
                }

                switch (_permitActivityActionTypeCode.ToUpper())
                {
                    case "AR":
                    {
                        return "AM";
                    }
                    case "NR":
                    {
                        return "";
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