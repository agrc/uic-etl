﻿using System;
using System.Runtime.InteropServices;
using ESRI.ArcGIS.Geodatabase;
using uic_etl.models;
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

            try
            {

                workspace = WorkspaceService.GetSdeWorkspace(options.SdeConnectionPath);

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
        }
    }
}