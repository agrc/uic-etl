using System;
using System.Collections.Generic;
using AutoMapper;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using ESRI.ArcGIS.Geodatabase;
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
            });

            return config.CreateMapper();
        }

        public static FacilitySdeModel MapFacilityModel(IFeature row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new FacilitySdeModel
            {
                Guid = new Guid((string) row.Value[fieldMap["GUID"].Index]),
                FacilityName = GuardNull(row.Value[fieldMap["FacilityName"].Index]),
                FacilityAddress = GuardNull(row.Value[fieldMap["FacilityAddress"].Index]),
                FacilityCity = GuardNull(row.Value[fieldMap["FacilityCity"].Index]),
                FacilityId = GuardNull(row.Value[fieldMap["FacilityID"].Index]),
                FacilityZip = GuardNull(row.Value[fieldMap["FacilityZip"].Index]),
                FacilityType = GuardNull(row.Value[fieldMap["FacilityType"].Index]), 
                CountyFips = (int)row.Value[fieldMap["CountyFIPS"].Index], 
                NoMigrationPetStatus = GuardNull(row.Value[fieldMap["NoMigrationPetStatus"].Index])
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

        private static string GuardNull(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return (string) value;
        }
    }
}