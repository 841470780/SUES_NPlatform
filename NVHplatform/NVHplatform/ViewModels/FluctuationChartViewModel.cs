using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NVHplatform.ViewModels
{
    public class FluctuationChartViewModel : INotifyPropertyChanged
    {
        private List<float> _rawSamples = new List<float>();
        public int SampleRate { get; set; } = 44100;

        private const int MaxPoints = 200;
        private readonly int winSize = 1024;

        private double _currentTime = 0;
        private ObservableCollection<ObservablePoint> _fluctuationPoints = new ObservableCollection<ObservablePoint>();

        private ISeries[] _fluctuationSeries;
        public ISeries[] FluctuationSeries
        {
            get => _fluctuationSeries;
            set
            {
                _fluctuationSeries = value;
                OnPropertyChanged();
            }
        }

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public FluctuationChartViewModel()
        {
            FluctuationSeries = new ISeries[]
            {
                new LineSeries<ObservablePoint>
                {
                    Values = _fluctuationPoints,
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Crimson, 2),
                    GeometryFill = null,
                    GeometryStroke = null
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
                    Name = "Fluctuation (RMS)",
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

        /// <summary>
        /// 文件导入或分析时刷新波动曲线（整批替换）
        /// </summary>
        public void UpdateFluctuation(float[] samples)
        {
            if (samples == null || samples.Length == 0) return;

            double deltaT = winSize / (double)SampleRate;
            _fluctuationPoints.Clear();
            _currentTime = 0;

            for (int i = 0; i < samples.Length; i += winSize)
            {
                double sum = 0;
                for (int j = 0; j < winSize && (i + j) < samples.Length; j++)
                    sum += samples[i + j] * samples[i + j];
                double rms = Math.Sqrt(sum / winSize);
                _fluctuationPoints.Add(new ObservablePoint(_currentTime, rms));
                _currentTime += deltaT;
            }

            OnPropertyChanged(nameof(FluctuationSeries));
            System.Diagnostics.Debug.WriteLine($"[FluctuationChart] 导入更新完成，共 {_fluctuationPoints.Count} 点");
        }

        /// <summary>
        /// 实时录音追加单个 RMS 点
        /// </summary>
        public void AddRealtimeFluctuationPoint(double rms)
        {
            double deltaT = winSize / (double)SampleRate;

            App.Current.Dispatcher.Invoke(() =>
            {
                if (_fluctuationPoints.Count >= MaxPoints)
                    _fluctuationPoints.RemoveAt(0);

                _fluctuationPoints.Add(new ObservablePoint(_currentTime, rms));
                _currentTime += deltaT;
            });
        }

        /// <summary>
        /// 清除波动图
        /// </summary>
        public void ClearFluctuation()
        {
            _fluctuationPoints.Clear();
            _currentTime = 0;
            OnPropertyChanged(nameof(FluctuationSeries));
        }
        public float[] GetSamples()
        {
            return _rawSamples.ToArray();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
