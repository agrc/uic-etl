using FluentValidation;

namespace domain.uic_etl.xml
{
    public class ResponseDetail
    {
        public string ResponseEnforcementIdentifier { get; set; }
        public string ResponseViolationIdentifier { get; set; }
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