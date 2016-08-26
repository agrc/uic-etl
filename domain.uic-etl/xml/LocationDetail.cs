﻿using System;
using System.Linq;
using domain.uic_etl.sde;
using FluentValidation;

namespace domain.uic_etl.xml
{
    public class LocationDetail
    {
        public LocationDetail(WellSdeModel well, FacilitySdeModel facility, double lon, double lat, Func<Guid, string> generateIdentifierFunc)
        {
            LocationIdentifier = generateIdentifierFunc(Guid.NewGuid());
            LocationAddressCounty = facility.CountyFips;
            LocationAccuracyValueMeasure = string.IsNullOrEmpty(well.LocationAccuracy) ? "U" : well.LocationAccuracy;
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
        public string LocationAccuracyValueMeasure { get; set; }
        public int LocationAddressCounty { get; set; }
        public string LocationIdentifier { get; set; }
        public string HorizontalCollectionMethodCode { get; set; }
        public string LocationPointLineAreaCode { get; set; }
        public string SourceMapScaleNumeric { get; set; }
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
                        if (!int.TryParse(x, out accuracy))
                        {
                            return false;
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
                    .Must(code => new [] {"NA", "1", "2", "3", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "U"}
                         .Contains(code.ToUpper()));
            });
        }
    }
}