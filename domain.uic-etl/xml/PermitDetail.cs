using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class PermitDetail
    {
        public PermitDetail()
        {
            PermitActivityDetail = new List<PermitActivityDetail>();
        }

        public string PermitIdentifier { get; set; }
        public string PermitAuthorizedStatusCode { get; set; }
        public string PermitOwnershipTypeCode { get; set; }
        public string PermitAuthorizedIdentifier { get; set; }
        public double PermitAorWellNumberNumeric { get; set; }
        public List<PermitActivityDetail> PermitActivityDetail { get; set; }
    }

    public class PermitDetailValidator : AbstractValidator<PermitDetail>
    {
        public PermitDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.PermitIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.PermitAuthorizedStatusCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"IP", "AP", "GP", "EP", "OP", "RA", "NO"}.Contains(code.ToUpper()));

                RuleFor(src => src.PermitAuthorizedIdentifier)
                    .NotEmpty()
                    .Length(1, 50);
            });

            RuleSet("R2", () =>
            {
                RuleFor(src => src.PermitOwnershipTypeCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"PB", "PN", "PF", "PV", "FG", "OI", "OS", "OT", "OR"}.Contains(code.ToUpper()));
            });

            // todo: class I, II, and VI wells only
            RuleSet("R2C", () =>
            {
                RuleFor(src => src.PermitAorWellNumberNumeric)
                    .LessThan(1000000)
                    .GreaterThanOrEqualTo(0);
            });
        }
    }
}