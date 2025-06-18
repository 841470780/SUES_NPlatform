using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NVHplatform.ViewModels
{
    public class FluctuationChartViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<double> _fluctuationValues;
        private ISeries[] _fluctuationSeries;
        private Axis[] _xAxes;
        private Axis[] _yAxes;

        public ObservableCollection<double> FluctuationValues
        {
            get => _fluctuationValues;
            set
            {
                _fluctuationValues = value;
                OnPropertyChanged();
            }
        }

        public ISeries[] FluctuationSeries
        {
            get => _fluctuationSeries;
            set
            {
                _fluctuationSeries = value;
                OnPropertyChanged();
            }
        }

        public Axis[] XAxes
        {
            get => _xAxes;
            set
            {
                _xAxes = value;
                OnPropertyChanged();
            }
        }

        public Axis[] YAxes
        {
            get => _yAxes;
            set
            {
                _yAxes = value;
                OnPropertyChanged();
            }
        }

        public FluctuationChartViewModel()
        {
            FluctuationValues = new ObservableCollection<double> { 0.2, 0.4, 0.35, 0.6, 0.55, 0.8, 0.75 };

            FluctuationSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = FluctuationValues,
                    GeometrySize = 6,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.OrangeRed, 2),
                    GeometryFill = new SolidColorPaint(SKColors.White),
                    GeometryStroke = new SolidColorPaint(SKColors.OrangeRed, 2)
                }
            };

            XAxes = new[]
            {
                CreateAxis("Time (s)", v => v.ToString("F1"))
            };

            YAxes = new[]
            {
                CreateAxis("Fluctuation", v => v.ToString("F2"), minLimit: 0)
            };
        }

        private Axis CreateAxis(string name, Func<double, string> labeler, double? minLimit = null)
        {
            return new Axis
            {
                Name = name,
                NameTextSize = 16,
                TextSize = 14,
                Labeler = labeler,
                MinLimit = minLimit,
                LabelsPaint = new SolidColorPaint(SKColors.Black),
                TicksPaint = new SolidColorPaint(SKColors.Gray),
                SeparatorsPaint = new SolidColorPaint(SKColors.LightGray),
                Padding = new LiveChartsCore.Drawing.Padding(5)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
