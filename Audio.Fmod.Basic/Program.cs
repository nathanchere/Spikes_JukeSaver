using System;
using System.Threading;
using FmodSharp = Linsft.FmodSharp;

namespace Audio.Fmod.Basic
{
    class Program
    {
        static void Main(string[] args)
        {
            const string FILE_NAME = @".\doowackadoo.mp3";

            var quit = new ManualResetEvent(false);
            Console.CancelKeyPress += (s, a) => {
                quit.Set();
                a.Cancel = true;
            };

            using (var fmod = new FmodSharp.SoundSystem.SoundSystem())
            {
                fmod.Init();

                var audio = fmod.CreateSound(FILE_NAME);
                fmod.PlaySound(audio);                

                Console.WriteLine("FmodSharp test\nPlaying doowackadoo; Ctrl+C to quit");
                quit.WaitOne();                

                fmod.CloseSystem();
            }
        }
    }
}
