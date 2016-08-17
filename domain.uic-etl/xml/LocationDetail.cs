using System;
using domain.uic_etl.sde;

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
}