using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class FacilityDetail
    {
        public FacilityDetail()
        {
            FacilityViolationDetail = new List<ViolationDetail>();
            FacilityInspectionDetail = new List<WellInspectionDetail>();
        }

        public string FacilityIdentifier { get; set; }
        public Guid Guid { get; set; }
        public string LocalityName { get; set; }
        public string FacilitySiteName { get; set; }
        public string FacilityPetitionStatusCode { get; set; }
        public string LocationAddressStateCode { get; set; }
        public string FacilityStateIdentifier { get; set; }
        public string LocationAddressText { get; set; }
        public string FacilitySiteTypeCode { get; set; }
        public string LocationAddressPostalCode { get; set; }
        public string NaicsCode { get; set; }
        public List<ViolationDetail> FacilityViolationDetail { get; set; }
        public List<WellInspectionDetail> FacilityInspectionDetail { get; set; } 
    }

    public class FacilityDetailValidator : AbstractValidator<FacilityDetail>
    {
        public FacilityDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.FacilityIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.FacilitySiteName)
                    .NotEmpty()
                    .Length(1, 80);

                RuleFor(src => src.FacilityStateIdentifier)
                    .NotEmpty()
                    .Length(1, 50);

                RuleFor(src => src.LocationAddressText)
                    .NotEmpty()
                    .Length(1, 150);
            });

            RuleSet("R2C", () =>
            {
                RuleFor(src => src.NaicsCode)
                    .Length(1, 6);
            });

            RuleSet("R2C-1H", () =>
            {
                //todo only for facilities with 1H well
                RuleFor(src => src.FacilityPetitionStatusCode)
                    .Length(2)
                    .Must(code => new[] {"AP", "DA", "NA"}.Contains(code));
            });

            RuleSet("R2C-Class1", () =>
            {
                RuleFor(src => src.FacilitySiteTypeCode)
                    .Length(1)
                    .Must(code => new[] {"C", "N", "U"}.Contains(code));
            });
        }
    }
}