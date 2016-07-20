﻿using System;
using System.Collections.Generic;
using domain.uic_etl.sde;

namespace domain.uic_etl.xml
{
    public class WellDetail
    {
        public WellDetail()
        {
            WellStatusDetail = new List<WellStatusDetail>();
            WellTypeDetail = new List<WellTypeDetail>();
        }

        public string WellName { get; set; }
        public int WellIdentifier { get; set; }
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
        public string WellTypeCode { get; set; }
        public List<WellStatusDetail> WellStatusDetail { get; set; }
        public List<WellTypeDetail> WellTypeDetail { get; set; }
        public LocationDetailModel LocationDetail { get; set; }
        public List<ViolationDetail> WellViolationDetail { get; set; }
        public List<WellInspectionDetail> WellInspectionDetail { get; set; }
        public List<MiTestDetail> MitTestDetail { get; set; }
        public List<EngineeringDetail> EngineeringDetail { get; set; }
        public List<WasteDetail> WasteDetail { get; set; }
    }
}