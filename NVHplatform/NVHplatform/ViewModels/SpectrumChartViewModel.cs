// ViewModels/SpectrumChartViewModel.cs
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography;

namespace NVHplatform.ViewModels
{
    public class SpectrumChartViewModel : INotifyPropertyChanged
    {
        private const int MaxPoints = 512;
        private ObservableCollection<double> _spectrumValues = new ObservableCollection<double>();
        public ISeries[] SpectrumSeries { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

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

            XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Frequency (Hz)",
                    NameTextSize = 16,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    LabelsRotation = 0,
                    TextSize = 13,
                    Padding = new LiveChartsCore.Drawing.Padding(10, 5),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 0.5f },
                    TicksPaint = new SolidColorPaint(SKColors.Gray),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    DrawTicksPath = true
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Amplitude (dB)",
                    NameTextSize = 16,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    LabelsRotation = 0,
                    TextSize = 13,
                    Padding = new LiveChartsCore.Drawing.Padding(10, 5),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 0.5f },
                    TicksPaint = new SolidColorPaint(SKColors.Gray),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    DrawTicksPath = true
                }
            };
        }

        public void UpdateSpectrum(float[] samples)
        {
            if (samples == null) return;

            int fftLength = 1024;
            var fftBuffer = new NAudio.Dsp.Complex[fftLength];

            // 必须为每个元素 new 一个 Complex 实例！
            for (int i = 0; i < fftLength; i++)
                fftBuffer[i] = new NAudio.Dsp.Complex();

            for (int i = 0; i < fftLength; i++)
            {
                fftBuffer[i].X = (float)(i < samples.Length
                    ? samples[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftLength)
                    : 0);
                fftBuffer[i].Y = 0;
            }

            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2), fftBuffer);

            if (_spectrumValues == null)
                _spectrumValues = new ObservableCollection<double>();
            else
                _spectrumValues.Clear();

            for (int i = 0; i < fftLength / 2; i++)
            {
                double magnitude = 20 * Math.Log10(Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X +
                                                              fftBuffer[i].Y * fftBuffer[i].Y) + double.Epsilon);
                _spectrumValues.Add(magnitude);
            }

            // 如果 SpectrumSeries[0] 还未初始化，则提前 new 一下
            var line = SpectrumSeries[0] as LineSeries<double>;
            if (line != null)
                line.Values = _spectrumValues;

            OnPropertyChanged(nameof(SpectrumSeries));
        }

        public void ClearSpectrum()
        {
            _spectrumValues.Clear();
            OnPropertyChanged(nameof(SpectrumSeries));
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


    }
}
