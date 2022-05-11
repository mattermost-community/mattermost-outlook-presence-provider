using System;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.IO;

namespace MMPresenceProvider
{
    // Taken from:https://www.codeproject.com/Articles/19560/Launching-Your-Application-After-Install-using-Vis
    // Set 'RunInstaller' attribute to true.

    [RunInstaller(true)]
    public class Installer : System.Configuration.Install.Installer
    {
        public Installer(){}

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
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                Directory.SetCurrentDirectory(Path.GetDirectoryName(assemblyLocation));
                Process.Start(Path.GetDirectoryName($"{assemblyLocation}\\.exe"));
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
