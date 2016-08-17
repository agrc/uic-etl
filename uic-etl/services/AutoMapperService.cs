using System;
using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using ESRI.ArcGIS.Geodatabase;
using Microsoft.CSharp.RuntimeBinder;
using uic_etl.commands;
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
                _.CreateMap<FacilitySdeModel, FacilityDetail>()
                    .ForMember(dest => dest.FacilityIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.Guid, opts => opts.MapFrom(src => src.Guid))
                    .ForMember(dest => dest.LocalityName, opts => opts.MapFrom(src => src.FacilityCity))
                    .ForMember(dest => dest.FacilityPetitionStatusCode,
                        opts => opts.MapFrom(src => src.NoMigrationPetStatus))
                    .ForMember(dest => dest.FacilitySiteName, opts => opts.MapFrom(src => src.FacilityName))
                    .ForMember(dest => dest.FacilitySiteTypeCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.FacilityType) ? "U" : src.FacilityType))
                    .ForMember(dest => dest.FacilityStateIdentifier, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.FacilityViolationDetail, opts => opts.Ignore())
                    .ForMember(dest => dest.LocationAddressPostalCode, opts => opts.MapFrom(src => src.FacilityZip))
                    .ForMember(dest => dest.LocationAddressStateCode, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.LocationAddressText, opts => opts.MapFrom(src => src.FacilityAddress));

                _.CreateMap<ViolationSdeModel, ViolationDetail>()
                    .ForMember(dest => dest.ViolationIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.ViolationContaminationCode, opts => opts.MapFrom(src => src.UsdwContamination))
                    .ForMember(dest => dest.ViolationEndangeringCode, opts => opts.MapFrom(src => src.Endanger))
                    .ForMember(dest => dest.ViolationReturnComplianceDate, opts => opts.MapFrom(src => src.ReturnToComplianceDate.HasValue ? src.ReturnToComplianceDate.Value.ToString("yyyyMMdd") : DateTime.MinValue.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.ViolationSignificantCode, opts => opts.MapFrom(src => src.SignificantNonCompliance))
                    .ForMember(dest => dest.ViolationDeterminedDate, opts => opts.MapFrom(src => src.ViolationDate.HasValue ? src.ViolationDate.Value.ToString("yyyyMMdd") : DateTime.MinValue.ToString("yyyyyMMdd")))
                    .ForMember(dest => dest.ViolationTypeCode, opts => opts.MapFrom(src => src.ViolationType))
                    .ForMember(dest => dest.ViolationFacilityIdentifier, opts => opts.MapFrom(src => src.FacilityId))
                    .ForMember(dest => dest.Guid, opts => opts.MapFrom(src => src.Guid))
                    .ForMember(dest => dest.ViolationWellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WellId).Execute()))
                    .ForMember(dest => dest.ResponseDetail, opts => opts.Ignore());

                _.CreateMap<EnforcementSdeModel, ResponseDetail>()
                    .ForMember(dest => dest.ResponseViolationIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.ResponseEnforcementIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()));

                _.CreateMap<WellSdeModel, WellDetail>()
                    .ForMember(dest => dest.WellTotalDepthNumeric, opts => opts.Ignore())
                    .ForMember(dest => dest.WellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.WellAquiferExemptionInjectionCode, opts => opts.MapFrom(src => src.InjectionAquiferExempt))
                    .ForMember(dest => dest.WellHighPriorityDesignationCode, opts => opts.MapFrom(src => src.HighPriority))
                    .ForMember(dest => dest.WellContactIdentifier, opts => opts.Ignore())
                    .ForMember(dest => dest.WellFacilityIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.FacilityGuid).Execute()))
                    .ForMember(dest => dest.WellGeologyIdentifier, opts => opts.MapFrom(__ => new GenerateIdentifierCommand(Guid.Empty).Execute()))
                    .ForMember(dest => dest.WellSiteAreaNameText, opts => opts.Ignore())
                    .ForMember(dest => dest.WellPermitIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.AuthorizationGuid).Execute()))
                    .ForMember(dest => dest.WellStateIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
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
                    .ForMember(dest => dest.WellStatusIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.WellStatusDate, opts => opts.MapFrom(src => src.OperatingStatusDate.HasValue ? src.OperatingStatusDate.Value.ToString("yyyyMMdd") : DateTime.MinValue.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.WellStatusWellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WellGuid).Execute()))
                    .ForMember(dest => dest.WellStatusOperatingStatusCode, opts => opts.MapFrom(src => src.OperatingStatusType));

                _.CreateMap<WellInspectionSdeModel, WellInspectionDetail>()
                    .ForMember(dest => dest.InspectionIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.InspectionAssistanceCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.InspectionAssistance) ? "U" : src.InspectionAssistance))
                    .ForMember(dest => dest.InspectionDeficiencyCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.InspectionDeficiency) ? "U" : src.InspectionDeficiency))
                    .ForMember(dest => dest.InspectionActionDate, opts => opts.MapFrom(src => src.InspectionDate.HasValue ? src.InspectionDate.Value.ToString("yyyyMMdd") : DateTime.MinValue.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.InspectionIcisComplianceMonitoringReasonCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.IcisCompMonActReason) ? "U" : src.IcisCompMonActReason))
                    .ForMember(dest => dest.InspectionIcisComplianceMonitoringTypeCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.IcisCompMonType) ? "U" : src.IcisCompMonType))
                    .ForMember(dest => dest.InspectionIcisComplianceActivityTypeCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.IcisCompActType) ? "U" : src.IcisCompActType))
                    .ForMember(dest => dest.InspectionIcisMoaName, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.IcisMoaPriority) ? "U" : src.IcisMoaPriority))
                    .ForMember(dest => dest.InspectionIcisRegionalPriorityName, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.IcisRegionalPriority) ? "U" : src.IcisRegionalPriority))
                    .ForMember(dest => dest.InspectionTypeActionCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.InspectionType) ? "U" : src.InspectionType))
                    .ForMember(dest => dest.InspectionWellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WellFk).Execute()))
                    .ForMember(dest => dest.CorrectionDetail, opts => opts.Ignore());

                _.CreateMap<CorrectionSdeModel, CorrectionDetail>()
                    .ForMember(dest => dest.CorrectionIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.CorrectiveActionTypeCode, opts => opts.MapFrom(src => src.CorrectiveAction))
                    .ForMember(dest => dest.CorrectionCommentText, opts => opts.MapFrom(src => src.Comments))
                    .ForMember(dest => dest.CorrectionInspectionIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()));

                _.CreateMap<MiTestSdeModel, MiTestDetail>()
                    .ForMember(dest => dest.MechanicalIntegrityTestIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.MechanicalIntegrityTestCompletedDate, opts => opts.MapFrom(src => src.MitDate.HasValue ? src.MitDate.Value.ToString("yyyyMMdd") : DateTime.MinValue.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.MechanicalIntegrityTestResultCode, opts => opts.MapFrom(src => src.MitResult))
                    .ForMember(dest => dest.MechanicalIntegrityTestTypeCode, opts => opts.MapFrom(src => src.MitType))
                    .ForMember(dest => dest.MechanicalIntegrityTestRemedialActionDate, opts => opts.MapFrom(src => src.MitRemActDate.HasValue ? src.MitRemActDate.Value.ToString("yyyyMMdd") : DateTime.MinValue.ToString("yyyyMMdd")))
                    .ForMember(dest => dest.MechanicalIntegrityTestRemedialActionTypeCode, opts => opts.MapFrom(src => string.IsNullOrEmpty(src.MitRemediationAction) ? "U" : src.MitRemediationAction))
                    .ForMember(dest => dest.MechanicalIntegrityTestWellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WellFk).Execute()));

                _.CreateMap<WellOperatingSdeModel, EngineeringDetail>()
                    .ForMember(dest => dest.EngineeringIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.EngineeringMaximumFlowRateNumeric, opts => opts.MapFrom(src => src.MaxInjectionRate))
                    .ForMember(dest => dest.EngineeringPermittedOnsiteInjectionVolumeNumeric, opts => opts.MapFrom(src => src.OnSiteVolume))
                    .ForMember(dest => dest.EngineeringPermittedOffsiteInjectionVolumeNumeric, opts => opts.MapFrom(src => src.OffSiteVolume))
                    .ForMember(dest => dest.EngineeringWellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WellFk).Execute()));

                _.CreateMap<WasteClassISdeModel, WasteDetail>()
                    .ForMember(dest => dest.WasteCode, opts => opts.MapFrom(src => src.WasteCode))
                    .ForMember(dest => dest.WasteStreamClassificationCode, opts => opts.MapFrom(src => src.WasteStream))
                    .ForMember(dest => dest.WasteWellIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WellFk).Execute()))
                    .ForMember(dest => dest.WasteIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.ConstituentDetail, opts => opts.Ignore());

                _.CreateMap<ConstituentClassISdeModel, ConstituentDetail>()
                    .ForMember(dest => dest.MeasureValue, opts => opts.MapFrom(src => src.Concentration))
                    .ForMember(dest => dest.MeasureUnitCode, opts => opts.MapFrom(src => src.Unit))
                    .ForMember(dest => dest.ConstituentNameText, opts => opts.MapFrom(src => src.Constituent))
                    .ForMember(dest => dest.ConstituentWasteIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.WasteGuid).Execute()))
                    .ForMember(dest => dest.ConstituentIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()));

                _.CreateMap<ContactSdeModel, ContactDetail>()
                    .ForMember(dest => dest.TelephoneNumberText, opts => opts.MapFrom(src => src.ContactPhone))
                    .ForMember(dest => dest.IndividualFullName, opts => opts.MapFrom(src => src.ContactName))
                    .ForMember(dest => dest.ContactCityName, opts => opts.MapFrom(src => src.ContactMailCity))
                    .ForMember(dest => dest.ContactAddressStateCode, opts => opts.MapFrom(src => src.ContactMailState))
                    .ForMember(dest => dest.ContactAddressText, opts => opts.MapFrom(src => src.ContactMailAddress))
                    .ForMember(dest => dest.ContactAddressPostalCode, opts => opts.MapFrom(src => src.ZipCode5 + src.ZipCode4))
                    .ForMember(dest => dest.ContactIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()));

                _.CreateMap<AuthorizationSdeModel, PermitDetail>()
                    .ForMember(dest => dest.PermitAuthorizedStatusCode, opts => opts.MapFrom(src => src.AuthorizeType))
                    .ForMember(dest => dest.PermitOwnershipTypeCode, opts => opts.MapFrom(src => src.OwnerSectorType))
                    .ForMember(dest => dest.PermitAuthorizedIdentifier, opts => opts.MapFrom(src => src.AuthorizeNumber))
                    .ForMember(dest => dest.PermitAorWellNumberNumeric, opts => opts.Ignore())
                    .ForMember(dest => dest.PermitIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.PermitActivityDetail, opts => opts.Ignore());

                _.CreateMap<AuthorizationActionSdeModel, PermitActivityDetail>()
                    .ForMember(dest => dest.PermitActivityActionTypeCode, opts => opts.MapFrom(src => src.AuthorizeActionType))
                    .ForMember(dest => dest.PermitActivityDate, opts => opts.MapFrom(src => src.AuthorizeActionDate))
                    .ForMember(dest => dest.PermitActivityPermitIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()))
                    .ForMember(dest => dest.PermitActivityIdentifier, opts => opts.MapFrom(src => new GenerateIdentifierCommand(src.Guid).Execute()));
            });

            return config.CreateMapper();
        }

        public static FacilitySdeModel MapFacilityModel(IFeature row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new FacilitySdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
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

        public static ViolationSdeModel MapViolationModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new ViolationSdeModel
            {
                UsdwContamination = GetDomainValue(row, fieldMap["USDWContamination"]),
                Endanger = GetDomainValue(row, fieldMap["ENDANGER"]),
                ReturnToComplianceDate = GetDateValue(row.Value[fieldMap["ReturnToComplianceDate"].Index]),
                SignificantNonCompliance = GetDomainValue(row, fieldMap["SignificantNonCompliance"]),
                ViolationDate = GetDateValue(row.Value[fieldMap["ViolationDate"].Index]),
                ViolationType = GetDomainValue(row, fieldMap["ViolationType"]),
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

        public static EnforcementSdeModel MapResponseModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new EnforcementSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["Guid"].Index])
            };

            return model;
        }

        public static WellSdeModel MapWellModel(IFeature row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                WellId = GetDomainValue(row, fieldMap["WellID"]),
                FacilityGuid = new Guid((string)row.Value[fieldMap["Facility_FK"].Index]),
                AuthorizationGuid = new Guid((string)row.Value[fieldMap["Authorization_FK"].Index]),
                InjectionAquiferExempt = GetDomainValue(row, fieldMap["InjectionAquiferExempt"]),
                HighPriority = GetDomainValue(row, fieldMap["HighPriority"]),
                WellSwpz = GetDomainValue(row, fieldMap["WellSWPZ"]),
                LocationAccuracy = GetDomainValue(row, fieldMap["LocationAccuracy"]),
                LocationMethod = GetDomainValue(row, fieldMap["LocationAccuracy"]),
                WellName = GetDomainValue(row, fieldMap["WellName"]),
                WellSubClass = GetDomainValue(row, fieldMap["WellSubClass"])
            };

            return model;
        }

        public static VerticalWellEventSdeModel MapVerticalWellEventModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new VerticalWellEventSdeModel
            {
                Length = GetDomainValue(row, fieldMap["Length"])
            };

            return model;
        }

        public static WellStatusSdeModel MapWellStatusModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellStatusSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                OperatingStatusDate = GetDateValue(row.Value[fieldMap["OperatingStatusDate"].Index]),
                OperatingStatusType = GetDomainValue(row, fieldMap["OperatingStatusType"]),
                WellGuid = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static WellInspectionSdeModel MapWellInspectionModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellInspectionSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                InspectionAssistance = GetDomainValue(row, fieldMap["InspectionAssistance"]),
                InspectionDeficiency = GetDomainValue(row, fieldMap["InspectionDeficiency"]),
                InspectionDate = GetDateValue(row.Value[fieldMap["InspectionDate"].Index]),
                IcisCompMonActReason = GetDomainValue(row, fieldMap["ICISCompMonActReason"]),
                IcisCompMonType = GetDomainValue(row, fieldMap["ICISCompMonType"]),
                IcisCompActType = GetDomainValue(row, fieldMap["ICISCompActType"]),
                IcisMoaPriority = GetDomainValue(row, fieldMap["ICISMOAPriority"]),
                IcisRegionalPriority = GetDomainValue(row, fieldMap["ICISRegionalPriority"]),
                InspectionType = GetDomainValue(row, fieldMap["InspectionType"]),
                WellFk = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static CorrectionSdeModel MapCorrectionSdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new CorrectionSdeModel
            {
                CorrectiveAction = GetDomainValue(row, fieldMap["CorrectiveAction"]),
                Comments = GetDomainValue(row, fieldMap["Comments"]),
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index])
            };

            return model;
        }

        public static MiTestSdeModel MapMiTestSdeModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new MiTestSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                MitDate = GetDateValue(row.Value[fieldMap["MITDate"].Index]),
                MitResult = GetDomainValue(row, fieldMap["MITResult"]),
                MitType = GetDomainValue(row, fieldMap["MITType"]),
                MitRemActDate = GetDateValue(row.Value[fieldMap["MITRemActDate"].Index]),
                MitRemediationAction = GetDomainValue(row, fieldMap["MITRemediationAction"]),
                WellFk = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static WellOperatingSdeModel MapWellOperationSdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new WellOperatingSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                MaxInjectionRate = double.Parse((string)row.Value[fieldMap["MaxInjectionRate"].Index]),
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
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                WasteCode = GetDomainValue(row, fieldMap["WasteCode"]),
                WasteStream = GetDomainValue(row, fieldMap["WasteStream"]),
                WellFk = new Guid((string)row.Value[fieldMap["Well_FK"].Index])
            };

            return model;
        }

        public static ConstituentClassISdeModel MapConsituentClassISdeModel(IObject row,
            IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new ConstituentClassISdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                Concentration = Convert.ToDouble(row.Value[fieldMap["Concentration"].Index]),
                Unit = GetDomainValue(row, fieldMap["Unit"]),
                Constituent = GetDomainValue(row, fieldMap["Constituent"])
            };

            return model;
        }

        public static ContactSdeModel MapContactSdeModel(IRow row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new ContactSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                ContactPhone = GetDomainValue(row, fieldMap["ContactPhone"]),
                ContactName = GetDomainValue(row, fieldMap["ContactName"]),
                ContactMailCity = GetDomainValue(row, fieldMap["ContactMailCity"]),
                ContactMailState = GetDomainValue(row, fieldMap["ContactMailState"]),
                ContactMailAddress = GetDomainValue(row, fieldMap["ContactMailAddress"]),
                ZipCode5 = GetDomainValue(row, fieldMap["ZipCode5"]),
                ZipCode4 = GetDomainValue(row, fieldMap["ZipCode4"])
            };

            return model;
        }

        public static AuthorizationSdeModel MapAuthorizationSdeModel(IRow row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new AuthorizationSdeModel
            {
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index]),
                AuthorizeType = GetDomainValue(row, fieldMap["AuthorizationType"]),
                OwnerSectorType = GetDomainValue(row, fieldMap["OwnerSectorType"]),
                AuthorizeNumber = GetDomainValue(row, fieldMap["AuthorizationID"])
            };

            return model;
        }

        public static AreaOfReviewSdeModel MapAreaOfReviewSdeModel(IRow row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var abandon = GuardNull(row.Value[fieldMap["CA_Abandon"].Index]);
            var repair = GuardNull(row.Value[fieldMap["CA_Repair"].Index]);
            var replug = GuardNull(row.Value[fieldMap["CA_Replug"].Index]);
            var other = GuardNull(row.Value[fieldMap["CA_Other"].Index]);

            var model = new AreaOfReviewSdeModel
            {
                CaAbandon = string.IsNullOrEmpty(abandon) ? 0 : Convert.ToDouble(abandon),
                CaRepair = string.IsNullOrEmpty(repair) ? 0 : Convert.ToDouble(repair),
                CaReplug = string.IsNullOrEmpty(replug)? 0 : Convert.ToDouble(replug),
                CaOther = string.IsNullOrEmpty(other) ? 0 : Convert.ToDouble(other)
            };

            return model;
        }

        public static AuthorizationActionSdeModel MapAuthorizationActionSdeModel(IObject row, IReadOnlyDictionary<string, IndexFieldMap> fieldMap)
        {
            var model = new AuthorizationActionSdeModel
            {
                AuthorizeActionType = GetDomainValue(row, fieldMap["AuthorizationActionType"]),
                AuthorizeActionDate = GetDateValue(row.Value[fieldMap["AuthorizationActionDate"].Index]),
                Guid = new Guid((string)row.Value[fieldMap["GUID"].Index])
            };

            return model;
        }

        private static string GuardNull(this object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        private static string GetDomainValue(IRow row, IndexFieldMap fieldMap)
        {
            return GetDomainValue((IObject)row, fieldMap);
        }

        private static string GetDomainValue(IObject row, IndexFieldMap fieldMap)
        {
            if (row == null)
            {
                return null;
            }

            string value = GuardNull(row.Value[fieldMap.Index]);

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

        private static DateTime? GetDateValue(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            return (DateTime)value;
        }
    }
}