namespace uic_etl.models.dtos
{
    public class HeaderInformation
    {
        // The time the etl tool is run. 2010-03-24T16:22:08
        public string CreationTime { get; set; }

        // Comments that go in the header section. THIS IS A SAMPLE XML DOC V.2
        public string Comments { get; set; }
    }
}