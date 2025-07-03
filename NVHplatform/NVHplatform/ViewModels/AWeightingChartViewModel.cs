using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using NVHplatform.Interop;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace NVHplatform.ViewModels
{
    public class AWeightingChartViewModel
    {
        public ObservableCollection<ISeries> Series { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }

        public AWeightingChartViewModel()
        {
            Series = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double>(),
                    Stroke = new SolidColorPaint(SKColors.SkyBlue, 2),
                    GeometrySize = 4,
                    Fill = null  // 无填充
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
                    Name = "dB (A)",
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

        public void AddRealtimePoint(double dBA)
        {
            var series = Series[0] as LineSeries<double>;
            if (series?.Values is ObservableCollection<double> values)
            {
                values.Add(dBA);
                if (values.Count > 200)
                    values.RemoveAt(0);
            }
        }

        public void Clear()
        {
            var series = Series[0] as LineSeries<double>;
            if (series?.Values is ObservableCollection<double> values)
                values.Clear();
        }

        public void UpdateFromSamples(float[] samples)
        {
            double[] doubles = new double[samples.Length];
            for (int i = 0; i < samples.Length; i++)
                doubles[i] = samples[i];

            double rms = AWeightingInterop.ApplyAWeightingAndComputeRMS(doubles, doubles.Length);
            double dba = AWeightingInterop.ComputeSPLA(rms);

            AddRealtimePoint(dba);
        }
    }
}
