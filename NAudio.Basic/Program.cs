using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;

namespace NAudio.Basic
{
    class Program
    {        
        static void Main(string[] args)
        {
            bool quit = false;
            Console.CancelKeyPress += (sender, eventArgs) => quit = true;

            IWavePlayer waveOutDevice;
            AudioFileReader audioFileReader;

            waveOutDevice = new WaveOut();
           
            audioFileReader = new AudioFileReader("doowackadoo.mp3");

            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();

            Console.WriteLine("Playing doowackadoo; Ctrl+C to quit");
            while(!quit);
        }
    }
}
