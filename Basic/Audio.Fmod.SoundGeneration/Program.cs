using System;
using System.Threading;
using nFMOD;

namespace Audio.Fmod.Basic
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

            using (var fmod = new FmodSystem())
            {
                fmod.Init();                			    
                using (var oscillator = fmod.CreateDspByType(DspType.Oscillator))
                {
                    var channel = fmod.PlayDsp(oscillator);

                    Console.WriteLine("nFMOD test\nGenerating sine wave; Ctrl+C to quit");
                    quit.WaitOne();
                }
                fmod.CloseSystem();
            }
        }
    }
}
