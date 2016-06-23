﻿using System;

namespace domain.uic_etl.xml
{
    public class WellDetail
    {
        public string WellName { get; set; }
        public int EventType { get; set; }
        public string WellTypeCode { get; set; }
        public string WellIdentifier { get; set; }
        public string WellAquiferExemptionInjectionCode { get; set; }
        public string WellTotalDepthNumeric { get; set; }
        public string WellHighPriorityDesignationCode { get; set; }
        public string WellContactIdentifier { get; set; }
        public string WellFacilityIdentifier { get; set; }
        public string WellGeologyIdentifier { get; set; }
        public string WellSiteAreaNameText { get; set; }
        public string WellPermitIdentifier { get; set; }
        public string WellStateIdentifier { get; set; }
        public string WellStateTribalCode { get; set; }
        public string WellInSourceWaterAreaLocationText { get; set; }
        public object[] WellStatusDetail { get; set; }
        public object[] WellTypeDetail { get; set; }
        public object[] LocationDetail { get; set; }
        public object[] WellViolationDetail { get; set; }
        public object[] WellInspectionDetail { get; set; }
        public object[] MitTestDetail { get; set; }
        public object[] EngineeringDetail { get; set; }
        public object[] WasteDetail { get; set; }
    }
}