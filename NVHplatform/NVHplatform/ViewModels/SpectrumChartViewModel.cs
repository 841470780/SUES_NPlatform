// ViewModels/SpectrumChartViewModel.cs
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NVHplatform.ViewModels
{
    public class SpectrumChartViewModel : INotifyPropertyChanged
    {
        private const int MaxPoints = 512;
        private ObservableCollection<double> _spectrumValues = new ObservableCollection<double>();
        public ISeries[] SpectrumSeries { get; set; }

        public SpectrumChartViewModel()
        {
            SpectrumSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = _spectrumValues,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.MediumVioletRed) { StrokeThickness = 1.5f },
                    GeometrySize = 0
                }
            };
        }

        public void UpdateSpectrum(float[] samples)
        {
            int fftLength = 1024;
            var fftBuffer = new NAudio.Dsp.Complex[fftLength];

            for (int i = 0; i < fftLength; i++)
            {
                fftBuffer[i].X = (float)(i < samples.Length
                    ? samples[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftLength)
                    : 0);
                fftBuffer[i].Y = 0;
            }

            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2), fftBuffer);

            _spectrumValues.Clear();
            for (int i = 0; i < fftLength / 2; i++)
            {
                double magnitude = 20 * Math.Log10(Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X +
                                                              fftBuffer[i].Y * fftBuffer[i].Y) + double.Epsilon);
                _spectrumValues.Add(magnitude);
            }

            ((LineSeries<double>)SpectrumSeries[0]).Values = _spectrumValues;
            OnPropertyChanged(nameof(SpectrumSeries));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
