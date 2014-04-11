using System;
using System.Collections.Generic;

namespace IPC.MMF
{
    public class AudioDataDTO
    {
        public IList<float> Samples { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }       
    }
}