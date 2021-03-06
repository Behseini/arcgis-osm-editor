// (c) Copyright Esri, 2010 - 2016
// This source is subject to the Apache 2.0 License.
// Please see http://www.apache.org/licenses/LICENSE-2.0.html for details.
// All other rights reserved.


using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Resources;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.esriSystem;

using System.Xml;
using System.Xml.Serialization;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.DataSourcesGDB;
using System.IO;
using System.Reflection;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.OSM.OSMClassExtension;

namespace ESRI.ArcGIS.OSM.GeoProcessing
{
    [Guid("75e2c8f2-c54e-4bc6-84d8-b52a853f5fda")]
    [ClassInterface(ClassInterfaceType.None)]
    [ProgId("OSMEditor.OSMGPRemoveExtension")]
    public class OSMGPRemoveExtension : ESRI.ArcGIS.Geoprocessing.IGPFunction2
    {

        string m_DisplayName = String.Empty;
        int in_osmFeaturesNumber, out_osmFeaturesNumber;
        ResourceManager resourceManager = null;
        OSMGPFactory osmGPFactory = null;

        public OSMGPRemoveExtension()
        {
            resourceManager = new ResourceManager("ESRI.ArcGIS.OSM.GeoProcessing.OSMGPToolsStrings", this.GetType().Assembly);
            osmGPFactory = new OSMGPFactory();
        }

        #region "IGPFunction2 Implementations"
        public ESRI.ArcGIS.esriSystem.UID DialogCLSID
        {
            get
            {
                return default(ESRI.ArcGIS.esriSystem.UID);
            }
        }

        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(m_DisplayName))
                {
                    m_DisplayName = osmGPFactory.GetFunctionName(OSMGPFactory.m_RemoveExtensionName).DisplayName;
                }

                return m_DisplayName;
            }
        }

        public void Execute(ESRI.ArcGIS.esriSystem.IArray paramvalues, ESRI.ArcGIS.esriSystem.ITrackCancel TrackCancel, ESRI.ArcGIS.Geoprocessing.IGPEnvironmentManager envMgr, ESRI.ArcGIS.Geodatabase.IGPMessages message)
        {
            try
            {
                IGPUtilities3 gpUtilities3 = new GPUtilitiesClass();

                if (TrackCancel == null)
                {
                    TrackCancel = new CancelTrackerClass();
                }

                IGPParameter inputFeatureClassParameter = paramvalues.get_Element(in_osmFeaturesNumber) as IGPParameter;
                IGPValue inputFeatureGPValue = gpUtilities3.UnpackGPValue(inputFeatureClassParameter) as IGPValue;

                IFeatureClass osmFeatureClass = null;
                IQueryFilter queryFilter = null;

                gpUtilities3.DecodeFeatureLayer(inputFeatureGPValue, out osmFeatureClass, out queryFilter);

                ((ITable)osmFeatureClass).RemoveOSMClassExtension();
            }
            catch (Exception ex)
            {
                message.AddError(120057, ex.Message);
            }
        }

        public ESRI.ArcGIS.esriSystem.IName FullName
        {
            get
            {
                IName fullName = null;

                if (osmGPFactory != null)
                {
                    fullName = osmGPFactory.GetFunctionName(OSMGPFactory.m_RemoveExtensionName) as IName;
                }

                return fullName;
            }
        }

        public object GetRenderer(ESRI.ArcGIS.Geoprocessing.IGPParameter pParam)
        {
            return default(object);
        }

        public int HelpContext
        {
            get
            {
                return default(int);
            }
        }

        public string HelpFile
        {
            get
            {
                return default(string);
            }
        }

        public bool IsLicensed()
        {
            return true;
        }

        public string MetadataFile
        {
            get
            {
                string metadafile = "osmgpremoveextension.xml";

                try
                {
                    string[] languageid = System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Split("-".ToCharArray());

                    string ArcGISInstallationLocation = OSMGPFactory.GetArcGIS10InstallLocation();
                    string localizedMetaDataFileShort = ArcGISInstallationLocation + System.IO.Path.DirectorySeparatorChar.ToString() + "help" + System.IO.Path.DirectorySeparatorChar.ToString() + "gp" + System.IO.Path.DirectorySeparatorChar.ToString() + "osmgpremoveextension_" + languageid[0] + ".xml";
                    string localizedMetaDataFileLong = ArcGISInstallationLocation + System.IO.Path.DirectorySeparatorChar.ToString() + "help" + System.IO.Path.DirectorySeparatorChar.ToString() + "gp" + System.IO.Path.DirectorySeparatorChar.ToString() + "osmgpremoveextension_" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + ".xml";

                    if (System.IO.File.Exists(localizedMetaDataFileShort))
                    {
                        metadafile = localizedMetaDataFileShort;
                    }
                    else if (System.IO.File.Exists(localizedMetaDataFileLong))
                    {
                        metadafile = localizedMetaDataFileLong;
                    }
                }
                catch { }

                return metadafile;
            }
        }

        public string Name
        {
            get
            {
                return OSMGPFactory.m_RemoveExtensionName;
            }
        }

        public ESRI.ArcGIS.esriSystem.IArray ParameterInfo
        {
            get
            {
                IArray parameters = new ArrayClass();

                IGPParameterEdit3 inputOSMFeatureLayer = new GPParameterClass() as IGPParameterEdit3;
                inputOSMFeatureLayer.Name = "in_osmfeatures";
                inputOSMFeatureLayer.DisplayName = resourceManager.GetString("GPTools_OSMGPRemoveExtension_inputlayer_displayname");
                inputOSMFeatureLayer.ParameterType = esriGPParameterType.esriGPParameterTypeRequired;
                inputOSMFeatureLayer.Direction = esriGPParameterDirection.esriGPParameterDirectionInput;
                inputOSMFeatureLayer.DataType = new GPFeatureLayerTypeClass();

                in_osmFeaturesNumber = 0;
                parameters.Add((IGPParameter) inputOSMFeatureLayer);

                IGPParameterEdit3 outputOSMFeatureLayer = new GPParameterClass() as IGPParameterEdit3;
                outputOSMFeatureLayer.Name = "out_osmfeatures";
                outputOSMFeatureLayer.DisplayName = resourceManager.GetString("GPTools_OSMGPRemoveExtension_outputlayer_displayname");
                outputOSMFeatureLayer.ParameterType = esriGPParameterType.esriGPParameterTypeDerived;
                outputOSMFeatureLayer.Direction = esriGPParameterDirection.esriGPParameterDirectionOutput;
                outputOSMFeatureLayer.DataType = new GPFeatureLayerTypeClass();
                outputOSMFeatureLayer.AddDependency("in_osmfeatures");

                out_osmFeaturesNumber = 1;
                parameters.Add((IGPParameter) outputOSMFeatureLayer);


                return parameters;
            }
        }

        public void UpdateMessages(ESRI.ArcGIS.esriSystem.IArray paramvalues, ESRI.ArcGIS.Geoprocessing.IGPEnvironmentManager pEnvMgr, ESRI.ArcGIS.Geodatabase.IGPMessages Messages)
        {
            IGPUtilities3 gpUtilities3 = new GPUtilitiesClass();

            IGPParameter inputFCParameter = paramvalues.get_Element(in_osmFeaturesNumber) as IGPParameter;
            IGPValue osmFCGPValue = gpUtilities3.UnpackGPValue(inputFCParameter);

            if (osmFCGPValue.IsEmpty())
            {
                return;
            }

            IFeatureClass osmFeatureClass = null;
            IQueryFilter queryFilter = null;

            if (osmFCGPValue is IGPFeatureLayer)
            {
                gpUtilities3.DecodeFeatureLayer(osmFCGPValue, out osmFeatureClass, out queryFilter);

                if (osmFeatureClass.EXTCLSID != null)
                {
                    UID osmEditorExtensionCLSID = osmFeatureClass.EXTCLSID;

                    if (osmEditorExtensionCLSID.Value.ToString().Equals("{65CA4847-8661-45eb-8E1E-B2985CA17C78}", StringComparison.InvariantCultureIgnoreCase) == false)
                    {
                        Messages.ReplaceError(in_osmFeaturesNumber, -1, resourceManager.GetString("GPTools_OSMGPRemoveExtension_noEXTCLSID")); 
                    }
                }
                else
                {
                    Messages.ReplaceError(in_osmFeaturesNumber, -1, resourceManager.GetString("GPTools_OSMGPRemoveExtension_noEXTCLSID"));
                }
            }

        }

        public void UpdateParameters(ESRI.ArcGIS.esriSystem.IArray paramvalues, ESRI.ArcGIS.Geoprocessing.IGPEnvironmentManager pEnvMgr)
        {
        }

        public ESRI.ArcGIS.Geodatabase.IGPMessages Validate(ESRI.ArcGIS.esriSystem.IArray paramvalues, bool updateValues, ESRI.ArcGIS.Geoprocessing.IGPEnvironmentManager envMgr)
        {
            return default(ESRI.ArcGIS.Geodatabase.IGPMessages);
        }
        #endregion

    }
}
