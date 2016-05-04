namespace uic_etl.models
{
    internal class EtlOptions
    {
        public string SdeConnectionPath { get; set; }
        public string OutputXmlPath { get; set; }
        public bool Verbose { get; set; }
        public Configruation Configruation { get; set; }
    }
}