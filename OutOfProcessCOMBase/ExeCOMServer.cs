#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
#endregion


namespace CSExeCOMServer
{
    sealed public class ExeCOMServer
    {
        #region Singleton Pattern

        private ExeCOMServer()
        {
        }

        private static ExeCOMServer _instance = new ExeCOMServer();
        public static ExeCOMServer Instance
        {
            get { return _instance; }
        }

        #endregion


        private object syncRoot = new Object(); // For thread-sync in lock
        private bool _bRunning = false; // Whether the server is running

        // The ID of the thread that runs the message loop
        private uint _nMainThreadID = 0;

        // The lock count (the number of active COM objects) in the server
        private int _nLockCnt = 0;
        
        // The timer to trigger GC every 5 seconds
        private Timer _gcTimer;

        public delegate void OnCOMHosted();

        public event OnCOMHosted OnCOMReady;


        /// <summary>
        /// The method is call every 5 seconds to GC the managed heap after 
        /// the COM server is started.
        /// </summary>
        /// <param name="stateInfo"></param>
        private static void GarbageCollect(object stateInfo)
        {
            GC.Collect();   // GC
        }

        private uint _cookieSimpleObj;

        /// <summary>
        /// PreMessageLoop is responsible for registering the COM class 
        /// factories for the COM classes to be exposed from the server, and 
        /// initializing the key member variables of the COM server (e.g. 
        /// _nMainThreadID and _nLockCnt).
        /// </summary>
        private void PreMessageLoop()
        {
            /////////////////////////////////////////////////////////////////
            // Register the COM class factories.
            // 

            Guid clsidSimpleObj = new Guid(ExeCOMServer.Instance.ComInterfaceGUID);

            // Register the CSSimpleObject class object
            int hResult = COMNative.CoRegisterClassObject(
                ref clsidSimpleObj,                 // CLSID to be registered
                new CSSimpleObjectClassFactory(),   // Class factory
                CLSCTX.LOCAL_SERVER,                // Context to run
                REGCLS.MULTIPLEUSE | REGCLS.SUSPENDED,
                out _cookieSimpleObj);
            if (hResult != 0)
            {
                throw new ApplicationException(
                    "CoRegisterClassObject failed w/err 0x" + hResult.ToString("X"));
            }

            // Register other class objects 
            // ...

            // Inform the SCM about all the registered classes, and begins 
            // letting activation requests into the server process.
            hResult = COMNative.CoResumeClassObjects();
            if (hResult != 0)
            {
                // Revoke the registration of CSSimpleObject on failure
                if (_cookieSimpleObj != 0)
                {
                    COMNative.CoRevokeClassObject(_cookieSimpleObj);
                }

                // Revoke the registration of other classes
                // ...

                throw new ApplicationException(
                    "CoResumeClassObjects failed w/err 0x" + hResult.ToString("X"));
            }


            /////////////////////////////////////////////////////////////////
            // Initialize member variables.
            // 

            // Records the ID of the thread that runs the COM server so that 
            // the server knows where to post the WM_QUIT message to exit the 
            // message loop.
            _nMainThreadID = NativeMethod.GetCurrentThreadId();

            // Records the count of the active COM objects in the server. 
            // When _nLockCnt drops to zero, the server can be shut down.
            _nLockCnt = 0;

            // Start the GC timer to trigger GC every 5 seconds.
            _gcTimer = new Timer(new TimerCallback(GarbageCollect), null,
                5000, 5000);

            if (OnCOMReady != null)
            {
                OnCOMReady();
            }
        }

        /// <summary>
        /// RunMessageLoop runs the standard message loop. The message loop 
        /// quits when it receives the WM_QUIT message.
        /// </summary>
        private void RunMessageLoop()
        {
            MSG msg;
            while (NativeMethod.GetMessage(out msg, IntPtr.Zero, 0, 0))
            {
                NativeMethod.TranslateMessage(ref msg);
                NativeMethod.DispatchMessage(ref msg);
            }
        }

        /// <summary>
        /// PostMessageLoop is called to revoke the registration of the COM 
        /// classes exposed from the server, and perform the cleanups.
        /// </summary>
        private void PostMessageLoop()
        {
            /////////////////////////////////////////////////////////////////
            // Revoke the registration of the COM classes.
            // 

            // Revoke the registration of CSSimpleObject
            if (_cookieSimpleObj != 0)
            {
                COMNative.CoRevokeClassObject(_cookieSimpleObj);
            }

            // Revoke the registration of other classes
            // ...


            /////////////////////////////////////////////////////////////////
            // Perform the cleanup.
            // 

            // Dispose the GC timer.
            if (_gcTimer != null)
            {
                _gcTimer.Dispose();
            }

            // Wait for any threads to finish.
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Run the COM server. If the server is running, the function 
        /// returns directly.
        /// </summary>
        /// <remarks>The method is thread-safe.</remarks>
        public void Run(Type comObjType, bool ExitOnLastInstance = true)
        {
            ComObjectType = comObjType;

            ExitOnLastInstanceExit = ExitOnLastInstance;

            lock (syncRoot) // Ensure thread-safe
            {
                // If the server is running, return directly.
                if (_bRunning)
                    return;

                // Indicate that the server is running now.
                _bRunning = true;
            }

            try
            {
                // Call PreMessageLoop to initialize the member variables 
                // and register the class factories.
                PreMessageLoop();

                // Run the message loop.
                RunMessageLoop();

                // Call PostMessageLoop to revoke the registration.
                PostMessageLoop();
            }
            finally
            {
                _bRunning = false;
            }
        }

        /// <summary>
        /// Increase the lock count
        /// </summary>
        /// <returns>The new lock count after the increment</returns>
        /// <remarks>The method is thread-safe.</remarks>
        public int Lock()
        {
            return Interlocked.Increment(ref _nLockCnt);
        }

        /// <summary>
        /// Decrease the lock count. When the lock count drops to zero, post 
        /// the WM_QUIT message to the message loop in the main thread to 
        /// shut down the COM server.
        /// </summary>
        /// <returns>The new lock count after the increment</returns>
        public int Unlock()
        {
            int nRet = Interlocked.Decrement(ref _nLockCnt);

            // If lock drops to zero, attempt to terminate the server.
            if ((nRet == 0) && ExitOnLastInstanceExit)
            {
                // Post the WM_QUIT message to the main thread
                NativeMethod.PostThreadMessage(_nMainThreadID, 
                    NativeMethod.WM_QUIT, UIntPtr.Zero, IntPtr.Zero); 
            }

            return nRet;
        }


        /// <summary>
        /// Get the current lock count.
        /// </summary>
        /// <returns></returns>
        public int GetLockCount()
        {
            return _nLockCnt;
        }

        private bool ExitOnLastInstanceExit { get; set; }

        public Type ComObjectType { set; get; }

        public Type ComInterfaceType
        {
            get
            {
                return ComObjectType.GetInterfaces()[0];
            }
        }

        public string ComInterfaceGUID
        {
            get
            {
                GuidAttribute attr = (GuidAttribute)Attribute.GetCustomAttribute(ComObjectType, typeof(GuidAttribute));
               return attr.Value;
            }
        }
    }

    /// <summary>
    /// Class factory for the class CSSimpleObject.
    /// </summary>
    internal class CSSimpleObjectClassFactory : IClassFactory
    {
        public int CreateInstance(IntPtr pUnkOuter, ref Guid riid, out IntPtr ppvObject)
        {
            ppvObject = IntPtr.Zero;

            if (pUnkOuter != IntPtr.Zero)
            {
                // The pUnkOuter parameter was non-NULL and the object does 
                // not support aggregation.
                Marshal.ThrowExceptionForHR(COMNative.CLASS_E_NOAGGREGATION);
            }

            if (riid == new Guid(ExeCOMServer.Instance.ComInterfaceGUID) ||
                riid == new Guid(COMNative.IID_IDispatch) ||
                riid == new Guid(COMNative.IID_IUnknown))
            {
                // Create the instance of the .NET object
                ppvObject = Marshal.GetComInterfaceForObject(Activator.CreateInstance(ExeCOMServer.Instance.ComObjectType), ExeCOMServer.Instance.ComInterfaceType);
            }
            else
            {
                // The object that ppvObject points to does not support the 
                // interface identified by riid.
                Marshal.ThrowExceptionForHR(COMNative.E_NOINTERFACE);
            }

            return 0;   // S_OK
        }

        public int LockServer(bool fLock)
        {
            return 0;   // S_OK
        }
    }
}