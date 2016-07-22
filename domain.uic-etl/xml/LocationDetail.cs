using System;
using domain.uic_etl.sde;

namespace domain.uic_etl.xml
{
    public class LocationDetail
    {
        public LocationDetail(WellSdeModel well, FacilitySdeModel facility, double lon, double lat)
        {
            LocationIdentifier = "";
            LocationAddressCounty = facility.CountyFips;
            LocationAccuracyValueMeasure = well.LocationAccuracy;
            GeographicReferencePointCode = "026";
            HorizontalCoordinateReferenceSystemDatumCode = "002";
            HorizontalCollectionMethodCode = well.LocationMethod;
            LocationPointLineAreaCode = "001";
            SourceMapScaleNumeric = well.LocationAccuracy;
            LocationWellIdentifier = well.Guid.ToString();
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