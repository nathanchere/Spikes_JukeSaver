using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NAudio.Dsp;
using NAudio.Wave;

namespace NAudio.SpectrumAnalyser
{
    class Program
    {               
        static void Main(string[] args)
        {            
            IWavePlayer device = new DirectSoundOut();
            var audio = new AudioFileReader("doowackadoo.mp3");            
            var aggregator = GetAggregator(audio);            
            device.Init(aggregator);

            device.Play();

            Console.WriteLine("Playing doowackadoo; Ctrl+C to quit");
            quit.WaitOne();

            if(device != null) device.Stop();
            if(audio != null) audio.Dispose();
            if(device != null) device.Dispose();
        }

        private static SampleAggregator GetAggregator(AudioFileReader audio)
        {
            var result = new SampleAggregator(audio,FFT_SIZE){
                NotificationCount = (int)(audio.WaveFormat.SampleRate * 0.01),
                PerformFFT = true,                
            };

            result.FftCalculated += (sender, args) => DoFft(args.Result);
            result.MaximumCalculated += (sender, args) => DoMaxCalculated(args);

            return result;
        }

        private static byte fftUpdateCount = 0;
        private static void DoFft(Complex[] results)
        {
            if(fftUpdateCount != 30)
            {
                fftUpdateCount += 1;
                return;
            }
            
            Console.WriteLine("FFT: " + results.Count());

        }

        private static byte maxUpdateCount = 0;
        private static void DoMaxCalculated(MaxSampleEventArgs args)
        {
            if(maxUpdateCount != 30)
            {
                maxUpdateCount  += 1;
                return;
            }

            Console.WriteLine("Maxcalcd: " + args.MaxSample);
        }
    }
}
