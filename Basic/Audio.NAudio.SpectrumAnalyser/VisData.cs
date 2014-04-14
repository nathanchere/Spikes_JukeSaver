using System.Collections.Generic;

namespace NAudio.SpectrumAnalyser
{
    public class VisData
    {
        public VisData()
        {
            WaveData = new List<float>();
            SpectrumData = new List<float>();
        }

        public List<float> WaveData;
        public List<float> SpectrumData; 
    }
}