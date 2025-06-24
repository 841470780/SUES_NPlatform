using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NVHplatform.ViewModels
{
    public class FluctuationChartViewModel : INotifyPropertyChanged
    {
        private const int MaxPoints = 200;

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
            var values = new ObservableCollection<double>();

            FluctuationSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = values,
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
                    Name = "Time (frames)",
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
                    Name = "Fluctuation",
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

            int winSize = 1024;
            var rms = new ObservableCollection<double>();
            for (int i = 0; i < samples.Length; i += winSize)
            {
                double sum = 0;
                for (int j = 0; j < winSize && (i + j) < samples.Length; j++)
                    sum += samples[i + j] * samples[i + j];
                rms.Add(Math.Sqrt(sum / winSize));
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                var series = FluctuationSeries.FirstOrDefault() as LineSeries<double>;
                if (series != null)
                {
                    series.Values = rms;
                }
            });

            System.Diagnostics.Debug.WriteLine($"[FluctuationChart] Update 完成，共 {rms.Count} 点");
        }

        /// 实时录音追加单个点（需要 ObservableCollection 支持）
        public void AddRealtimeFluctuationPoint(double rms)
        {
            var line = FluctuationSeries.FirstOrDefault() as LineSeries<double>;
            if (line == null) return;

            if (line.Values is ObservableCollection<double> values)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    if (values.Count >= MaxPoints)
                        values.RemoveAt(0);
                    values.Add(rms);
                });
            }
        }
        public void ClearFluctuation()
        {
            if (FluctuationSeries.Length > 0 && FluctuationSeries[0] is LineSeries<double> line)
            {
                if (line.Values is IList<double> values)
                {
                    values.Clear();
                    OnPropertyChanged(nameof(FluctuationSeries));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
