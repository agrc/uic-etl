using System;
using domain.uic_etl.sde;

namespace domain.uic_etl.xml
{
    public class LocationDetailModel
    {
        public LocationDetailModel(WellSdeModel well, FacilitySdeModel facility, double lon, double lat)
        {
            WellTypeWellIdentifer = well.Guid;
            WellAddressCounty = facility.CountyFips;
            LocationAccuracyValueMeasure = well.LocationAccuracy;
            GeographicReferencePointCode = "026";
            HorizontalCoordinateReferenceSystemDatumCode = "002";
            HorizontalCollectionMethodCode = well.LocationMethod;
            LocationPointLiveAreaCode = "001";
            SourceMapScaleNumeric = well.LocationAccuracy;
            LocationWellIdentifier = well.Guid;
            LatitudeMeasure = lat;
            LongitudeMeasure = lon;
        }

        public double LongitudeMeasure { get; set; }

        public double LatitudeMeasure { get; set; }

        public string HorizontalCoordinateReferenceSystemDatumCode { get; set; }

        public string GeographicReferencePointCode { get; set; }

        public string LocationAccuracyValueMeasure { get; set; }

        public int WellAddressCounty { get; set; }

        public Guid WellTypeWellIdentifer { get; set; }

        public string HorizontalCollectionMethodCode { get; set; }

        public string LocationPointLiveAreaCode { get; set; }

        public string SourceMapScaleNumeric { get; set; }

        public Guid LocationWellIdentifier { get; set; }
    }
}