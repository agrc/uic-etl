﻿using System;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;

namespace uic_etl.services
{
    internal static class WorkspaceService
    {
        internal static IWorkspace GetSdeWorkspace(string connectionString)
        {
            RuntimeManager.Bind(ProductCode.Desktop); 

            var init = new AoInitialize();
            init.Initialize(esriLicenseProductCode.esriLicenseProductCodeBasic);

            var factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.SdeWorkspaceFactory");
            var workspaceFactory2 = (IWorkspaceFactory2)Activator.CreateInstance(factoryType);

            return workspaceFactory2.OpenFromFile(connectionString, 0);
        }
    }
}