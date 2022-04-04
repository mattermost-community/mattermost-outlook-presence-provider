
#region Using directives
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32;
using UCCollaborationLib;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
#endregion


namespace OutlookPresenceProvider
{

    [ClassInterface(ClassInterfaceType.None)]
    [ComSourceInterfaces(typeof(_IUCOfficeIntegrationEvents))]
    [Guid("A8570DCA-CD23-413C-A8E1-53039C66302A"), ComVisible(true)]
    public class PresenceProvider : CSExeCOMServer.CSExeCOMServerBase, IUCOfficeIntegration
    {
        public static string COMAppExeName = "CSExeCOMServerTest";
        public static readonly HttpClient httpClient = new HttpClient();

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
                using (RegistryKey IMProviders = Registry.CurrentUser.OpenSubKey("SOFTWARE\\IM Providers", true))
                {
                    using (RegistryKey IMProvider = IMProviders.CreateSubKey(COMAppExeName))
                    {
                        IMProvider.SetValue("FriendlyName", "Test Outlook Presence Provider");
                        IMProvider.SetValue("ProcessName", COMAppExeName + ".exe");
                        GuidAttribute attr = (GuidAttribute)Attribute.GetCustomAttribute(typeof(PresenceProvider), typeof(GuidAttribute));
                        IMProvider.SetValue("GUID", attr.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw ex; // Re-throw the exception
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            RegasmUnregisterLocalServer(t);
            using (RegistryKey IMProviders = Registry.CurrentUser.OpenSubKey("SOFTWARE\\IM Providers", true))
            {
                IMProviders.DeleteSubKey(COMAppExeName);
            }
        }

        #endregion

        public static void Started()
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            using (RegistryKey IMProviders = Registry.CurrentUser.OpenSubKey("SOFTWARE\\IM Providers", true))
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
                using (RegistryKey IMProvider = IMProviders.CreateSubKey(COMAppExeName))
                {
                    IMProvider.SetValue("UpAndRunning", 0);
                }
            }
        }

        public OIFeature GetSupportedFeatures(string _version)
        {
            Writer.Print("GetSupportedFeatures was called.");
            OIFeature supportedFeature1 = OIFeature.oiFeatureNonBuddyPresence;

            return supportedFeature1;
        }

        public string GetAuthenticationInfo(string _version)
        {
            // Define the version of Office that the IM client application supports.
            string supportedOfficeVersion = "15.0.0.0";
            Writer.Print($"I was called with version {_version}");

            // Do a simple check for equivalency.
            if (supportedOfficeVersion == _version)
            {
                // If the version of Office is supported, this method must 
                // return the string literal "<authenticationinfo>" exactly.
                Writer.Print("I was returned.");
                return "<authenticationinfo>";
            }
            else
            {
                Writer.Print("null was returned.");
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
                        Writer.Print($"oiInterfaceILyncClient interface was called. {_interface}");
                        return new ClientBase();
                    }
                // The calling code is asking for an object that inherits
                // from IAutomation, so it returns such an object.
                case OIInterface.oiInterfaceIAutomation:
                    {
                        Writer.Print($"oiInterfaceIAutomation interface was called. {_interface}");
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
            if (this.OnShuttingDown != null)
            {
                this.OnShuttingDown();
            }
        }
        #endregion
    }

    public class Writer
    {
        public static void Print(string text)
        {
            // Creating a file
            string myfile = @"WriteLines.txt";

            // Checking the above file
            if (!File.Exists(myfile))
            {
                // Creating the same file if it doesn't exist
                using (StreamWriter sw = File.CreateText(myfile))
                {
                    sw.WriteLine("First line");
                    sw.WriteLine("Second line");
                    sw.WriteLine("Third line");
                }
            }

            // Appending the given texts
            using (StreamWriter sw = File.AppendText(myfile))
            {
                sw.WriteLine(text);
            }
        }
    }
}
