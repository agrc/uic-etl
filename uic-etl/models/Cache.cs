using System;
using System.Collections.Generic;
using ESRI.ArcGIS.Geodatabase;

namespace uic_etl.models
{
    public static class Cache
    {
        public static Dictionary<string, ICodedValueDomain> DomainDicionary = new Dictionary<string, ICodedValueDomain>();
    }
}