using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NVHplatform.ViewModels
{
    public class WaveformChartViewModel : INotifyPropertyChanged
    {
        private const int MaxPoints = 1024;
        private ObservableCollection<double> _waveformValues;
        private ISeries[] _waveformSeries;
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

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
            _waveformValues = new ObservableCollection<double>();

            WaveformSeries = new ISeries[]
            {
                new LineSeries<double>
                {
                    Values = _waveformValues,
                    Fill = null,
                    Stroke = new SolidColorPaint(SKColors.DeepSkyBlue, 1.5f),
                    GeometrySize = 0
                }
            };

            XAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Time (samples)",
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
                    Name = "Amplitude",
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
            foreach (var sample in samples)
            {
                _waveformValues.Add(sample);
                if (_waveformValues.Count > MaxPoints)
                    _waveformValues.RemoveAt(0);
            }

            // 显式通知图表更新（虽然通常 ObservableCollection 会自动绑定刷新）
            ((LineSeries<double>)WaveformSeries[0]).Values = _waveformValues;
            OnPropertyChanged(nameof(WaveformSeries));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
