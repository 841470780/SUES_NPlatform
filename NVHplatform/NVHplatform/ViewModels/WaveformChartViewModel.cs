using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NVHplatform.ViewModels
{
    public class WaveformChartViewModel : INotifyPropertyChanged
    {
        private const int MaxPoints = 1024;
        private List<ObservablePoint> _waveformPoints = new List<ObservablePoint>();        // 用于显示的坐标点
        private float[] _currentSamples; // 原始样本
        private ISeries[] _waveformSeries;

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        // 默认采样率和声压换算因子（单位转换）
        public double SamplingRate { get; set; } = 44100.0;
        public double CalibrationFactor { get; set; } = 1.0; // 若1.0样本 = 0.6325 Pa，可修改为0.6325

        public ISeries[] WaveformSeries
        {
            get => _waveformSeries;
            set
            {
                _waveformSeries = value;
                OnPropertyChanged();
            }
        }

        public WaveformChartViewModel()
        {
            WaveformSeries = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = _waveformPoints,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.DeepSkyBlue, 1.5f),
                    GeometrySize = 0
                }
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Time (s)",
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
                    Name = "Amplitude (Pa)",
                    NameTextSize = 16,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 13,
                    Padding = new LiveChartsCore.Drawing.Padding(10,5),
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 0.5f },
                    TicksPaint = new SolidColorPaint(SKColors.Gray),
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    DrawTicksPath = true
                }
            };
        }

        public void UpdateWaveform(float[] samples)
        {
            if (samples == null) return;

            _currentSamples = samples;
            _waveformPoints.Clear();

            int startIdx = samples.Length > MaxPoints ? samples.Length - MaxPoints : 0;
            double startTime = startIdx / SamplingRate;
            double endTime = samples.Length / SamplingRate;

            XAxes[0].MinLimit = startTime;
            XAxes[0].MaxLimit = endTime;

            for (int i = startIdx; i < samples.Length; i++)
            {
                double timeSec = i / SamplingRate;
                double paValue = samples[i] * CalibrationFactor;
                _waveformPoints.Add(new ObservablePoint(timeSec, paValue));
            }

            WaveformSeries[0].Values = _waveformPoints;
            OnPropertyChanged(nameof(WaveformSeries));
        }



        public float[] GetSamples()
        {
            return _currentSamples ?? new float[0];
        }

        public void ClearWaveform()
        {
            _waveformPoints.Clear();
            OnPropertyChanged(nameof(WaveformSeries));
        }

        public void ExportChart(FrameworkElement chartControl, string filePath)
        {
            if (chartControl == null) return;

            var renderBitmap = new RenderTargetBitmap(
                (int)chartControl.ActualWidth, (int)chartControl.ActualHeight,
                96d, 96d, PixelFormats.Pbgra32);

            renderBitmap.Render(chartControl);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(fileStream);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}