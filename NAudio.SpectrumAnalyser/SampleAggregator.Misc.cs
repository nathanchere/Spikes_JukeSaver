// Shamelessly taken from NAudio.WPFDemo

using System;
using System.Diagnostics;
using NAudio.Dsp;

namespace NAudio.SpectrumAnalyser
{
    public class MaxSampleEventArgs : EventArgs
    {
        public MaxSampleEventArgs(float minValue, float maxValue)
        {
            this.MaxSample = maxValue;
            this.MinSample = minValue;
        }
        public float MaxSample { get; private set; }
        public float MinSample { get; private set; }
    }

    public class FftEventArgs : EventArgs
    {
        public FftEventArgs(Complex[] result)
        {
            this.Result = result;
        }
        public Complex[] Result { get; private set; }
    }
}
