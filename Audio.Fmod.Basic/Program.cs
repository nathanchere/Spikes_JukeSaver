using System;
using System.Threading;

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

            Console.WriteLine("FmodSharp test\nPlaying doowackadoo; Ctrl+C to quit");
            quit.WaitOne();            
        }
    }
}
