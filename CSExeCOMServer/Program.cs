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

using System.IO;

namespace CSExeCOMServerTest
{
    class Program
    {
        // The main entry point for the application.
        static void Main(string[] args)
        {
            // Comment below lines if the Unified Collaborations type library is already registered in the system.
            string typeLibName = "UCCollaborationLib.tlb";
            string currentDir = Directory.GetCurrentDirectory();
            string typeLibPath = $"{currentDir}\\{typeLibName}";
            TypeLib.Register(typeLibPath);
           
            CSExeCOMServer.ExeCOMServer.Instance.OnCOMReady += new CSExeCOMServer.ExeCOMServer.OnCOMHosted(OnCOMReady);
            // Run the out-of-process COM server
            CSExeCOMServer.ExeCOMServer.Instance.Run(typeof(OutlookPresenceProvider.PresenceProvider), true);

            OutlookPresenceProvider.PresenceProvider.Stopped();
        }

        static void OnCOMReady()
        {
            OutlookPresenceProvider.PresenceProvider.Started();
        }
    }
}
