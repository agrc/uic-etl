using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
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

            var doc = XlmService.CreateDocument();

            var headerModel =  new HeaderInformation
            {
                Title = "data submission for quarter #1, fy 2010",
                CreationTime = DateTime.Now.ToString("s"),
                Comments = "This is a sample"
            };

            debug.Write("{1} Creating header property for: {0}", headerModel.Title, start.Elapsed);

            XlmService.AppendHeader(ref doc, headerModel);

            try
            {
                debug.Write("{1} Connecting to: {0}", options.SdeConnectionPath, start.Elapsed);

                workspace = WorkspaceService.GetSdeWorkspace(options.SdeConnectionPath);

                debug.Write("{0} Connected.", start.Elapsed);
            }
            catch (COMException e)
            {
                Console.Write("uic-etl: ");
                Console.WriteLine(e.Message);

                return; 
            }

            var featureWorkspace = (IFeatureWorkspace)workspace;

            Marshal.ReleaseComObject(featureWorkspace);
            Marshal.ReleaseComObject(workspace);

            if (options.Verbose)
            {
                start.Stop();
            }

            debug.Write("{0} finished.", start.Elapsed);
        }
    }
}