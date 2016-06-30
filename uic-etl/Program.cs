using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Runtime.InteropServices;
using AutoMapper;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using ESRI.ArcGIS.ADF;
using ESRI.ArcGIS.Geodatabase;
using uic_etl.commands;
using uic_etl.models;
using uic_etl.models.dtos;
using uic_etl.services;

namespace uic_etl
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            EtlOptions options;
            IWorkspace workspace;
            var comObjects = new Stack<object>();

            try
            {
                options = ArgParserService.Parse(args);
            }
            catch (InvalidOperationException e)
            {
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);

                return;
            }

            var debug = new DebugService(options.Verbose);
            var start = Stopwatch.StartNew();
            debug.Write("Staring: {0}", DateTime.Now.ToString("s"));

            try
            {
                debug.Write("{1} Connecting to: {0}", options.SdeConnectionPath, start.Elapsed);

                workspace = WorkspaceService.GetSdeWorkspace(options.SdeConnectionPath);
                comObjects.Push(workspace);
            }
            catch (COMException e)
            {
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);

                return;
            }

            var featureWorkspace = (IFeatureWorkspace) workspace;
            comObjects.Push(featureWorkspace);

            debug.Write("{0} Opening UICFacility feature class", start.Elapsed);
            var uicFacility = featureWorkspace.OpenFeatureClass("UICFacility");
            comObjects.Push(uicFacility);

            debug.Write("Opening Facility to Violation Relationship Class.");
            var violationRelation = featureWorkspace.OpenRelationshipClass("FacilityToViolation");
            comObjects.Push(violationRelation);

            debug.Write("Opening Violation to Response Relationship Class.");
            var responseRelation = featureWorkspace.OpenRelationshipClass("UICViolationToEnforcement");
            comObjects.Push(responseRelation);

            debug.Write("Opening Facility to Well Relationship Class.");
            var wellRelation = featureWorkspace.OpenRelationshipClass("FacilityToWell");
            comObjects.Push(wellRelation);

            debug.Write("Opening Vertical Well Event to Well Relationship Class.");
            var verticalWellRelation = featureWorkspace.OpenRelationshipClass("WellToVerticalWellEvent");
            comObjects.Push(verticalWellRelation);

            debug.Write("Opening Facility to Well Relationship Class.");
            var wellStatusRelation = featureWorkspace.OpenRelationshipClass("WellToWellOperatingStatus");
            comObjects.Push(wellStatusRelation);
             
            debug.Write("{0} Creating field mappings", start.Elapsed);
            var facilityFieldMap = new FindIndexByFieldNameCommand(uicFacility, FacilitySdeModel.Fields).Execute();
            var violationFieldMap = new FindIndexByFieldNameCommand(violationRelation, FacilityViolationSdeModel.Fields).Execute();
            var responseFieldMap = new FindIndexByFieldNameCommand(responseRelation, FacilityEnforcementSdeModel.Fields).Execute();
            var wellFieldMap = new FindIndexByFieldNameCommand(wellRelation, WellSdeModel.Fields).Execute();
            var verticalWellFieldMap = new FindIndexByFieldNameCommand(verticalWellRelation, VerticalWellEventSdeModel.Fields).Execute();
            var wellStatusFieldMap = new FindIndexByFieldNameCommand(wellStatusRelation, WellStatusSdeModel.Fields).Execute();

            debug.Write("{0} Creating mappings for domain models", start.Elapsed);
            var mapper = AutoMapperService.CreateMappings();

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
//                WhereClause = "1=1"
                WhereClause = "Guid='{268BB302-89F2-4BAA-A19B-45B3C207F236}'"
            };
            comObjects.Push(queryFilter);

            var facilityCursor = uicFacility.Search(queryFilter, true);
            comObjects.Push(facilityCursor);

            // Loop over UICFacility
            var facilityId = 0;
            IFeature facilityFeature;
            while ((facilityFeature = facilityCursor.NextFeature()) != null)
            {
                var violationId = 0;
                var facility = AutoMapperService.MapFacilityModel(facilityFeature, facilityFieldMap);
                var xmlFacility = mapper.Map<FacilitySdeModel, FacilityDetailModel>(facility);
                xmlFacility.FacilityIdentifier = facilityId++;

                debug.Write("finding violations for facility oid: {0}", facilityFeature.OID);
                var violationCursor = violationRelation.GetObjectsRelatedToObject(facilityFeature);
                comObjects.Push(violationCursor);

                // Find all UICViolations
                IObject violationFeature;
                while((violationFeature = violationCursor.Next()) != null)
                {
                    var violation = AutoMapperService.MapViolationModel(violationFeature, violationFieldMap);
                    var xmlViolation = mapper.Map<FacilityViolationSdeModel, FacilityViolationDetail>(violation);
                    xmlViolation.ViolationIdentifier = violationId++;

                    debug.Write("finding violation responses for violation: {0}", violationFeature.OID);
                    var facilityResponseDetailCursor = responseRelation.GetObjectsRelatedToObject(violationFeature);
                    comObjects.Push(facilityResponseDetailCursor);

                    // Find all Violation Responses which are UICEnforcements
                    var enforcementId = 0;
                    IObject responseFeature;
                    while ((responseFeature = facilityResponseDetailCursor.Next()) != null)
                    {
                        var responseDetail = AutoMapperService.MapResponseModel(responseFeature, responseFieldMap);
                        var xmlResponseDetail = mapper.Map<FacilityEnforcementSdeModel, FacilityResponseDetail>(responseDetail);
                        xmlResponseDetail.ResponseEnforcementIdentifier = enforcementId++;

                        xmlViolation.FacilityResponseDetails.Add(xmlResponseDetail);
                    }

                    xmlFacility.FacilityViolationDetail.Add(xmlViolation);
                }
             
                var facilityDetailElement = XmlService.AddFacility(ref payload, xmlFacility);

                var wellCursor = wellRelation.GetObjectsRelatedToObject(facilityFeature);
                comObjects.Push(wellCursor);

                // Find all wells
                var wellIdentifier = 0;
                IFeature wellFeature;
                while ((wellFeature = wellCursor.Next()) != null)
                {
                    var well = AutoMapperService.MapWellModel(wellFeature, wellFieldMap);
                    var xmlWell = mapper.Map<WellSdeModel, WellDetail>(well);
                    xmlWell.WellIdentifier = wellIdentifier++;

                    var verticalWellCursor = verticalWellRelation.GetObjectsRelatedToObject(wellFeature);
                    comObjects.Push(verticalWellCursor);

                    var verticalEventFeature = verticalWellCursor.Next();
                    var verticalEvent = AutoMapperService.MapVerticalWellEventModel(verticalEventFeature, verticalWellFieldMap);

                    xmlWell.WellTotalDepthNumeric = verticalEvent.EventType.ToString();

                    var wellStatusCursor = wellStatusRelation.GetObjectsRelatedToObject(wellFeature);
                    comObjects.Push(wellStatusCursor);

                    var wellStatusIdentifier = 0;
                    IObject wellStatusFeature;
                    while ((wellStatusFeature = wellStatusCursor.Next()) != null)
                    {
                        var wellStatus = AutoMapperService.MapWellStatusModel(wellStatusFeature, wellStatusFieldMap);
                        var xmlWellStatus = mapper.Map<WellStatusSdeModel, WellStatusDetail>(wellStatus);
                        xmlWellStatus.WellStatusIdentifier = wellStatusIdentifier++;

                        xmlWell.WellStatusDetail.Add(xmlWellStatus);
                    }

                    // Add Well Type Details
                    var wellTypeDetail = new WellTypeDetail
                    {
                        WellTypeCode = xmlWell.WellTypeCode,
                        WellTypeIdentifier = 0,
                        // WellTypeDate = TODO: github #6
                        WellTypeWellIdentifier = well.Guid
                    };

                    xmlWell.WellTypeDetail.Add(wellTypeDetail);

                    dynamic x = new ExpandoObject();
                    x.WellTypeWellIdentifer = well.Guid;
                    x.WellAddressCounty = facility.CountyFips;
                    x.LocationAccuracyValueMeasure = well.LocationAccuracy;
                    x.GeographicReferencePointCode = "026";
                    x.HorizontalCoordinateReferenceSystemDatumCode = "002";
                    x.HorizontalCollectionMethodCode = well.LocationMethod;
                    x.LocationPointLiveAreaCode = "001";
                    x.SourceMapScaleNumeric = well.LocationAccuracy;
                    x.LocationWellIdentifier = well.Guid;
                    // TODO: Convert to lat long
                    x.LatitudeMeasure = 0; 
                    x.LongitudeMeasure = 1;
                }
            }

            debug.Write("{1} Releasing COMOBJECTS: {0}", comObjects.Count, start.Elapsed);

            foreach (var item in comObjects)
            {
                ComReleaser.ReleaseCOMObject(item);
            }

            comObjects.Clear();

            debug.Write("{0} finished.", start.Elapsed);

            Console.ReadKey();
        }
    }
}