using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class WellDetail
    {
        public WellDetail()
        {
            WellStatusDetail = new List<WellStatusDetail>();
            WellTypeDetail = new List<WellTypeDetail>();
            WellViolationDetail = new List<ViolationDetail>();
            WellInspectionDetail = new List<WellInspectionDetail>();
            MitTestDetail = new List<MiTestDetail>();
            EngineeringDetail = new List<EngineeringDetail>();
            WasteDetail = new List<WasteDetail>();
        }

        public string WellName { get; set; }
        public string WellIdentifier { get; set; }
        public string WellAquiferExemptionInjectionCode { get; set; }
        public string WellTotalDepthNumeric { get; set; }
        public string WellHighPriorityDesignationCode { get; set; }
        public string WellContactIdentifier { get; set; }
        public string WellFacilityIdentifier { get; set; }
        public string WellGeologyIdentifier { get; set; }
        public string WellSiteAreaNameText { get; set; }
        public string WellPermitIdentifier { get; set; }
        public string WellStateIdentifier { get; set; }
        public string WellStateTribalCode { get; set; }
        public string WellInSourceWaterAreaLocationText { get; set; }
        public string WellTypeCode { get; set; }
        public List<WellStatusDetail> WellStatusDetail { get; set; }
        public List<WellTypeDetail> WellTypeDetail { get; set; }
        public LocationDetail LocationDetail { get; set; }
        public List<ViolationDetail> WellViolationDetail { get; set; }
        public List<WellInspectionDetail> WellInspectionDetail { get; set; }
        public List<MiTestDetail> MitTestDetail { get; set; }
        public List<EngineeringDetail> EngineeringDetail { get; set; }
        public List<WasteDetail> WasteDetail { get; set; }
        public int WellClass { get; set; }
    }

    public class WellDetailValidator : AbstractValidator<WellDetail>
    {
        public WellDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.WellIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellFacilityIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellContactIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellPermitIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.WellStateIdentifier)
                    .NotEmpty()
                    .Length(1, 50);
            });

            // todo: required for DI programs only
//            RuleSet("R1C", () =>
//            {
//                RuleFor(src => src.WellStateTribalCode)
//                    .NotEmpty()
//                    .Length(2, 3);
//            });

            // todo: tribal code list is super log and i'm not sure how it works

            RuleSet("R2C", () =>
            {
                RuleFor(src => src.WellName)
                    .NotEmpty()
                    .Length(1, 80);
            });

            RuleSet("R2C-except-6", () =>
            {
                // todo: requried for all wells except IV
                RuleFor(src => src.WellAquiferExemptionInjectionCode)
                    .NotEmpty()
                    .Length(1)
                    .Must(code => new[] {"Y", "N", "U"}.Contains(code.ToUpper()));
            });

            RuleSet("R2C-1-2", () =>
            {
                // todo: required for class I and II
                RuleFor(src => src.WellTotalDepthNumeric)
                    .NotEmpty()
                    .Must(value =>
                    {
                        decimal depth;

                        if (!decimal.TryParse(value, out depth))
                        {
                            return false;
                        }

                        return depth > 0 && depth < 100000;
                    });
            });

            RuleSet("R2C-5", () =>
            {
                // todo: required for class V
                RuleFor(src => src.WellHighPriorityDesignationCode)
                    .NotEmpty()
                    .Length(1)
                    .Must(code => new[] {"Y", "N", "U"}.Contains(code.ToUpper()));

                // todo: skipping geology
            });

            RuleSet("R2C-3-4", () =>
            {
                // todo: class III and IV 
                RuleFor(src => src.WellSiteAreaNameText)
                    .NotEmpty()
                    .Length(1, 50);
            });
        }
    }
}