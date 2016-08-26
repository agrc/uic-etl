﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using uic_etl.commands;
using uic_etl.models;
using uic_etl.models.dtos;
using uic_etl.services;
using FluentValidation;

namespace uic_etl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EtlOptions options;

            try
            {
                options = ArgParserService.Parse(args);
            }
            catch (InvalidOperationException e)
            {
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);
                Console.ReadKey();

                return;
            }

            var debug = new DebugService(options.Verbose);
            var start = Stopwatch.StartNew();
            debug.Write("Staring: {0}", DateTime.Now.ToString("s"));

            using (var releaser = new ComReleaser())
            {
                IWorkspace workspace;
                try
                {
                    debug.Write("{1} Connecting to: {0}", options.SdeConnectionPath, start.Elapsed);

                    workspace = WorkspaceService.GetSdeWorkspace(options.SdeConnectionPath);
                    releaser.ManageLifetime(workspace);
                }
                catch (COMException e)
                {
                    Console.Write("uic-etl: ");
                    Console.WriteLine(e.Message);

                    Console.ReadKey();
                    return;
                }

                var featureWorkspace = (IFeatureWorkspace) workspace;
                releaser.ManageLifetime(featureWorkspace);

                debug.Write("{0} Opening feature classes", start.Elapsed);
                var uicFacility = featureWorkspace.OpenFeatureClass("UICFacility");
                releaser.ManageLifetime(uicFacility);

                debug.Write("{0} Opening relationship classes", start.Elapsed);
                var facilityViolationRelation = featureWorkspace.OpenRelationshipClass("FacilityToViolation");
                releaser.ManageLifetime(facilityViolationRelation);

                var wellViolationRelation = featureWorkspace.OpenRelationshipClass("WellToViolation");
                releaser.ManageLifetime(wellViolationRelation);

                var responseRelation = featureWorkspace.OpenRelationshipClass("UICViolationToEnforcement");
                releaser.ManageLifetime(responseRelation);

                var wellRelation = featureWorkspace.OpenRelationshipClass("FacilityToWell");
                releaser.ManageLifetime(wellRelation);

                var verticalWellRelation = featureWorkspace.OpenRelationshipClass("WellToVerticalWellEvent");
                releaser.ManageLifetime(verticalWellRelation);

                var wellStatusRelation = featureWorkspace.OpenRelationshipClass("WellToWellOperatingStatus");
                releaser.ManageLifetime(wellStatusRelation);

                var wellInspectionRelation = featureWorkspace.OpenRelationshipClass("WellToInspection");
                releaser.ManageLifetime(wellInspectionRelation);

                var wellIntegrityRelation = featureWorkspace.OpenRelationshipClass("WellToIntegrityTest");
                releaser.ManageLifetime(wellIntegrityRelation);

                var deepWellRelation = featureWorkspace.OpenRelationshipClass("WellToDeepWellOperation");
                releaser.ManageLifetime(deepWellRelation);

                var wasteRelation = featureWorkspace.OpenRelationshipClass("WellToClassIWaste");
                releaser.ManageLifetime(wasteRelation);

                var authorizationActionRelation = featureWorkspace.OpenRelationshipClass("AuthorizationToAuthorizationAction");
                releaser.ManageLifetime(authorizationActionRelation);

                var areaOfReviewRelation = featureWorkspace.OpenRelationshipClass("AuthorizationToAreaOfReview");
                releaser.ManageLifetime(areaOfReviewRelation);

                var facilityToContactRelation = featureWorkspace.OpenRelationshipClass("UICFacilityToContact");
                releaser.ManageLifetime(facilityToContactRelation);

                debug.Write("{0} Opening tables", start.Elapsed);
                var uicContact = featureWorkspace.OpenTable("UICContact");
                releaser.ManageLifetime(uicContact);

                var uicAuthorization = featureWorkspace.OpenTable("UICAuthorization");
                releaser.ManageLifetime(uicAuthorization);

                debug.Write("{0} Creating field mappings", start.Elapsed);
                var facilityFieldMap = new FindIndexByFieldNameCommand(uicFacility, FacilitySdeModel.Fields).Execute();
                var violationFieldMap = new FindIndexByFieldNameCommand(facilityViolationRelation, ViolationSdeModel.Fields).Execute();
                var responseFieldMap = new FindIndexByFieldNameCommand(responseRelation, EnforcementSdeModel.Fields).Execute();
                var wellFieldMap = new FindIndexByFieldNameCommand(wellRelation, WellSdeModel.Fields).Execute();
                var verticalWellFieldMap = new FindIndexByFieldNameCommand(verticalWellRelation, VerticalWellEventSdeModel.Fields).Execute();
                var wellStatusFieldMap = new FindIndexByFieldNameCommand(wellStatusRelation, WellStatusSdeModel.Fields).Execute();
                var wellInspectionFieldMap = new FindIndexByFieldNameCommand(wellInspectionRelation, WellInspectionSdeModel.Fields).Execute();
                var mechanicalInspectionFieldMap = new FindIndexByFieldNameCommand(wellIntegrityRelation, MiTestSdeModel.Fields).Execute();
                var deepWellFieldMap = new FindIndexByFieldNameCommand(deepWellRelation, WellOperatingSdeModel.Fields).Execute();
                var wasteFieldMap = new FindIndexByFieldNameCommand(wasteRelation, WasteClassISdeModel.Fields).Execute();
                var contactFieldMap = new FindIndexByFieldNameCommand(uicContact, ContactSdeModel.Fields).Execute();
                var authorizationFieldMap = new FindIndexByFieldNameCommand(uicAuthorization, AuthorizationSdeModel.Fields).Execute();
                var authorizationActionFieldMap = new FindIndexByFieldNameCommand(authorizationActionRelation, AuthorizationActionSdeModel.Fields).Execute();
                var areaOfReviewFieldMap = new FindIndexByFieldNameCommand(areaOfReviewRelation, AreaOfReviewSdeModel.Fields).Execute();

                var srFactory = new SpatialReferenceEnvironment();
                var newSpatialRefefence = srFactory.CreateGeographicCoordinateSystem(4326);

                debug.Write("{0} Creating mappings for domain models", start.Elapsed);
                var mapper = EtlMappingService.CreateMappings();

                debug.Write("{0} Creating model validators", start.Elapsed);
                var engineeringValidator = new EngineeringDetailValidator();
                var facilityValidator = new FacilityDetailValidator();
                var permitValidator = new PermitDetailValidator();
                var wasteValidator = new WasteDetailValidator();
                var wellValidator = new WellDetailValidator();
                var wellInspectionValidator = new WellInspectionDetailValidator();
                var validator = new ValidatingService();

                debug.Write("{0} Creating XML document object.", start.Elapsed);
                var doc = XmlService.CreateDocument();

                var headerModel = new HeaderInformation
                {
                    Title = "data submission for quarter #1, fy 2010",
                    CreationTime = DateTime.Now.ToString("s"),
                    Comments = "This is a sample"
                };

                debug.Write("{1} Creating header property for: {0}", headerModel.Title, start.Elapsed);
                XmlService.AppendHeader(ref doc, headerModel);

                debug.Write("{0} Creating payload elements", start.Elapsed);
                var payload = XmlService.CreatePayloadElements();

                debug.Write("{0} Quering UICFacility features.", start.Elapsed);
                var queryFilter = new QueryFilter
                {
                    WhereClause = "1=1",
//                    WhereClause = "Guid='{268BB302-89F2-4BAA-A19B-45B3C207F236}'",
                    SubFields = string.Join(",", FacilitySdeModel.Fields)
                };
                releaser.ManageLifetime(queryFilter);

                var facilityCursor = uicFacility.Search(queryFilter, true);
                releaser.ManageLifetime(facilityCursor);

                var facilityCount = 0;

                // Loop over UICFacility
                IFeature facilityFeature;
                while ((facilityFeature = facilityCursor.NextFeature()) != null)
                {
                    debug.Write("{0} Facilities processed {1}", start.Elapsed, facilityCount++);
#if DEBUG
                    if (facilityCount > 25)
                    {
                        break;
                    }
#endif
                    releaser.ManageLifetime(facilityFeature);

                    var facility = EtlMappingService.MapFacilityModel(facilityFeature, facilityFieldMap);
                    var xmlFacility = mapper.Map<FacilitySdeModel, FacilityDetail>(facility);

                    var violationCursor = facilityViolationRelation.GetObjectsRelatedToObject(facilityFeature);
                    releaser.ManageLifetime(violationCursor);

                    // Find all UICViolations
                    IObject violationFeature;
                    while ((violationFeature = violationCursor.Next()) != null)
                    {
                        releaser.ManageLifetime(violationFeature);

                        var violation = EtlMappingService.MapViolationModel(violationFeature, violationFieldMap);
                        var xmlViolation = mapper.Map<ViolationSdeModel, ViolationDetail>(violation);
                        xmlViolation.ViolationFacilityIdentifier = xmlFacility.FacilityIdentifier;

                        var facilityResponseDetailCursor = responseRelation.GetObjectsRelatedToObject(violationFeature);
                        releaser.ManageLifetime(facilityResponseDetailCursor);

                        // Find all Violation Responses which are UICEnforcements
                        IObject responseFeature;
                        while ((responseFeature = facilityResponseDetailCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(responseFeature);

                            var responseDetail = EtlMappingService.MapResponseModel(responseFeature, responseFieldMap);
                            var xmlResponseDetail = mapper.Map<EnforcementSdeModel, ResponseDetail>(responseDetail);

                            xmlResponseDetail.ResponseViolationIdentifier = xmlViolation.ViolationIdentifier;

                            xmlViolation.ResponseDetail.Add(xmlResponseDetail);
                        }

                        xmlFacility.FacilityViolationDetail.Add(xmlViolation);
                    }

                    var facilityDetailElement = XmlService.AddFacility(ref payload, xmlFacility);

                    var wellCursor = wellRelation.GetObjectsRelatedToObject(facilityFeature);
                    releaser.ManageLifetime(wellCursor);

                    debug.Write("{1} finding wells for facility: {0}", facility.Guid, start.Elapsed);

                    var wellCount = 1;
                    // Find all wells
                    IFeature wellFeature;
                    while ((wellFeature = wellCursor.Next()) != null)
                    {
                        releaser.ManageLifetime(wellFeature);
                        debug.Write("{0} Wells processed {1}", start.Elapsed, wellCount++);

                        var well = EtlMappingService.MapWellModel(wellFeature, wellFieldMap);
                        var xmlWell = mapper.Map<WellSdeModel, WellDetail>(well);
                        xmlWell.WellSiteAreaNameText = xmlFacility.FacilitySiteName;

                        var facilityContactCursor = facilityToContactRelation.GetObjectsRelatedToObject(facilityFeature);
                        releaser.ManageLifetime(facilityContactCursor);

                        var mostImportantContact = 1000; // this is the 1 higher than 999 or other contact type
                        var mostImportantContactGuid = Guid.Empty;

                        IRow facilityContactFeature;
                        while ((facilityContactFeature = facilityContactCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(facilityContactFeature);

                            var contact = EtlMappingService.MapContactSdeModel(facilityContactFeature, contactFieldMap);

                            if (contact.ContactType > mostImportantContact)
                            {
                                continue;
                            }

                            mostImportantContact = contact.ContactType;
                            mostImportantContactGuid = contact.Guid;
                        }

                        xmlWell.WellContactIdentifier = new GenerateIdentifierCommand(mostImportantContactGuid).Execute();

                        var verticalWellCursor = verticalWellRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(verticalWellCursor);

                        var verticalEventFeature = verticalWellCursor.Next();
                        var verticalEvent = EtlMappingService.MapVerticalWellEventModel(verticalEventFeature, verticalWellFieldMap);

                        xmlWell.WellTotalDepthNumeric = string.IsNullOrEmpty(verticalEvent.Length) ? "0" : verticalEvent.Length;

                        var wellStatusCursor = wellStatusRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(wellStatusCursor);

                        var wellTypeDate = DateTime.MaxValue;
                        // write well status
                        IObject wellStatusFeature;
                        while ((wellStatusFeature = wellStatusCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(wellStatusFeature);

                            var wellStatus = EtlMappingService.MapWellStatusModel(wellStatusFeature, wellStatusFieldMap);
                            var xmlWellStatus = mapper.Map<WellStatusSdeModel, WellStatusDetail>(wellStatus);

                            // get the earliest date
                            if (wellStatus.OperatingStatusDate.HasValue && wellTypeDate > wellStatus.OperatingStatusDate)
                            {
                                wellTypeDate = wellStatus.OperatingStatusDate.Value;
                            }

                            xmlWell.WellStatusDetail.Add(xmlWellStatus);
                        }

                        var wellTypeDateFormatted = wellTypeDate.ToString("yyyyyMMdd");

                        if (wellTypeDate == DateTime.MaxValue)
                        {
                            wellTypeDateFormatted = null;
                        }

                        // Add Well Type Details
                        var wellTypeDetail = new WellTypeDetail
                        {
                            WellTypeIdentifier = xmlWell.WellIdentifier,
                            WellTypeCode = xmlWell.WellTypeCode,
                            WellTypeDate = wellTypeDateFormatted,
                            WellTypeWellIdentifier = new GenerateIdentifierCommand(well.Guid).Execute()
                        };

                        xmlWell.WellTypeDetail.Add(wellTypeDetail);

                        // location detail
                        var utm = wellFeature.ShapeCopy;

                        utm.Project(newSpatialRefefence);
                        var point = (IPoint) utm;

                        var locationDetail = new LocationDetail(well, facility, point.X, point.Y, guid => new GenerateIdentifierCommand(guid).Execute());

                        if (validator.IsValid(locationDetail))
                        {
                            xmlWell.LocationDetail = locationDetail;
                        }

                        // well violation detail uses same table and relationship as facility violations
                        var wellViolationCursor = wellViolationRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(wellViolationCursor);

                        IObject wellViolationFeature;
                        while ((wellViolationFeature = wellViolationCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(wellViolationFeature);

                            var violation = EtlMappingService.MapViolationModel(wellViolationFeature, violationFieldMap);
                            var xmlViolation = mapper.Map<ViolationSdeModel, ViolationDetail>(violation);

                            var wellResponseDetailCursor = responseRelation.GetObjectsRelatedToObject(wellViolationFeature);
                            releaser.ManageLifetime(wellResponseDetailCursor);

                            IObject responseFeature;
                            while ((responseFeature = wellResponseDetailCursor.Next()) != null)
                            {
                                releaser.ManageLifetime(responseFeature);

                                var responseDetail = EtlMappingService.MapResponseModel(responseFeature, responseFieldMap);
                                var xmlResponseDetail = mapper.Map<EnforcementSdeModel, ResponseDetail>(responseDetail);

                                xmlResponseDetail.ResponseViolationIdentifier = xmlViolation.ViolationIdentifier;

                                if (validator.IsValid(xmlResponseDetail))
                                {
                                    xmlViolation.ResponseDetail.Add(xmlResponseDetail);
                                }
                            }

                            if (validator.IsValid(xmlViolation))
                            {
                                xmlWell.WellViolationDetail.Add(xmlViolation);
                            }
                        }

                        // well inspection detail
                        var wellInspectionCursor = wellInspectionRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(wellInspectionRelation);

                        IObject wellInspectionFeature;
                        while ((wellInspectionFeature = wellInspectionCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(wellInspectionFeature);

                            var wellInspection = EtlMappingService.MapWellInspectionModel(wellInspectionFeature, wellInspectionFieldMap);
                            var xmlWellInspection = mapper.Map<WellInspectionSdeModel, WellInspectionDetail>(wellInspection);

                            xmlWell.WellInspectionDetail.Add(xmlWellInspection);
                        }

                        // MI Test detail
                        var mechanicalIntegrityCursor = wellIntegrityRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(mechanicalIntegrityCursor);

                        IObject mechanicalIntegrityFeature;
                        while ((mechanicalIntegrityFeature = mechanicalIntegrityCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(mechanicalIntegrityFeature);

                            var mit = EtlMappingService.MapMiTestSdeModel(mechanicalIntegrityFeature, mechanicalInspectionFieldMap);
                            var xmlMit = mapper.Map<MiTestSdeModel, MiTestDetail>(mit);

                            if (validator.IsValid(xmlMit))
                            {
                                xmlWell.MitTestDetail.Add(xmlMit);                                
                            }
                        }

                        // Engineering Detail
                        var deepWellCursor = deepWellRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(deepWellCursor);

                        IObject deepWellFeature;
                        while ((deepWellFeature = deepWellCursor.Next()) != null)
                        {
                            releaser.ManageLifetime(deepWellFeature);

                            var deepWell = EtlMappingService.MapWellOperationSdeModel(deepWellFeature, deepWellFieldMap);
                            var engineeringDetail = mapper.Map<WellOperatingSdeModel, EngineeringDetail>(deepWell);

                            xmlWell.EngineeringDetail.Add(engineeringDetail);
                        }

                        // Waste Detail
                        var wasteCurser = wasteRelation.GetObjectsRelatedToObject(wellFeature);
                        releaser.ManageLifetime(wasteCurser);

                        IObject wasteFeature;
                        while ((wasteFeature = wasteCurser.Next()) != null)
                        {
                            releaser.ManageLifetime(wasteFeature);

                            var waste = EtlMappingService.MapWasteClassISdeModel(wasteFeature, wasteFieldMap);
                            var xmlWaste = mapper.Map<WasteClassISdeModel, WasteDetail>(waste);

                            var result = wasteValidator.Validate(xmlWaste, ruleSet: "R1");

                            if (!result.IsValid)
                            {
                                ErrorReportingService.LogErrors(result.Errors, "R1");

                                continue;
                            }

                            xmlWell.WasteDetail.Add(xmlWaste);
                        }

                        XmlService.AddWell(ref facilityDetailElement, xmlWell);
                    }
                }

                debug.Write("{0} finding all contacts", start.Elapsed);
                queryFilter.SubFields = string.Join(",", ContactSdeModel.Fields);

                var contactCursor = uicContact.Search(queryFilter, true);
                releaser.ManageLifetime(contactCursor);

                var contacts = new List<ContactDetail>();

                IRow contactFeature;
                while ((contactFeature = contactCursor.NextRow()) != null)
                {
                    releaser.ManageLifetime(contactFeature);

                    var contact = EtlMappingService.MapContactSdeModel(contactFeature, contactFieldMap);
                    var xmlContact = mapper.Map<ContactSdeModel, ContactDetail>(contact);

                    contacts.Add(xmlContact);
                }

                queryFilter.SubFields = string.Join(",", AuthorizationSdeModel.Fields);

                var authorizationCursor = uicAuthorization.Search(queryFilter, true);
                releaser.ManageLifetime(authorizationCursor);

                var permits = new List<PermitDetail>();

                debug.Write("{0} finding all permits", start.Elapsed);
                IRow authorizeFeature;
                while ((authorizeFeature = authorizationCursor.NextRow()) != null)
                {
                    releaser.ManageLifetime(authorizeFeature);

                    var authorize = EtlMappingService.MapAuthorizationSdeModel(authorizeFeature, authorizationFieldMap);
                    var xmlPermit = mapper.Map<AuthorizationSdeModel, PermitDetail>(authorize);

                    var authorizationActionCursor = authorizationActionRelation.GetObjectsRelatedToObject((IObject) authorizeFeature);
                    releaser.ManageLifetime(authorizationActionCursor);

                    IObject authorizationActionFeature;
                    while ((authorizationActionFeature = authorizationActionCursor.Next()) != null)
                    {
                        var authorizationAction = EtlMappingService.MapAuthorizationActionSdeModel(authorizationActionFeature, authorizationActionFieldMap);
                        var permitActivityDetail = mapper.Map<AuthorizationActionSdeModel, PermitActivityDetail>(authorizationAction);

                        if (string.IsNullOrEmpty(permitActivityDetail.PermitActivityActionTypeCode) ||
                            permitActivityDetail.PermitActivityActionTypeCode.ToUpper() == "NR")
                        {
                            // skip authorized by rule wells.
                            // https://github.com/agrc/uic-etl/issues/10#issuecomment-241120723
                            continue;
                        }

                        xmlPermit.PermitActivityDetail.Add(permitActivityDetail);
                    }

                    var areaOfReviewCursor = areaOfReviewRelation.GetObjectsRelatedToObject((IObject) authorizeFeature);
                    releaser.ManageLifetime(areaOfReviewCursor);

                    var areaOfReviewFeature = areaOfReviewCursor.Next();
                    if (areaOfReviewFeature == null)
                    {
                        permits.Add(xmlPermit);
                        continue;
                    }

                    var areaOfReview = EtlMappingService.MapAreaOfReviewSdeModel(areaOfReviewFeature, areaOfReviewFieldMap);
                    xmlPermit.PermitAorWellNumberNumeric = areaOfReview.PermitAorWellNumberNumeric;

                    permits.Add(xmlPermit);
                }

                XmlService.AppendPayloadElements(ref payload, contacts, permits);

                doc.Root.Add(payload);

                using (var w = new XmlTextWriter(string.Format("UTEQ-{0}.xml", DateTime.Now.ToShortDateString().Replace('/', '-')), new UTF8Encoding(false)))
                {
                    w.Formatting = Formatting.Indented;
                    doc.Save(w);
                }
            }

            debug.Write("{0} finished.", start.Elapsed);

            Console.ReadKey();
        }
    }
}