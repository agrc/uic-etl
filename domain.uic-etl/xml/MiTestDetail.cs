using System.Linq;
using FluentValidation;

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

    public class MiTestDetailValidator : AbstractValidator<MiTestDetail>
    {
        public MiTestDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.MechanicalIntegrityTestIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.MechanicalIntegrityTestCompletedDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.MechanicalIntegrityTestWellIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.MechanicalIntegrityTestResultCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"PS", "FU", "FP", "FA"}.Contains(code.ToUpper()));

                RuleFor(src => src.MechanicalIntegrityTestResultCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"AP", "CT", "MR", "WI", "WA", "AT", "SR", "OL", "CR", "TN", "RC", "CB", "OA", "RS", "DC", "OF"}
                        .Contains(code.ToUpper()));
            });

            RuleSet("R1C", () => {
                RuleFor(src => src.MechanicalIntegrityTestRemedialActionDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}")
                    .Unless(src => string.IsNullOrEmpty(src.MechanicalIntegrityTestRemedialActionTypeCode));
            });

            RuleSet("R2C", () =>
            {
                RuleFor(src => src.MechanicalIntegrityTestRemedialActionTypeCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"CS", "TP", "PA", "OT"}.Contains(code.ToUpper()))
                    .When(src => src.MechanicalIntegrityTestResultCode.ToUpper() == "F");
            });
        }
    }
}