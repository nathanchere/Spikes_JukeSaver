using System;
using System.Collections.Generic;
using System.Text;

namespace IPC.MMF
{
    public class AudioDataDTO
    {
        public IList<float> Samples { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            var result = new StringBuilder(' ');
            foreach (var value in Samples) result.Append(value.ToString("0.00 "));
            return string.Format("{0}:{1:00}:{2:00}.{3:000}: {4} [{5}]",
                Timestamp.Hour,
                Timestamp.Minute,
                Timestamp.Second,
                Timestamp.Millisecond,
                Message,
                Samples
                );
        }
    }
}