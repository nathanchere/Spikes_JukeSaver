using System;

namespace IPC.MMF
{
    public class BroadcastEventArgs : EventArgs
    {
        public BroadcastEventArgs(Guid token)
        {
            SyncToken = token;
        }

        public Guid SyncToken { get; private set; }
    }
}