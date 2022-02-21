using System;
using UCCollaborationLib;

namespace OutlookPresenceProvider
{
    public class IMClientAsyncOperation : AsynchronousOperation
    {
        public void CancelOperation()
        {
            throw new NotImplementedException();
        }

        public dynamic AsyncState => throw new NotImplementedException();

        public int StatusCode => throw new NotImplementedException();

        public bool IsCompleted => throw new NotImplementedException();

        public bool IsSucceeded => throw new NotImplementedException();

        public bool IsCancelable => throw new NotImplementedException();

        public int DiagnosticCode => throw new NotImplementedException();
    }
}
