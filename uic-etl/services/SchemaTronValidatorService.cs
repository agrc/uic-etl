using System.Collections.Generic;
using domain.uic_etl.xml;
using FluentValidation.Results;
using uic_etl.models.dtos;
using System.Linq;

namespace uic_etl.services
{
    public class SchemaTronValidatorService
    {
        public void ValidateSixtyOneB(FacilityDetail facility, int wellCount)
        {
            if (wellCount > 0)
            {
                return;
            }

            if (facility.FacilityInspectionDetail.Count == 0)
            {
                var errors = new Dictionary<string, IEnumerable<ValidationFailure>>
                {
                    {
                        "61b",
                        new[] {new ValidationFailure("FacilityInspectionDetail", "Facility has no wells and no Facility Inspection with the code NW", null),}
                    }
                };

                var model = new LogModel("FacilityDetail", facility.FacilityIdentifier, errors);

                ReportingService.LogErrors(new[] {model});

                return;
            }

            if (facility.FacilityInspectionDetail.Count(x => x.InspectionTypeActionCode == "NW") < 1)
            {
                var errors = new Dictionary<string, IEnumerable<ValidationFailure>>
                {
                    {
                        "61b",
                        new[] {new ValidationFailure("FacilityInspectionDetail", "Facility has no wells and no Facility Inspection with the code NW", null),}
                    }
                };

                var model = new LogModel("FacilityDetail", facility.FacilityIdentifier, errors);

                ReportingService.LogErrors(new[] {model});
            }
        }
    }
}