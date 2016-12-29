using FluentValidation;

namespace domain.uic_etl.xml
{
    public class ResponseDetail
    {
        private string _enforcementActionType;
        public string ResponseEnforcementIdentifier { get; set; }
        public string ResponseViolationIdentifier { get; set; }
        public string EnforcementActionDate { get; set; }

        public string EnforcementActionType
        {
            get
            {
                if (string.IsNullOrEmpty(_enforcementActionType))
                {
                    return _enforcementActionType;
                }

                if (_enforcementActionType.ToUpper() == "INF")
                {
                    _enforcementActionType = "OTR";
                }

                return _enforcementActionType;
            }
            set { _enforcementActionType = value; }
        }
    }

    public class ResponseDetailValidator : AbstractValidator<ResponseDetail>
    {
        public ResponseDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.ResponseViolationIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.ResponseEnforcementIdentifier)
                    .NotEmpty()
                    .Length(20);
            });
        }
    }
}