using System;

namespace IPC.MMF
{
    public static class Config
    {     
        public const string MAPPED_FILE_NAME = @"ipcmmfserializationtest";
        public const long BufferSize = 65535;
        public static event EventHandler<BroadcastEventArgs> BroadcastEvent;

        internal static void RaiseBroadcastEvent(BroadcastEventArgs e)
        {
            var handler = BroadcastEvent;
            if (handler != null) handler(null, e);
        }
    }
}