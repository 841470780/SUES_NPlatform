using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace NVHplatform.ViewModels
{
    public class FluctuationChartViewModel : INotifyPropertyChanged
    {
        private const int MaxPoints = 200;

        private ObservableCollection<double> _fluctuationValues = new ObservableCollection<double>();

        public ObservableCollection<double> FluctuationValues
        {
            get => _fluctuationValues;
            set
            {
                _fluctuationValues = value;
                OnPropertyChanged();
            }
        }

        public ISeries[] FluctuationSeries { get; set; }

        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public FluctuationChartViewModel()
        {
            FluctuationSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = FluctuationValues,
                    GeometrySize = 0,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.Crimson, 2),
                    GeometryFill = null,
                    GeometryStroke = null
                }
            };

            XAxes = new[]
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

            YAxes = new[]
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

        public void UpdateFluctuation(float[] samples)
        {
            if (samples == null || samples.Length == 0) return;

            double rms = Math.Sqrt(samples.Average(s => s * s));

            App.Current.Dispatcher.Invoke(() =>
            {
                if (FluctuationValues.Count >= MaxPoints)
                    FluctuationValues.RemoveAt(0);
                FluctuationValues.Add(rms);
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
