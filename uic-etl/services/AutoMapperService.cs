using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.CSharp.RuntimeBinder;
using uic_etl.models;
using uic_etl.models.dtos;

namespace uic_etl.services
{
    public static class AutoMapperService
    {
        public static IMapper CreateMappings()
        {
            var config = new MapperConfiguration(_ =>
            {
                _.CreateMap<FacilitySdeModel, FacilityDetailModel>()
                    .ForMember(dest => dest.FacilityIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.Guid, opts => opts.MapFrom(src => src.Guid))
                    .ForMember(dest => dest.LocalityName, opts => opts.MapFrom(src => src.FacilityCity))
                    .ForMember(dest => dest.FacilityPetitionStatusCode,
                        opts => opts.MapFrom(src => src.NoMigrationPetStatus))
                    .ForMember(dest => dest.FacilitySiteName, opts => opts.MapFrom(src => src.FacilityName))
                    .ForMember(dest => dest.FacilitySiteTypeCode, opts => opts.MapFrom(src => src.FacilityType))
                    .ForMember(dest => dest.FacilityStateIdentifier, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.FacilityViolationDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.LocationAddressPostalCode, opts => opts.MapFrom(src => src.FacilityZip))
                    .ForMember(dest => dest.LocationAddressStateCode, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.LocationAddressText, opts => opts.MapFrom(src => src.FacilityAddress))
                    .ForMember(dest => dest.Xmlns, opts => opts.Ignore());

                _.CreateMap<FacilityViolationSdeModel, FacilityViolationDetail>()
                    .ForMember(dest => dest.ViolationIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.ViolationContaminationCode,
                        opts => opts.MapFrom(src => src.UsdwContamination))
                    .ForMember(dest => dest.ViolationEndangeringCode, opts => opts.MapFrom(src => src.Endanger))
                    .ForMember(dest => dest.ViolationReturnComplianceDate,
                        opts => opts.MapFrom(src => src.ReturnToComplianceDate.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.ViolationSignificantCode,
                        opts => opts.MapFrom(src => src.SignificantNonCompliance))
                    .ForMember(dest => dest.ViolationDeterminedDate,
                        opts => opts.MapFrom(src => src.ViolationDate.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.ViolationTypeCode, opts => opts.MapFrom(src => src.ViolationType))
                    .ForMember(dest => dest.ViolationFacilityIdentifier, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.Guid, opts => opts.MapFrom(src => src.Guid))
                    .ForMember(dest => dest.WellId, opts => opts.MapFrom(src => src.WellId))
                    .ForMember(dest => dest.FacilityResponseDetails, opts => opts.Ignore());

                _.CreateMap<FacilityEnforcementSdeModel, FacilityResponseDetail>()
                    .ForMember(dest => dest.ResponseViolationIdentifier, opts => opts.MapFrom(src => src.Guid))
                    .ForMember(dest=> dest.ResponseEnforcementIdentifier, opts => opts.Ignore());

                _.CreateMap<WellSdeModel, WellDetail>()
                    .ForMember(dest => dest.WellTotalDepthNumeric, opts => opts.Ignore())
                    .ForMember(dest => dest.WellIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.WellAquiferExemptionInjectionCode, opts => opts.MapFrom(src => src.InjectionAquiferExempt))
                    .ForMember(dest => dest.WellHighPriorityDesignationCode, opts => opts.MapFrom(src => src.HighPriority))
                    .ForMember(dest => dest.WellContactIdentifier, opts => opts.Ignore())// TODO github #4
                    .ForMember(dest => dest.WellFacilityIdentifier, opts => opts.MapFrom(src => src.FacilityGuid))
                    .ForMember(dest => dest.WellGeologyIdentifier, opts => opts.Ignore()) // TODO github #5
                    .ForMember(dest => dest.WellSiteAreaNameText, opts => opts.Ignore())
                    .ForMember(dest => dest.WellPermitIdentifier, opts => opts.MapFrom(src => src.AuthorizationGuid))
                    .ForMember(dest => dest.WellStateIdentifier, opts => opts.MapFrom(src => src.Guid))
                    .ForMember(dest => dest.WellStateTribalCode, opts => opts.UseValue("UT"))
                    .ForMember(dest => dest.WellName, opts => opts.MapFrom(src => src.WellName))
                    .ForMember(dest => dest.WellTypeCode, opts => opts.MapFrom(src => src.WellSubClass))
                    .ForMember(dest => dest.WellInSourceWaterAreaLocationText, opts => opts.MapFrom(src => src.WellSwpz))
                    .ForMember(dest => dest.WellStatusDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.WellTypeDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.LocationDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.WellViolationDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.MitTestDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.WellInspectionDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.EngineeringDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.WasteDetail, opts => opts.Ignore());

                _.CreateMap<WellStatusSdeModel, WellStatusDetail>()
                    .ForMember(dest => dest.WellStatusIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.WellStatusDate,
                        opts => opts.MapFrom(src => src.OperatingStatusDate.ToString("yyyMMdd")))
                    .ForMember(dest => dest.WellStatusWellIdentifier, opts => opts.MapFrom(src => src.WellGuid))
                    .ForMember(dest => dest.WellStatusOperatingStatusCode, opts => opts.MapFrom(src => src.OperatingStatusType));

                _.CreateMap<WellInspectionSdeModel, WellInspectionDetail>()
                    .ForMember(dest => dest.InspectionIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.InspectionAssistanceCode,
                        opts => opts.MapFrom(src => src.InspectionAssistance))
                    .ForMember(dest => dest.InspectionDeficiencyCode, opts => opts.MapFrom(src => src.InspectionDeficiency))
                    .ForMember(dest => dest.InspectionActionDate, opts => opts.MapFrom(src => src.InspectionDate.ToString("yyyMMdd")))
                    .ForMember(dest => dest.InspectionIdisComplianceMonitoringReasonCode, opts => opts.MapFrom(src => src.IcisCompMonActReason))
                    .ForMember(dest => dest.InspectionIcisComplianceMonitoringTypeCode, opts => opts.MapFrom(src => src.IcisCompMonType))
                    .ForMember(dest => dest.InspectionIcisComplianceActivityTypeCode, opts => opts.MapFrom(src => src.IcisCompActType))
                    .ForMember(dest => dest.InspectionIcisMoaName, opts => opts.MapFrom(src => src.IcisMoaPriority))
                    .ForMember(dest => dest.InspectionIcisRegionalPriorityName, opts => opts.MapFrom(src => src.IcisRegionalPriority))
                    .ForMember(dest => dest.InspectionTypeActionCode, opts => opts.MapFrom(src => src.InspectionType))
                    .ForMember(dest => dest.InspectionWellIdentifier, opts => opts.MapFrom(src => src.WellFk));

                _.CreateMap<CorrectionSdeModel, CorrectionDetail>()
                    .ForMember(dest => dest.CorrectionIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.CorrectiveActionTypeCode, opts => opts.MapFrom(src => src.CorrectiveAction))
                    .ForMember(dest => dest.CorrectionCommentText, opts => opts.MapFrom(src => src.Comments))
                    .ForMember(dest => dest.CorrectionInspectionIdentifier, opts => opts.MapFrom(src => src.Guid));

                _.CreateMap<MiTestSdeModel, MiTestDetail>()
                    .ForMember(dest => dest.MechanicalIntegrityTestIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.MechanicalIntegrityTestCompletedDate, opts => opts.MapFrom(src => src.MitDate.ToString("yyyMMdd")))
                    .ForMember(dest => dest.MechanicalIntegrityTestResultCode, opts => opts.MapFrom(src => src.MitResult))
                    .ForMember(dest => dest.MechanicalIntegrityTestTypeCode, opts => opts.MapFrom(src => src.MitType))
                    .ForMember(dest => dest.MechanicalIntegrityTestRemedialActionDate, opts => opts.MapFrom(src => src.MitRemActDate.ToString("yyyMMdd")))
                    .ForMember(dest => dest.MechanicalIntegrityTestRemedialActionTypeCode, opts => opts.MapFrom(src => src.MitRemediationAction))
                    .ForMember(dest => dest.MechanicalIntegrityTestWellIdentifier, opts => opts.MapFrom(src => src.WellFk));

                _.CreateMap<WellOperatingSdeModel, EngineeringDetail>()
                    .ForMember(dest => dest.EngineeringMaximumFlowRateNumeric,
                        opts => opts.MapFrom(src => src.MaxInjectionRate))
                    .ForMember(dest => dest.EngineeringPermittedOnsiteInjectionVolumeNumeric, opts => opts.MapFrom(src => src.OnSiteVolume))
                    .ForMember(dest => dest.EngineeringPermittedOffsiteInjectionVolumeNumeric, opts => opts.MapFrom(src => src.OffSiteVolume))
                    .ForMember(dest => dest.EngineeringWellIdentifier, opts => opts.MapFrom(src => src.WellFk));

                _.CreateMap<WasteClassISdeModel, WasteDetail>()
                    .ForMember(dest => dest.WasteCode, opts => opts.MapFrom(src => src.WasteCode))
                    .ForMember(dest => dest.WasteStreamClassificationCode, opts => opts.MapFrom(src => src.WasteStream))
                    .ForMember(dest => dest.WasteWellIdentifier, opts => opts.MapFrom(src => src.WellFk));

                _.CreateMap<ConstituentClassISdeModel, ConstituentDetail>()
                    .ForMember(dest => dest.MeasureValue, opts => opts.MapFrom(src => src.Concentration))
                    .ForMember(dest => dest.MeasureUnitCode, opts => opts.MapFrom(src => src.Unit))
                    .ForMember(dest => dest.ConstituentNameText, opts => opts.MapFrom(src => src.ConstituentCode))
                    .ForMember(dest => dest.ConstituentWasteIdentifier, opts => opts.MapFrom(src => src.WasteGuid));
            });

            return config.CreateMapper();
        }

        public static FacilitySdeModel MapFacilityModel(IFeature row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new FacilitySdeModel
            {
                Guid = new Guid((string) row.Value[fieldMap["GUID"].Index]),
                FacilityName = GetDomainValue(row, fieldMap["FacilityName"]),
                FacilityAddress = GetDomainValue(row, fieldMap["FacilityAddress"]),
                FacilityCity = GetDomainValue(row, fieldMap["FacilityCity"]),
                FacilityId = GetDomainValue(row, fieldMap["FacilityID"]),
                FacilityZip = GetDomainValue(row, fieldMap["FacilityZip"]),
                FacilityType = GetDomainValue(row, fieldMap["FacilityType"]), 
                CountyFips = (int)row.Value[fieldMap["CountyFIPS"].Index],
                NoMigrationPetStatus = GetDomainValue(row, fieldMap["NoMigrationPetStatus"])
            };

            return model;
        }

        public static FacilityViolationSdeModel MapViolationModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new FacilityViolationSdeModel
            {
                UsdwContamination = GuardNull(row.Value[fieldMap["USDWContamination"].Index]),
                Endanger = GuardNull(row.Value[fieldMap["ENDANGER"].Index]),
                ReturnToComplianceDate = (DateTime) row.Value[fieldMap["ReturnToComplianceDate"].Index],
                SignificantNonCompliance = GuardNull(row.Value[fieldMap["SignificantNonCompliance"].Index]),
                ViolationDate = (DateTime) row.Value[fieldMap["ViolationDate"].Index],
                ViolationType = GuardNull(row.Value[fieldMap["ViolationType"].Index]),
            };

            var guidString = GuardNull(row.Value[fieldMap["GUID"].Index]);
            var guid = Guid.Empty;

            if (!string.IsNullOrEmpty(guidString))
            {
                guid = new Guid(guidString);
            }

            model.Guid = guid;

            var wellGuidString = GuardNull(row.Value[fieldMap["Well_FK"].Index]);
            var wellGuid = Guid.Empty;

            if (!string.IsNullOrEmpty(wellGuidString))
            {
                wellGuid = new Guid(wellGuidString);
            }

            model.WellId = wellGuid;

            return model;
        }

        public static FacilityEnforcementSdeModel MapResponseModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new FacilityEnforcementSdeModel
            {
                Guid = new Guid((string) row.Value[fieldMap["Guid"].Index])
            };

            return model;
        }

        public static WellSdeModel MapWellModel(IFeature row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellSdeModel
            {
                Guid = new Guid((string) row.Value[fieldMap["GUID"].Index]),
                WellId = GuardNull(row.Value[fieldMap["WellID"].Index]),
                FacilityGuid = new Guid((string)row.Value[fieldMap["Facility_FK"].Index]),
                AuthorizationGuid = new Guid((string)row.Value[fieldMap["Authorization_FK"].Index]),
                InjectionAquiferExempt = GuardNull(row.Value[fieldMap["InjectionAquiferExempt"].Index]),
                HighPriority = GuardNull(row.Value[fieldMap["HighPriority"].Index]),
                WellSwpz = GuardNull(row.Value[fieldMap["WellSWPZ"].Index]),
                LocationAccuracy = GuardNull(row.Value[fieldMap["LocationAccuracy"].Index]),
                LocationMethod = GuardNull(row.Value[fieldMap["LocationAccuracy"].Index]),
                WellName = GuardNull(row.Value[fieldMap["WellName"].Index]),
                WellSubClass = (int)row.Value[fieldMap["WellSubClass"].Index]
            };

            return model;
        }

        public static VerticalWellEventSdeModel MapVerticalWellEventModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new VerticalWellEventSdeModel
            {
                EventType = (int)row.Value[fieldMap["EventType"].Index]
            };

            return model;
        }

        public static WellStatusSdeModel MapWellStatusModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellStatusSdeModel
            {
                OperatingStatusDate = (DateTime)row.Value[fieldMap["OperatingStatusDate"].Index],
                OperatingStatusType = GuardNull(row.Value[fieldMap["OperatingStatusType"].Index]),
                WellGuid = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static WellInspectionSdeModel MapWellInspectionModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellInspectionSdeModel
            {
                InspectionAssistance = GuardNull(row.Value[fieldMap["InspectionAssistance"].Index]),
                InspectionDeficiency = GuardNull(row.Value[fieldMap["InspectionDeficiency"].Index]),
                InspectionDate = (DateTime)row.Value[fieldMap["InspectionDate"].Index],
                IcisCompMonActReason = GuardNull(row.Value[fieldMap["ICISCompMonActReason"].Index]),
                IcisCompMonType = GuardNull(row.Value[fieldMap["ICISCompMonType"].Index]),
                IcisCompActType = GuardNull(row.Value[fieldMap["ICISCompActType"].Index]),
                IcisMoaPriority = GuardNull(row.Value[fieldMap["ICISMOAPriority"].Index]),
                IcisRegionalPriority = GuardNull(row.Value[fieldMap["ICISRegionalPriority"].Index]),
                InspectionType = GuardNull(row.Value[fieldMap["InspectionType"].Index]),
                WellFk = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static CorrectionSdeModel MapCorrectionSdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new CorrectionSdeModel
            {
                CorrectiveAction = GuardNull(row.Value[fieldMap["CorrectiveAction"].Index]),
                Comments = GuardNull(row.Value[fieldMap["Comments"].Index]),
                Guid = new Guid((string) row.Value[fieldMap["GUID"].Index])
            };

            return model;
        }

        public static MiTestSdeModel MapMiTestSdeModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new MiTestSdeModel
            {
                MitDate = (DateTime)row.Value[fieldMap["MITDate"].Index],
                MitResult = GuardNull(row.Value[fieldMap["MITResult"].Index]),
                MitType = GuardNull(row.Value[fieldMap["MITType"].Index]),
                MitRemActDate = (DateTime)row.Value[fieldMap["MITRemActDate"].Index],
                MitRemediationAction = GuardNull(row.Value[fieldMap["MITRemediationAction"].Index]),
                WellFk = new Guid((string) row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static WellOperatingSdeModel MapWellOperationSdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellOperatingSdeModel
            {
                MaxInjectionRate = double.Parse((string) row.Value[fieldMap["MaxInjectionRate"].Index]),
                OnSiteVolume = double.Parse((string)row.Value[fieldMap["OnSiteVolume"].Index]),
                OffSiteVolume = double.Parse((string)row.Value[fieldMap["OffSiteVolume"].Index]),
                WellFk = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static WasteClassISdeModel MapWasteClassISdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WasteClassISdeModel
            {
                WasteCode = GuardNull(row.Value[fieldMap["WasteCode"].Index]),
                WasteStream = GuardNull(row.Value[fieldMap["WasteStream"].Index]),
                WellFk = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static ConstituentClassISdeModel MapConsituentClassISdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new ConstituentClassISdeModel
            {
                Concentration = Convert.ToInt32(row.Value[fieldMap["Concentration"].Index]),
                Unit = Convert.ToInt32(row.Value[fieldMap["Unit"].Index]),
                ConstituentCode = GuardNull(row.Value[fieldMap["ConstituentCode"].Index])
            };

            return model;
        }
        
        private static string GuardNull(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return (string) value;
        }

        private static string GetDomainValue(IObject row, IndexFieldMap fieldMap)
        {
            var value = GuardNull(row.Value[fieldMap.Index]);

            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            if (string.IsNullOrEmpty(fieldMap.DomainName))
            {
                return value;
            }

            if (!Cache.DomainDicionary.ContainsKey(fieldMap.DomainName) ||
                Cache.DomainDicionary[fieldMap.DomainName] == null)
            {
                return value;
            }

            try
            {
                if (!((IDomain)Cache.DomainDicionary[fieldMap.DomainName]).MemberOf(value))
                {
                    return value;
                }
            }
            catch (RuntimeBinderException)
            {
                
            }

            for (var values = 0; values < Cache.DomainDicionary[fieldMap.DomainName].CodeCount; values++)
            {
                if (value != Cache.DomainDicionary[fieldMap.DomainName].Name[values])
                {
                    continue;
                }

                return value;
            }

            return value;
        }
    }
}