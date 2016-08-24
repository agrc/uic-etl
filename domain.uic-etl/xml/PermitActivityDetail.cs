using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class PermitActivityDetail
    {
        private string _permitActivityActionTypeCode;
        
        public string PermitActivityIdentifier { get; set; }
        public string PermitActivityActionTypeCode
        {
            get
            {
                if (string.IsNullOrEmpty(_permitActivityActionTypeCode))
                {
                    return null;
                }

                switch (_permitActivityActionTypeCode.ToUpper())
                {
                    case "AR":
                    {
                        return "AM";
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

    public class PermitActivityDetailValidator : AbstractValidator<PermitActivityDetail>
    {
        public PermitActivityDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.PermitActivityIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.PermitActivityDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.PermitActivityPermitIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.PermitActivityActionTypeCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"AI", "AM", "PI", "PD", "PN", "PM", "FR"}.Contains(code.ToUpper()));
            });
        }
    }
}