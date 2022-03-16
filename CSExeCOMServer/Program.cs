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

#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using ComTypes = System.Runtime.InteropServices.ComTypes;
#endregion

namespace CSExeCOMServerTest
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            TypeLib.Register("C:\\Users\\Administrator\\Downloads\\OLPresenceProvider\\OLPresenceProvider\\DLL\\lync4.tlb");
            // TypeLib.Register("C:\\Users\\Administrator\\Downloads\\OLPresenceProvider\\OLPresenceProvider\\DLL\\lyncEndorser.tlb");
            // TypeLib.Register("C:\\Users\\Administrator\\Downloads\\OLPresenceProvider\\OLPresenceProvider\\DLL\\interopExtension.tlb");
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

    internal static class TypeLib
    {
        public static void Register(string tlbPath)
        {
            Trace.WriteLine($"Registering type library:");
            Trace.Indent();
            Trace.WriteLine(tlbPath);
            Trace.Unindent();
            ComTypes.ITypeLib typeLib;
            int hr = OleAut32.LoadTypeLibEx(tlbPath, OleAut32.REGKIND.REGKIND_NONE, out typeLib);
            if (hr < 0)
            {
                Trace.WriteLine($"Registering type library failed: 0x{hr:x}");
                return;
            }
            hr = OleAut32.RegisterTypeLib(typeLib, tlbPath, string.Empty);
            if (hr < 0)
            {
                Trace.WriteLine($"Registering type library failed: 0x{hr:x}");
            }
        }

        public static void Unregister(string tlbPath)
        {
            Trace.WriteLine($"Unregistering type library:");
            Trace.Indent();
            Trace.WriteLine(tlbPath);
            Trace.Unindent();

            ComTypes.ITypeLib typeLib;
            int hr = OleAut32.LoadTypeLibEx(tlbPath, OleAut32.REGKIND.REGKIND_NONE, out typeLib);
            if (hr < 0)
            {
                Trace.WriteLine($"Unregistering type library failed: 0x{hr:x}");
                return;
            }

            IntPtr attrPtr = IntPtr.Zero;
            try
            {
                typeLib.GetLibAttr(out attrPtr);
                if (attrPtr != IntPtr.Zero)
                {
                    ComTypes.TYPELIBATTR attr = Marshal.PtrToStructure<ComTypes.TYPELIBATTR>(attrPtr);
                    hr = OleAut32.UnRegisterTypeLib(ref attr.guid, attr.wMajorVerNum, attr.wMinorVerNum, attr.lcid, attr.syskind);
                    if (hr < 0)
                    {
                        Trace.WriteLine($"Unregistering type library failed: 0x{hr:x}");
                    }
                }
            }
            finally
            {
                if (attrPtr != IntPtr.Zero)
                {
                    typeLib.ReleaseTLibAttr(attrPtr);
                }
            }
        }

        private class OleAut32
        {
            // https://docs.microsoft.com/windows/api/oleauto/ne-oleauto-regkind
            public enum REGKIND
            {
                REGKIND_DEFAULT = 0,
                REGKIND_REGISTER = 1,
                REGKIND_NONE = 2
            }

            // https://docs.microsoft.com/windows/api/oleauto/nf-oleauto-loadtypelibex
            [DllImport(nameof(OleAut32), CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int LoadTypeLibEx(
                [In, MarshalAs(UnmanagedType.LPWStr)] string fileName,
                REGKIND regKind,
                out ComTypes.ITypeLib typeLib);

            // https://docs.microsoft.com/windows/api/oleauto/nf-oleauto-unregistertypelib
            [DllImport(nameof(OleAut32))]
            public static extern int UnRegisterTypeLib(
                ref Guid id,
                short majorVersion,
                short minorVersion,
                int lcid,
                ComTypes.SYSKIND sysKind);

            [DllImport(nameof(OleAut32), CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int RegisterTypeLib(
                 [In] ComTypes.ITypeLib typeLib,
                 [In, MarshalAs(UnmanagedType.LPWStr)] string fileName,
                 [In, MarshalAs(UnmanagedType.LPWStr)] string helpDir);

            [DllImport(nameof(OleAut32), CharSet = CharSet.Unicode, ExactSpelling = true)]
            public static extern int RegisterTypeLibForUser(
                 [In] ComTypes.ITypeLib typeLib,
                 [In, MarshalAs(UnmanagedType.LPWStr)] string fileName,
                 [In, MarshalAs(UnmanagedType.LPWStr)] string helpDir);
        }
    }
}
