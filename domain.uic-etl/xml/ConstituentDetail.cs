using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class ConstituentDetail
    {
        public string ConstituentIdentifier { get; set; }
        public string MeasureValue { get; set; }
        public string MeasureUnitCode { get; set; }
        public string ConstituentNameText { get; set; }
        public string ConstituentWasteIdentifier { get; set; }
    }

    public class ConstituentDetailValidator : AbstractValidator<ConstituentDetail>
    {
        public ConstituentDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.ConstituentIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.ConstituentWasteIdentifier)
                    .NotEmpty()
                    .Length(20);
            });

            RuleSet("RC2", () =>
            {
                RuleFor(src => src.ConstituentNameText)
                    .Length(0, 50)
                    .Unless(src => string.IsNullOrEmpty(src.ConstituentNameText));

                RuleFor(src => src.MeasureValue)
                    .Length(1, 11)
                    .Must(value =>
                    {
                        decimal concentration;
                        if (!decimal.TryParse(value, out concentration))
                        {
                            return false;
                        }

                        return concentration > 0 && concentration < 1000000;
                    })
                    .Unless(src => string.IsNullOrEmpty(src.ConstituentNameText));

                RuleFor(src => src.MeasureUnitCode)
                    .Must(unitCode => new[] { "MG/L", "PCI/L" }.Contains(unitCode.ToUpper()))
                    .Unless(src => string.IsNullOrEmpty(src.MeasureValue));  
            });
        }
    }
}