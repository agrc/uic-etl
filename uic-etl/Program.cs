using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AutoMapper;
using domain.uic_etl.sde;
using domain.uic_etl.xml;
using ESRI.ArcGIS.Geodatabase;
using uic_etl.commands;
using uic_etl.models;
using uic_etl.models.dtos;
using uic_etl.services;

namespace uic_etl
{
    internal class Program
    {
        private static MapperConfiguration Mapper;
        private static void Main(string[] args)
        {
            EtlOptions options;
            IWorkspace workspace;

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
            Stopwatch start = null;
            if (options.Verbose)
            {
                start = Stopwatch.StartNew();
            }

            debug.Write("Staring: {0}", DateTime.Now.ToString("s"));

            debug.Write("{0} Creating XML document.", start.Elapsed);

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

            var comObjects = new List<object>();

            try
            {
                debug.Write("{1} Connecting to: {0}", options.SdeConnectionPath, start.Elapsed);

                workspace = WorkspaceService.GetSdeWorkspace(options.SdeConnectionPath);
                comObjects.Add(workspace);

                debug.Write("{0} Connected.", start.Elapsed);
            }
            catch (COMException e)
            {
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);

                return;
            }

            var featureWorkspace = (IFeatureWorkspace) workspace;
            comObjects.Add(featureWorkspace);

            debug.Write("{0} Opening UICFacility feature class", start.Elapsed);

            var uicFacility = featureWorkspace.OpenFeatureClass("UICFacility");
            comObjects.Add(uicFacility);

            debug.Write("{0} Creating UICFacility field mapping", start.Elapsed);

            var facilityFields = new[]
            {
                "GUID", "FacilityID", "CountyFIPS", "NAICSPrimary", "FacilityName", "FacilityAddress", "FacilityCity",
                "FacilityState", "FacilityZip", "FacilityMilePost", "FacilityType", "NoMigrationPetStatus"
            };

            var facilityFieldMap = new FindIndexByFieldNameCommand(uicFacility, facilityFields).Execute();

            var queryFilter = new QueryFilter
            {
                WhereClause = "1=1"
            };

            comObjects.Add(queryFilter);

            debug.Write("{0} Quering UICFacility features.", start.Elapsed);

            var facilityCursor = uicFacility.Search(queryFilter, true);
            comObjects.Add(facilityCursor);

            IFeature feature;
            while ((feature = facilityCursor.NextFeature()) != null)
            {

            }

            debug.Write("{1} Releasing COMOBJECTS: {0}", comObjects.Count, start.Elapsed);

            // dispost of objects in reverse order they were added
            comObjects.Reverse();
            foreach (var item in comObjects)
            {
                Marshal.ReleaseComObject(item);
            }

            if (options.Verbose)
            {
                start.Stop();
            }

            debug.Write("{0} finished.", start.Elapsed);
        }

        private static MapperConfiguration SetupMappings()
        {
             return new MapperConfiguration(_ => _.CreateMap<UicFacilityModel, FacilityDetailModel>()
                 .ForMember(dest => dest.FacilityIdentifier, opts => opts.MapFrom(src => src.FacilityId))
                 .ForMember(dest => dest.FacilityPetitionStatusCode, opts => opts.MapFrom(src => src.NoMigrationPetStatus))
                 .ForMember(dest => dest.FacilitySiteName, opts => opts.MapFrom(src => src.FacilityName))
                 .ForMember(dest => dest.FacilitySiteTypeCode, opts => opts.MapFrom(src => src.FacilityType))
                 .ForMember(dest => dest.FacilityStateIdentifier, opts => opts.MapFrom(src => src.FacilityState))
                 //.ForMember(dest => dest.LocalityName, opts => opts.MapFrom(src => src.))
                 .ForMember(dest => dest.LocationAddressPostalCode, opts => opts.MapFrom(src => src.FacilityZip))
                 .ForMember(dest => dest.LocationAddressStateCode, opts => opts.MapFrom(src => src.FacilityState))
                 .ForMember(dest => dest.LocationAddressText, opts => opts.MapFrom(src => src.FacilityAddress))
                 );
        }
    }
}