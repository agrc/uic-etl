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
        public static IMapper CreateMappings(IReadOnlyDictionary<string, IndexFieldMap> facilityFieldMap)
        {
            var config = new MapperConfiguration(_ =>
            {
                _.CreateMap<UicFacilityModel, FacilityDetailModel>()
                    .ForMember(dest => dest.FacilityIdentifier, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.FacilityPetitionStatusCode,
                        opts => opts.MapFrom(src => src.NoMigrationPetStatus))
                    .ForMember(dest => dest.FacilitySiteName, opts => opts.MapFrom(src => src.FacilityName))
                    .ForMember(dest => dest.FacilitySiteTypeCode, opts => opts.MapFrom(src => src.FacilityType))
                    .ForMember(dest => dest.FacilityStateIdentifier, opts => opts.MapFrom(src => src.FacilityState))
                    .ForMember(dest => dest.LocalityName, opts => opts.Ignore())
                    .ForMember(dest => dest.FacilityViolationDetails, opts => opts.Ignore())
                    .ForMember(dest => dest.LocationAddressPostalCode, opts => opts.MapFrom(src => src.FacilityZip))
                    .ForMember(dest => dest.LocationAddressStateCode, opts => opts.MapFrom(src => src.FacilityState))
                    .ForMember(dest => dest.LocationAddressText, opts => opts.MapFrom(src => src.FacilityAddress));

                _.CreateMap<IFeature, UicFacilityModel>()
                    .ForMember(dest => dest.Guid,
                        opts => opts.ResolveUsing(g => new Guid(g.get_Value(facilityFieldMap["GUID"].Index))))
                    .ForMember(dest => dest.FacilityId,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityID"].Index)))
                    .ForMember(dest => dest.FacilityName,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityName"].Index)))
                    .ForMember(dest => dest.FacilityAddress,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityAddress"].Index)))
                    .ForMember(dest => dest.FacilityCity,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityCity"].Index)))
                    .ForMember(dest => dest.FacilityState,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityState"].Index)))
                    .ForMember(dest => dest.FacilityZip,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityZip"].Index)))
                    .ForMember(dest => dest.FacilityType,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["FacilityType"].Index)))
                    .ForMember(dest => dest.NoMigrationPetStatus,
                        opts => opts.MapFrom(src => src.get_Value(facilityFieldMap["NoMigrationPetStatus"].Index)));
            });

            return config.CreateMapper();
        }
    }
}