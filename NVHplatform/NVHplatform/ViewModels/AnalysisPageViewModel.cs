using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MathNet.Numerics.IntegralTransforms;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Linq;
using System.Numerics;
using System.Windows;

namespace NVHplatform.ViewModels
{
    public partial class AnalysisPageViewModel : ObservableObject
    {
        private ISeries[] waveformSeries;
        public ISeries[] WaveformSeries
        {
            get => waveformSeries;
            set => SetProperty(ref waveformSeries, value);
        }

        private ISeries[] spectrumSeries;
        public ISeries[] SpectrumSeries
        {
            get => spectrumSeries;
            set => SetProperty(ref spectrumSeries, value);
        }

        private Axis[] spectrumXAxis;
        public Axis[] SpectrumXAxis
        {
            get => spectrumXAxis;
            set => SetProperty(ref spectrumXAxis, value);
        }

        private int currentSampleRate = 44100;

        [RelayCommand]
        private void ImportAudio()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "WAV 文件|*.wav",
                Title = "选择一个 WAV 音频文件"
            };

            if (ofd.ShowDialog() == true)
            {
                var samples = LoadWavFile(ofd.FileName);
                if (samples != null)
                {
                    ShowWaveform(samples);
                    ShowSpectrum(samples);
                }
            }
        }

        private float[] LoadWavFile(string filePath)
        {
            try
            {
                using var reader = new AudioFileReader(filePath);
                currentSampleRate = reader.WaveFormat.SampleRate;
                var buffer = new float[reader.Length / 4];
                int read = reader.Read(buffer, 0, buffer.Length);
                return buffer.Take(read).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取音频文件失败：" + ex.Message);
                return null;
            }
        }

        private void ShowWaveform(float[] samples)
        {
            var values = samples.Take(5000).Select(v => (double)v).ToArray();

            WaveformSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = values,
                    GeometrySize = 0,
                    Fill = null
                }
            };
        }

        private void ShowSpectrum(float[] samples)
        {
            int fftLength = 2048;
            var fftBuffer = new Complex[fftLength];

            for (int i = 0; i < fftLength && i < samples.Length; i++)
            {
                double window = Math.Pow(Math.Sin(Math.PI * i / fftLength), 2);
                fftBuffer[i] = new Complex(samples[i] * window, 0);
            }

            Fourier.Forward(fftBuffer, FourierOptions.Matlab);

            int numBins = 128;
            var magnitudes = fftBuffer.Take(fftLength / 2).Select(c => c.Magnitude).Take(numBins).ToArray();
            var frequencies = Enumerable.Range(0, numBins)
                .Select(i => (double)i * currentSampleRate / fftLength)
                .ToArray();

            SpectrumSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = magnitudes
                }
            };

            SpectrumXAxis = new Axis[]
            {
                new Axis
                {
                    Labels = frequencies.Select(f => f.ToString("F0")).ToArray(),
                    LabelsRotation = 45,
                    Name = "Frequency (Hz)",
                    TextSize = 12
                }
            };
        }
    }
}
