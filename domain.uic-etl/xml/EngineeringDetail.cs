using FluentValidation;

namespace domain.uic_etl.xml
{
    public class EngineeringDetail
    {
        public string EngineeringMaximumFlowRateNumeric { get; set; }
        public string EngineeringPermittedOnsiteInjectionVolumeNumeric { get; set; }
        public string EngineeringPermittedOffsiteInjectionVolumeNumeric { get; set; }
        public string EngineeringWellIdentifier { get; set; }
        public string EngineeringIdentifier { get; set; }
    }

    public class EngineeringDetailValidator : AbstractValidator<EngineeringDetail>
    {
        public EngineeringDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.EngineeringWellIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.EngineeringIdentifier)
                    .NotEmpty()
                    .Length(20);
            });

            // Todo: only valid for class I, II and designated wells in appendix B or national db dictionary
            // todo: how do we get the well status here? validate in program only under these conditions
            /*
             * Class I ***
                "1H", "1I", "1M", "1R", "1C" , "1W", "Other", "1X"

               Class II ***
                "2A", "2D", "2F", "2H", "2M", "2R", "2C", "Other", "2X" 
             */

            RuleSet("R2C", () =>
            {
                RuleFor(src => src.EngineeringMaximumFlowRateNumeric);

                RuleFor(src => src.EngineeringPermittedOnsiteInjectionVolumeNumeric);

                RuleFor(src => src.EngineeringPermittedOffsiteInjectionVolumeNumeric);
            });
        }
    }
}