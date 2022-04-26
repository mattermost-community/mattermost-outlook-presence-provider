using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ComTypes = System.Runtime.InteropServices.ComTypes;

namespace CSExeCOMServerTest
{
    // Reference: https://github.com/dotnet/samples/blob/main/core/extensions/OutOfProcCOM/COMRegistration/TypeLib.cs
    internal static class TypeLib
    {
        public static void Register(string tlbPath)
        {
            Trace.TraceInformation($"Registering type library:");
            Trace.TraceInformation(tlbPath);
            ComTypes.ITypeLib typeLib;
            int hr = OleAut32.LoadTypeLibEx(tlbPath, OleAut32.REGKIND.REGKIND_NONE, out typeLib);
            if (hr < 0)
            {
                Trace.TraceError($"Registering type library failed: 0x{hr:x}");
                return;
            }
            hr = OleAut32.RegisterTypeLibForUser(typeLib, tlbPath, string.Empty);
            if (hr < 0)
            {
                Trace.TraceError($"Registering type library failed: 0x{hr:x}");
                return;
            }
            Trace.TraceInformation($"Registering type library succeeded.");
        }

        public static void Unregister(string tlbPath)
        {
            Trace.TraceInformation($"Unregistering type library:");
            Trace.TraceInformation(tlbPath);

            ComTypes.ITypeLib typeLib;
            int hr = OleAut32.LoadTypeLibEx(tlbPath, OleAut32.REGKIND.REGKIND_NONE, out typeLib);
            if (hr < 0)
            {
                Trace.TraceError($"Unregistering type library failed: 0x{hr:x}");
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
                        Trace.TraceError($"Unregistering type library failed: 0x{hr:x}");
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
