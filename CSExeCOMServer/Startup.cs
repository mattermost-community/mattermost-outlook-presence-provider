using System;
using IWshRuntimeLibrary;

namespace CSExeCOMServerTest
{
    public class Startup
    {
        public static void EnableStartup(string workingDirectory, string appName)
        {
            WshShell wshShell = new WshShell();
            string startUpFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            // Create the shortcut
            IWshShortcut shortcut = (IWshShortcut)wshShell.CreateShortcut($"{startUpFolderPath}\\{appName}.lnk");
            shortcut.TargetPath = $"{workingDirectory}\\{appName}.exe";
            shortcut.WorkingDirectory = workingDirectory;
            shortcut.Description = "Launch Mattermost Presence Provider";
            shortcut.Save();
        }
    }
}
