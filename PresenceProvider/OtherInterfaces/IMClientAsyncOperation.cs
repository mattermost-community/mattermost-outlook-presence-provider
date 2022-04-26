using System.Diagnostics;
using System.Runtime.InteropServices;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    [ComVisible(true)]
    public class IMClientAsyncOperation : AsynchronousOperation
    {
        public IMClientAsyncOperation()
        {
        }

        public void CancelOperation()
        {
            Trace.TraceInformation("Operation cancelled");
        }

        private dynamic _asyncState;
        public dynamic AsyncState
        {
            get => _asyncState;
            set => _asyncState = value;
        }

        private int _statusCode;
        public int StatusCode
        {
            get => _statusCode;
            set => _statusCode = value;
        }

        private bool _isCompleted;
        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }

        private bool _isSucceeded;
        public bool IsSucceeded
        {
            get => _isSucceeded;
            set => _isSucceeded = value;
        }

        private bool _isCancelable;
        public bool IsCancelable
        {
            get => _isCancelable;
            set => _isCancelable = value;
        }

        private int _diagnosticCode;
        public int DiagnosticCode
        {
            get => _diagnosticCode;
            set => _diagnosticCode = value;
        }
    }
}
