using System;

namespace domain.uic_etl.xml
{
    public class CorrectionDetail
    {
        public string CorrectionIdentifier { get; set; }
        public string CorrectiveActionTypeCode { get; set; }
        public string CorrectionCommentText { get; set; }
        public string CorrectionInspectionIdentifier { get; set; }
    }
}