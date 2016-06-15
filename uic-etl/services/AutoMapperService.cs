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
                FacilityType = GuardNull(row.Value[fieldMap["FacilityType"].Index]), //need to handle db null type
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