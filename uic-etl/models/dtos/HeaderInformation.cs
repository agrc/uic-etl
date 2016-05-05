namespace uic_etl.models.dtos
{
    public class HeaderInformation
    {
        // The header Title. DATA SUBMISSION FOR QUARTER #1 FY 2010
        public string Title { get; set; }

        // The time the etl tool is run. 2010-03-24T16:22:08
        public string CreationTime { get; set; }

        // Comments that go in the header section. THIS IS A SAMPLE XML DOC V.2
        public string Comments { get; set; }
    }
}