using System;

namespace domain.uic_etl.sde
{
    public class ContactSdeModel
    {
        public static string[] Fields = { "GUID", "ContactType", "ContactPhone", "ContactName", "ContactMailCity", "ContactMailState", "ContactMailAddress", "ZipCode5", "ZipCode4"};

        public string ContactPhone { get; set; }
        public string ContactName { get; set; }
        public string ContactMailCity { get; set; }
        public string ContactMailState { get; set; }
        public string ContactMailAddress { get; set; }
        public string ZipCode5 { get; set; }
        public string ZipCode4 { get; set; }
        public int ContactType { get; set; }
        public Guid Guid { get; set; }
    }
}