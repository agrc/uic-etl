using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class CorrectionDetail
    {
        public string CorrectionIdentifier { get; set; }
        public string CorrectiveActionTypeCode { get; set; }
        public string CorrectionCommentText { get; set; }
        public string CorrectionInspectionIdentifier { get; set; }
    }

    public class CorrectionDetailValidator : AbstractValidator<CorrectionDetail>
    {
        public CorrectionDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.CorrectionIdentifier)
                    .NotEmpty()
                    .Length(20);
                
                RuleFor(src => src.CorrectionInspectionIdentifier)
                    .NotEmpty()
                    .Length(20);
            });

            RuleSet("RC2", () =>
            {
                RuleFor(src => src.CorrectiveActionTypeCode)
                    .Length(2)
                    .Must(code => new[] {"VE", "RK", "MD", "NO", "PA", "NM", "IP", "RP", "SI", "OT"}.Contains(code.ToUpper()))
                    .Unless(src => string.IsNullOrEmpty(src.CorrectiveActionTypeCode));

                RuleFor(src => src.CorrectiveActionTypeCode)
                    .Length(1, 200)
                    .Unless(src => string.IsNullOrEmpty(src.CorrectiveActionTypeCode) || src.CorrectiveActionTypeCode.ToUpper() == "OT");
            });
        }
    }
}