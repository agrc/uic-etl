namespace domain.uic_etl.sde
{
    public class VerticalWellEventSdeModel
    {
        public static string[] Fields = {"Length", "EventDescriptor"};
        private string _length;

        public string Length
        {
            get
            {
                if (_length.Contains("."))
                {
                    return _length;
                }

                return string.Format("{0}.0", _length);
            }
            set { _length = value; }
        }

        public string EventDescriptor { get; set; }

        public bool IsTotalDepth
        {
            get { return EventDescriptor == "Total Depth"; }
        }
    }
}