using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class WellInspectionDetail
    {
        public WellInspectionDetail()
        {
            CorrectionDetail = new List<CorrectionDetail>();
        }

        public string InspectionIdentifier { get; set; }
        public string InspectionAssistanceCode { get; set; }
        public string InspectionDeficiencyCode { get; set; }
        public string InspectionActionDate { get; set; }
        public string InspectionIcisComplianceMonitoringReasonCode { get; set; }
        public string InspectionIcisComplianceMonitoringTypeCode { get; set; }
        public string InspectionIcisComplianceActivityTypeCode { get; set; }
        public string InspectionIcisMoaName { get; set; }
        public string InspectionIcisRegionalPriorityName { get; set; }
        public string InspectionTypeActionCode { get; set; }
        public string InspectionWellIdentifier { get; set; }
        public List<CorrectionDetail> CorrectionDetail { get; set; }
    }

    public class WellInspectionDetailValidator : AbstractValidator<WellInspectionDetail>
    {
        public WellInspectionDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.InspectionIdentifier)
                    .NotEmpty();

                RuleFor(src => src.InspectionActionDate)
                    .NotEmpty()
                    .Length(8)
                    .Matches(@"\d{8}");

                RuleFor(src => src.InspectionTypeActionCode)
                    .NotEmpty()
                    .Length(2)
                    .Must(code => new[] {"MI", "EC", "CO", "WP", "RP", "NW", "OT", "FI"}.Contains(code.ToUpper()));

                RuleFor(src => src.InspectionWellIdentifier)
                    .NotEmpty();
            });

            // todo only for DI programs (Last 2 characters of primacy agency code = `DI`
            // I do not think we have any DI types
//            RuleSet("R2C", () =>
//            {
//                RuleFor(src => src.InspectionAssistanceCode)
//                    .NotEmpty()
//                    .Length(2)
//                    .Must(code => new[] {"GN", "SS", "BH", "NO"}.Contains(code.ToUpper()));
//
//                RuleFor(src => src.InspectionDeficiencyCode)
//                    .NotEmpty()
//                    .Length(2)
//                    .Must(code => new[] {"OS", "CM", "BH", "NO"}.Contains(code.ToUpper()));
//
//                RuleFor(src => src.InspectionIcisComplianceMonitoringReasonCode)
//                    .NotEmpty()
//                    .Length(2)
//                    .Must(code => new[] {"AP", "CP", "MA"}.Contains(code.ToUpper()));
//
//                RuleFor(src => src.InspectionIcisComplianceMonitoringTypeCode)
//                    .NotEmpty()
//                    .Length(3)
//                    .Must(code => new[] { "CAI", "CEI", "CSE", "IUI", "IWS", "OCA", "OSV", "OVI", "PRM", "VLN" }.Contains(code.ToUpper()));
//
//                RuleFor(src => src.InspectionIcisComplianceActivityTypeCode)
//                    .NotEmpty()
//                    .Length(2)
//                    .Must(code => new[] {"IR", "CS", "CV", "OR", "VD"}.Contains(code.ToUpper()));
//
//                RuleFor(src => src.InspectionIcisMoaName)
//                    .NotEmpty()
//                    .Length(1, 50);
//
//                RuleFor(src => src.InspectionIcisRegionalPriorityName)
//                    .NotEmpty()
//                    .Length(1, 12);
//            });
        }
    }
}