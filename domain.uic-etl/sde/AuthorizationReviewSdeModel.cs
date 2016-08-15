using System;

namespace domain.uic_etl.sde
{
    public class AuthorizationActionSdeModel
    {
        public static string[] Fields = { "GUID", "AuthorizationActionType", "AuthorizationActionDate" };

        public string AuthorizeActionType { get; set; }
        public DateTime? AuthorizeActionDate { get; set; }
        public Guid Guid { get; set; }
    }
}