/****************************** Module Header ******************************\
* Module Name:	Program.cs
* Project:		CSExeCOMServer
* Copyright (c) Microsoft Corporation.
* 
* The main entry point for the application. It is responsible for starting  
* the out-of-proc COM server registered in the exectuable.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Diagnostics;
using System.IO;

namespace MMPresenceProvider
{
    class Program
    {
        // The main entry point for the application.
        static void Main(string[] args)
        {
            try
            {
                EventLogTraceListener listener = new EventLogTraceListener(MMPresenceProviderImpl.PresenceProvider.COMAppExeName);
                Trace.Listeners.Add(listener);

                // Comment below lines if the Unified Collaborations type library is already registered in the system.
                string currentDir = Directory.GetCurrentDirectory();
                string typeLibName = "UCCollaborationLib.tlb";
                string typeLibPath = $"{currentDir}\\{typeLibName}";
                TypeLib.Register(typeLibPath);
                
                // Enable the app to be run on Windows startup
                Startup.EnableStartup(currentDir, MMPresenceProviderImpl.PresenceProvider.COMAppExeName);

                CSExeCOMServer.ExeCOMServer.Instance.OnCOMReady += new CSExeCOMServer.ExeCOMServer.OnCOMHosted(OnCOMReady);
                // Run the out-of-process COM server
                CSExeCOMServer.ExeCOMServer.Instance.Run(typeof(MMPresenceProviderImpl.PresenceProvider), true);

                MMPresenceProviderImpl.PresenceProvider.Stopped();
            } catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
            }
        }

        static void OnCOMReady()
        {
            MMPresenceProviderImpl.PresenceProvider.Started();
        }
    }
}
