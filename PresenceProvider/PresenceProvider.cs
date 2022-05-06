#region Using directives
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;
using UCCollaborationLib;
#endregion


namespace OutlookPresenceProvider
{

    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(_IUCOfficeIntegrationEvents))]
    [Guid("A8570DCA-CD23-413C-A8E1-53039C66302A"), ComVisible(true)]
    public class PresenceProvider : CSExeCOMServer.CSExeCOMServerBase, IUCOfficeIntegration
    {
        public static string COMAppExeName = "MattermostPresenceProvider";
        public static Mattermost.Client client = new Mattermost.Client();

        #region COM Component Registration

        // These routines perform the additional COM registration needed by 
        // the service.

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                RegasmRegisterLocalServer(t);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message); // Log the error
                Trace.TraceError(ex.StackTrace);
                throw ex; // Re-throw the exception
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            RegasmUnregisterLocalServer(t);
        }

        #endregion

        public static void Started()
        {
            using (RegistryKey IMProviders = Registry.CurrentUser.CreateSubKey("SOFTWARE\\IM Providers", true))
            {
                IMProviders.SetValue("DefaultIMApp", COMAppExeName);
                using (RegistryKey IMProvider = IMProviders.CreateSubKey(COMAppExeName))
                {
                    IMProvider.SetValue("UpAndRunning", 2);
                }
            }
        }

        public static void Stopped()
        {
            using (RegistryKey IMProviders = Registry.CurrentUser.OpenSubKey("SOFTWARE\\IM Providers", true))
            {
                IMProviders.DeleteValue("DefaultIMApp");
                IMProviders.DeleteSubKey(COMAppExeName);
            }
        }

        public OIFeature GetSupportedFeatures(string _version)
        {
            OIFeature supportedFeature1 = OIFeature.oiFeatureNonBuddyPresence;

            return supportedFeature1;
        }

        public string GetAuthenticationInfo(string _version)
        {
            // Define the version of Office that the IM client application supports.
            string supportedOfficeVersion = "15.0.0.0";

            // Do a simple check for equivalency.
            if (supportedOfficeVersion == _version)
            {
                // If the version of Office is supported, this method must 
                // return the string literal "<authenticationinfo>" exactly.
                return "<authenticationinfo>";
            }
            else
            {
                return null;
            }
        }

        public dynamic GetInterface(string _version, OIInterface _interface)
        {
            // Return different object references depending on the value passed in
            // for the _interface parameter.
            switch (_interface)
            {
                // The calling code is asking for an object that inherits
                // from ILyncClient, so it returns such an object.
                case OIInterface.oiInterfaceILyncClient:
                    {
                        return new ClientBase();
                    }
                // The calling code is asking for an object that inherits
                // from IAutomation, so it returns such an object.
                case OIInterface.oiInterfaceIAutomation:
                    {
                        return new AutomationBase();
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        #region _IUCOfficeIntegrationEvents support
        // This event implements void _IUCOfficeIntegrationEvents.OnShuttingDown();
        public event _IUCOfficeIntegrationEvents_OnShuttingDownEventHandler OnShuttingDown;

        // This method is called by the IM application when it is beginning to shut down.
        // The method will raise the OnShuttingDown event which is translated by .NET COM interop layer
        // into a call to _IUCOfficeIntegrationEvents.OnShuttingDown.
        // This notifies Office applications that the IM application is going away.
        internal void RaiseOnShuttingDownEvent()
        {
            if (OnShuttingDown != null)
            {
                OnShuttingDown();
            }
        }
        #endregion
    }
}
