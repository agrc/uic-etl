using System;

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
                var index = _length.IndexOf(".", StringComparison.Ordinal);
                if (index > -1)
                {
                    _length = _length.Remove(index, _length.Length - index);
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