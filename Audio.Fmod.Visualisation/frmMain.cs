using System;
using System.Linq;
using System.Windows.Forms;

namespace nFMOD.Demo.SpectrumAnalysis
{
    public partial class frmMain : Form
    {
        private const string MP3_PATH = @"doowackadoo.mp3";
        private FmodSystem fmod;
        private Sound sound;
        private Channel channel;

        public frmMain()
        {
            InitializeComponent();

            fmod = new FmodSystem();
            fmod.Init();

            numSpectrumDetail_ValueChanged(null,null);

            var timer = new Timer();
            timer.Interval = 1000 / 60; // second number = desired FPS
            timer.Tick += (sender, args) => Render();
            timer.Start();
        }

        private void Render()
        {
            if (fmod == null || fmod.IsInvalid
                || channel == null || !channel.IsPlaying
                )
                return;
           
            int spectrumSize = (int)Math.Pow(2,(double) numSpectrumDetail.Value);
            int waveSize = (int)numWaveDetail.Value;

            var spectrum = new float[spectrumSize];
            var wavedata = new float[waveSize];

            var result = new VisData();
            fmod.GetWaveData(wavedata, waveSize, 0);
            fmod.GetSpectrum(spectrum, spectrumSize, 0, FFTWindow.Max);
            result.WaveData = wavedata.ToList();
            result.SpectrumData = spectrum.ToList();

            picVisualisation.UpdateData(result);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (channel != null && channel.IsPlaying)
                return;
            sound = fmod.CreateSound(MP3_PATH);
            channel = fmod.PlaySound(sound);
        }

        private void btnPause(object sender, EventArgs e)
        {
            channel.Paused = !channel.Paused;
        }

        ~frmMain()
        {
            if (sound != null)
            {
                sound.Close();
                sound.Dispose();
            }
            if (channel != null)
            {
                channel.Close();
                channel.Dispose();
            }
            if (fmod != null)
            {
                fmod.Close();
                fmod.Dispose();
            }
        }

        private void numSpectrumDetail_ValueChanged(object sender, EventArgs e)
        {
            lblSpectrum.Text = string.Format("({0})", Math.Pow(2,(double) numSpectrumDetail.Value));
        }
    }
}
