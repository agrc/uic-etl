namespace domain.uic_etl.sde
{
    public class VerticalWellEventSdeModel
    {
        public static string[] Fields = {"Length", "EventDescriptor"};
            
        public string Length { get; set; }
        public string EventDescriptor { get; set; }

        public bool IsTotalDepth
        {
            get { return EventDescriptor == "Total Depth"; }
        }
    }
}