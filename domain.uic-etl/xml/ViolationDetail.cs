using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class ViolationDetail
    {
        public ViolationDetail()
        {
            ResponseDetail = new List<ResponseDetail>();
        }

        public Guid Guid { get; set; }
        public string ViolationIdentifier { get; set; }
        public string ViolationContaminationCode { get; set; }
        public string ViolationEndangeringCode { get; set; }
        public string ViolationReturnComplianceDate { get; set; }
        public string ViolationSignificantCode { get; set; }
        public string ViolationDeterminedDate { get; set; }
        public string ViolationTypeCode { get; set; }
        public string ViolationFacilityIdentifier { get; set; }
        public List<ResponseDetail> ResponseDetail { get; set; }
        public string FacilityId { get; set; }
        public string ViolationWellIdentifier { get; set; }
    }

    public class ViolationDetailValidator : AbstractValidator<ViolationDetail>
    {
        public ViolationDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.ViolationIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.ViolationDeterminedDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.ViolationTypeCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"UI", "MI", "MO", "IP", "PA", "FO", "FA", "MR", "FI", "FR", "OM", "OT"}.Contains(code.ToUpper()));
            });

            RuleSet("R1C", () =>
            {
                RuleFor(src => src.FacilityId)
                    .NotEmpty()
                    .Must((src, value) => !new[] {"UI", "MI", "MO", "IP", "PA", "OM"}.Contains(src.ViolationTypeCode.ToUpper()))
                    .When(src => string.IsNullOrEmpty(src.ViolationWellIdentifier));

                RuleFor(src => src.ViolationWellIdentifier)
                    .NotEmpty()
                    .When(src => string.IsNullOrEmpty(src.FacilityId));
            });

            RuleSet("R2", () =>
            {
                RuleFor(src => src.ViolationContaminationCode)
                    .NotEmpty()
                    .Length(1)
                    .Must(code => new[] {"Y", "N", "U"}.Contains(code.ToUpper()));

                RuleFor(src => src.ViolationEndangeringCode)
                    .NotEmpty()
                    .Length(1)
                    .Must(code => new[] {"Y", "N", "U"}.Contains(code.ToUpper()));

                RuleFor(src => src.ViolationReturnComplianceDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.ViolationSignificantCode)
                    .NotEmpty()
                    .Length(1)
                    .Must(code => new[] {"Y", "N", "U"}.Contains(code.ToUpper()));
            });
        }
    }
}