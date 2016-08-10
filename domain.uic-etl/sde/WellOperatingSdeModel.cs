using System;

namespace domain.uic_etl.sde
{
    public class WellOperatingSdeModel
    {
        public static string[] Fields = { "GUID", "MaxInjectionRate", "OnSiteVolume", "OffSiteVolume", "Well_FK" };

        public double MaxInjectionRate { get; set; }
        public double OnSiteVolume { get; set; }
        public double OffSiteVolume { get; set; }
        public Guid WellFk { get; set; }
        public Guid Guid { get; set; }
    }
}