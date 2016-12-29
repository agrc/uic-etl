using System;
using System.Collections.Generic;
using domain.uic_etl.xml;

namespace uic_etl.comparers
{
    public class ResponseDetailComparer : IEqualityComparer<ResponseDetail>
    {
        public bool Equals(ResponseDetail x, ResponseDetail y)
        {
            if (y == null && x == null)
            {
                return true;
            }

            if (x == null | y == null)
            {
                return false;
            }

            if (x.ResponseEnforcementIdentifier == y.ResponseEnforcementIdentifier)
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(ResponseDetail obj)
        {
            return obj.ResponseEnforcementIdentifier.GetHashCode();
        }
    }
}