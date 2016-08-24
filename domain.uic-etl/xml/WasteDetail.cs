using System.Collections.Generic;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class WasteDetail
    {
        public WasteDetail()
        {
            ConstituentDetail = new List<ConstituentDetail>();
        }

        public string WasteIdentifier { get; set; }
        public string WasteCode { get; set; }
        public string WasteStreamClassificationCode { get; set; }
        public string WasteWellIdentifier { get; set; }
        public List<ConstituentDetail> ConstituentDetail { get; set; }
    }

    public class WasteDetailValidator : AbstractValidator<WasteDetail>
    {
        public WasteDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.WasteIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WasteWellIdentifier)
                    .NotEmpty()
                    .Length(20);
            });

            // todo: required for class I hazardous wells (1H)
            RuleSet("R2C", () =>
            {
                RuleFor(src => src.WasteCode)
                    .NotEmpty()
                    .Length(1, 4);

                RuleFor(src => src.WasteStreamClassificationCode)
                    .NotEmpty()
                    .Length(1, 100);
            });
        }
    }
}