using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using NAudio.Wave;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace NVHplatform
{
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeviceList.Items.Clear();
            int count = WaveIn.DeviceCount;
            if (count == 0)
            {
                DeviceList.Items.Add("未检测到音频输入设备！");
                return;
            }
            for (int i = 0; i < count; i++)
            {
                var cap = WaveIn.GetCapabilities(i);
                DeviceList.Items.Add($"[{i}] {cap.ProductName}");
            }
        }
    }
}


//using LiveChartsCore;
//using LiveChartsCore.SkiaSharpView;
//using LiveChartsCore.SkiaSharpView.Painting;
//using SkiaSharp;
//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Security.Cryptography;

//namespace NVHplatform.ViewModels
//{
//    public class SpectrumChartViewModel : INotifyPropertyChanged
//    {
//        private const int MaxPoints = 512;
//        private ObservableCollection<double> _spectrumValues = new ObservableCollection<double>();
//        public ISeries[] SpectrumSeries { get; set; }
//        public Axis[] XAxes { get; set; }
//        public Axis[] YAxes { get; set; }

//        public SpectrumChartViewModel()
//        {
//            SpectrumSeries = new ISeries[]
//            {
//                new LineSeries<double>
//                {
//                    Values = _spectrumValues,
//                    Fill = null,
//                    Stroke = new SolidColorPaint(SKColors.MediumVioletRed) { StrokeThickness = 1.5f },
//                    GeometrySize = 0
//                }
//            };

//            XAxes = new Axis[]
//            {
//                new Axis
//                {
//                    Name = "Frequency (Hz)",
//                    NameTextSize = 16,
//                    NamePaint = new SolidColorPaint(SKColors.Black),
//                    LabelsRotation = 0,
//                    TextSize = 13,
//                    Padding = new LiveChartsCore.Drawing.Padding(10, 5),
//                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 0.5f },
//                    TicksPaint = new SolidColorPaint(SKColors.Gray),
//                    LabelsPaint = new SolidColorPaint(SKColors.Black),
//                    DrawTicksPath = true
//                }
//            };

//            YAxes = new Axis[]
//            {
//                new Axis
//                {
//                    Name = "Amplitude (dB)",
//                    NameTextSize = 16,
//                    NamePaint = new SolidColorPaint(SKColors.Black),
//                    LabelsRotation = 0,
//                    TextSize = 13,
//                    Padding = new LiveChartsCore.Drawing.Padding(10, 5),
//                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray) { StrokeThickness = 0.5f },
//                    TicksPaint = new SolidColorPaint(SKColors.Gray),
//                    LabelsPaint = new SolidColorPaint(SKColors.Black),
//                    DrawTicksPath = true
//                }
//            };
//        }

//        public void UpdateSpectrum(float[] samples)
//        {
//            int fftLength = 1024;
//            var fftBuffer = new NAudio.Dsp.Complex[fftLength];

//            for (int i = 0; i < fftLength; i++)
//            {
//                fftBuffer[i].X = (float)(i < samples.Length
//                    ? samples[i] * NAudio.Dsp.FastFourierTransform.HammingWindow(i, fftLength)
//                    : 0);
//                fftBuffer[i].Y = 0;
//            }

//            NAudio.Dsp.FastFourierTransform.FFT(true, (int)Math.Log(fftLength, 2), fftBuffer);

//            _spectrumValues.Clear();
//            for (int i = 0; i < fftLength / 2; i++)
//            {
//                double magnitude = 20 * Math.Log10(Math.Sqrt(fftBuffer[i].X * fftBuffer[i].X +
//                                                              fftBuffer[i].Y * fftBuffer[i].Y) + double.Epsilon);
//                _spectrumValues.Add(magnitude);
//            }

//            ((LineSeries<double>)SpectrumSeries[0]).Values = _spectrumValues;
//            OnPropertyChanged(nameof(SpectrumSeries));
//        }

//        public event PropertyChangedEventHandler PropertyChanged;
//        protected void OnPropertyChanged(string name) =>
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


//    }
//}
//但是这个就可以显示，那么问题出在哪里