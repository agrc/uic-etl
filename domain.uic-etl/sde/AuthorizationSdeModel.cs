using System;
using System.Linq;

namespace domain.uic_etl.sde
{
    public class AuthorizationSdeModel
    {
        public static string[] Fields = { "GUID", "AuthorizationType", "OwnerSectorType", "AuthorizationID" };
        private string _ownerSectorType;

        public string AuthorizeType { get; set; }

        public string OwnerSectorType
        {
            get
            {
                if (new []{ "sg", "lg"}.Contains(_ownerSectorType.ToLower()))
                {
                    return "OS";
                }
                return _ownerSectorType;
            }
            set { _ownerSectorType = value; }
        }

        public string AuthorizeNumber { get; set; }
        public Guid Guid { get; set; }
    }
}