﻿﻿using System.Linq;
using FluentValidation;

﻿namespace domain.uic_etl.xml
{
    public class WellStatusDetail
    {
        public string WellStatusIdentifier { get; set; }
        public string WellStatusDate { get; set; }
        public string WellStatusOperatingStatusCode { get; set; }
        public string WellStatusWellIdentifier { get; set; }
    }

    public class WelLStatusDetailValidator : AbstractValidator<WellStatusDetail>
    {
        public WelLStatusDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.WellStatusIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellStatusDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.WellStatusWellIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellStatusOperatingStatusCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"AC", "UC", "TA", "PA", "AN", "PW", "PI"}.Contains(code.ToUpper()));
            });
        }
    }
}