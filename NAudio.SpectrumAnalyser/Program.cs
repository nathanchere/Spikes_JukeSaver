using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio.Wave;

namespace NAudio.Basic
{
    class Program
    {        
        static void Main(string[] args)
        {
            var quit = new ManualResetEvent(false);
            Console.CancelKeyPress += (s, a) => {
                quit.Set();
                a.Cancel = true;
            };

            var device = new DirectSoundOut();
            var audio = new AudioFileReader("doowackadoo.mp3");

            device.Init(audio);
            device.Play();

            Console.WriteLine("Playing doowackadoo; Ctrl+C to quit");
            quit.WaitOne();

            if(device != null) device.Stop();
            if(audio != null) audio.Dispose();
            if(device != null) device.Dispose();
        }
    }
}
