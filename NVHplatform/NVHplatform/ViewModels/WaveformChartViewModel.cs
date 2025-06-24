using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            if (samples == null || _waveformValues == null) return;

            foreach (var sample in samples)
            {
                _waveformValues.Add(sample);
                if (_waveformValues.Count > MaxPoints)
                    _waveformValues.RemoveAt(0);
            }
        }

        public void ClearWaveform()
        {
            _waveformValues.Clear();
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
