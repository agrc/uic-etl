using System;

namespace domain.uic_etl.sde
{
    public class AuthorizationSdeModel
    {
        public static string[] Fields = { "GUID", "AuthorizeType", "OwnerSectorType", "AuthorizeNumber" };

        public string AuthorizeType { get; set; }
        public string OwnerSectorType { get; set; }
        public string AuthorizeNumber { get; set; }
        public Guid Guid { get; set; }
    }
}