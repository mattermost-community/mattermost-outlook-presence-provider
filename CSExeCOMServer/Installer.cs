using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Reflection;
using System.IO;

namespace CSExeCOMServerTest
{
    // Taken from:http://msdn2.microsoft.com/en-us/library/
    // system.configuration.configurationmanager.aspx
    // Set 'RunInstaller' attribute to true.

    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        private string _appName = "";

        public Installer(){}
        public Installer(string appName)
        {
            _appName = appName;
            // Attach the 'Committed' event.
            this.Committed += new InstallEventHandler(MyInstaller_Committed);
        }

        // Event handler for 'Committed' event.
        private void MyInstaller_Committed(object sender, InstallEventArgs e)
        {
            try
            {
                Trace.TraceInformation(Assembly.GetExecutingAssembly().Location);
                Directory.SetCurrentDirectory(Path.GetDirectoryName
                    (Assembly.GetExecutingAssembly().Location));
                Process.Start(Path.GetDirectoryName($"{Assembly.GetExecutingAssembly().Location}\\{_appName}.exe"));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }

        // Override the 'Install' method.
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);
        }

        // Override the 'Commit' method.
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            try
            {
                Trace.TraceInformation(Assembly.GetExecutingAssembly().Location);
                Directory.SetCurrentDirectory(Path.GetDirectoryName
                    (Assembly.GetExecutingAssembly().Location));
                Process.Start(Path.GetDirectoryName($"{Assembly.GetExecutingAssembly().Location}\\{_appName}.exe"));
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }

        // Override the 'Rollback' method.
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}
