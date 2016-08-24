using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class WellTypeDetail
    {
        public string WellTypeIdentifier { get; set; }
        public string WellTypeCode { get; set; }
        public string WellTypeDate { get; set; }
        public string WellTypeWellIdentifier { get; set; }
    }

    public class WellTypeDetailValidator : AbstractValidator<WellTypeDetail>
    {
        private readonly IEnumerable<string> _wellTypes;

        public WellTypeDetailValidator()
        {
            var codeOne = new[] {"1H", "1I", "1M", "1R", "1W", "1X"};
            var codeTwo = new[] {"2A", "2D", "2F", "2H", "2M", "2R", "2C", "2X"};
            var codeThree = new[] {"3A", "3G", "3U", "3S", "3C", "3N", "3X"};
            var codeFour = new[] {"4A", "4P"};
            var codeFive = new[] {"5A", "5B", "5C", "5D", "5E", "5F", "5G", "5H", "5I", "5L", "5X"};
            var codeSix = new[] {"6A", "6B"};

            _wellTypes = codeOne.Concat(codeTwo)
                .Concat(codeThree)
                .Concat(codeFour)
                .Concat(codeFive)
                .Concat(codeSix);

            RuleSet("R1", () =>
            {
                RuleFor(src => src.WellTypeIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellTypeCode)
                    .NotEmpty()
                    .Length(1, 4)
                    .Matches("^[1-5]")
                    .Must(code => _wellTypes.Any(x => x.StartsWith(code)))
                    .Unless(src => src.WellTypeCode == "CV");

                RuleFor(src => src.WellTypeDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.WellTypeWellIdentifier)
                    .NotEmpty()
                    .Length(20);
            });
        }
    }
}