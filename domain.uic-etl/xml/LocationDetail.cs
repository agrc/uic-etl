using System;
using System.Linq;
using domain.uic_etl.sde;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class LocationDetail
    {
        private string _sourceMapScaleNumeric;

        public LocationDetail(WellSdeModel well, FacilitySdeModel facility, double lon, double lat, Func<Guid, string> generateIdentifierFunc)
        {
            LocationIdentifier = generateIdentifierFunc(well.Guid);
            LocationAddressCounty = facility.CountyFips;
            GeographicReferencePointCode = "026";
            HorizontalCoordinateReferenceSystemDatumCode = "002";
            HorizontalCollectionMethodCode = string.IsNullOrEmpty(well.LocationMethod) ? "U" : well.LocationMethod;
            LocationPointLineAreaCode = "001";
            SourceMapScaleNumeric = string.IsNullOrEmpty(well.LocationAccuracy) ? "U" : well.LocationAccuracy;
            LocationWellIdentifier = generateIdentifierFunc(well.Guid);
            LatitudeMeasure = lat;
            LongitudeMeasure = lon;
        }

        public double LongitudeMeasure { get; set; }
        public double LatitudeMeasure { get; set; }
        public string HorizontalCoordinateReferenceSystemDatumCode { get; set; }
        public string GeographicReferencePointCode { get; set; }
        public object LocationAccuracyValueMeasure
        {
            get
            {
                if (string.IsNullOrEmpty(HorizontalCollectionMethodCode) || HorizontalCollectionMethodCode == "U")
                {
                    return "U";
                }

                switch (HorizontalCollectionMethodCode)
                {
                    case "001":
                    {
                        return "50.0";
                    }
                    case "007":
                    {
                        return "200.0";
                    }
                    case "012":
                    case "013":
                    {
                        return "1.0";
                    }
                    case "014":
                    {
                        return "3.0";
                    }
                    case "016":
                    {
                        return "15.0";
                    }
                    case "011":
                    case "017":
                    {
                        return "80.0";
                    }
                    case "021":
                    {
                        return SourceMapScaleNumeric == "J" ? "200.0" : "40.0";
                    }
                    case "023":
                    {
                        return "400.0";
                    }
                    case "024":
                    {
                        return "800.0";
                    }
                    case "027":
                    {
                        return "900.0";
                    }
                    default:
                    {
                        return "U";
                    }
                }
            }
        }
        public int LocationAddressCounty { get; set; }
        public string LocationIdentifier { get; set; }
        public string HorizontalCollectionMethodCode { get; set; }
        public string LocationPointLineAreaCode { get; set; }
        public string SourceMapScaleNumeric
        {
            get { return _sourceMapScaleNumeric == "N" ? "NA" : _sourceMapScaleNumeric; }
            set { _sourceMapScaleNumeric = value; }
        }
        public string LocationWellIdentifier { get; set; }
    }

    public class LocationDetailValidator : AbstractValidator<LocationDetail>
    {
        public LocationDetailValidator()
        {
            RuleSet("R1", () =>
            {
                RuleFor(src => src.LocationIdentifier)
                    .NotEmpty()
                    .Length(20);

                RuleFor(src => src.LocationWellIdentifier)
                    .NotEmpty()
                    .Length(20);
            });

            RuleSet("R2", () =>
            {
                RuleFor(src => src.LatitudeMeasure)
                    .GreaterThan(-90)
                    .LessThan(90);

                RuleFor(src => src.LongitudeMeasure)
                    .GreaterThan(-180)
                    .LessThan(180);

                RuleFor(src => src.LocationAccuracyValueMeasure)
                    .Must(x =>
                    {
                        int accuracy;
                        if (!int.TryParse(x.ToString(), out accuracy))
                        {
                            return new[] {"NA", "U"}.Contains(x.ToString());
                        }

                        return accuracy > 0 && accuracy < 1000000;
                    });
            });

            RuleSet("R2C", () =>
            {
                RuleFor(src => src.HorizontalCollectionMethodCode)
                    .NotEmpty()
                    .Length(3)
                    .Must(code => new[] {"001", "007", "011", "012", "013", "014", "015", "016", "017", "021", "022", "023", "024", "025", "026", "027", "028"}
                        .Contains(code));

                RuleFor(src => src.SourceMapScaleNumeric)
                    .NotEmpty()
                    .Length(1, 2)
                    .Must(code => new[] {"NA", "1", "2", "3", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "U"}
                        .Contains(code.ToUpper()));
            });
        }
    }
}