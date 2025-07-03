using LiveChartsCore.SkiaSharpView.WPF;
using NVHplatform.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NVHplatform.Views
{
    public partial class ChartsView : UserControl
    {
        public ChartsView()
        {
            InitializeComponent();

            if (this.DataContext is ChartsViewModel vm)
            {
                vm.ExportRequested += ExportAllChartsAsImage;
                vm.ExportSingleChartRequested += ExportSingleChartAsImage;
            }

            this.DataContextChanged += (s, e) =>
            {
                if (e.NewValue is ChartsViewModel newVm)
                {
                    newVm.ExportRequested += ExportAllChartsAsImage;
                    newVm.ExportSingleChartRequested += ExportSingleChartAsImage;
                }
            };
        }

        private void AddChart_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ChartTypeDialog();
            dlg.Owner = Window.GetWindow(this);
            if (dlg.ShowDialog() == true && dlg.SelectedType.HasValue)
            {
                var vm = this.DataContext as ChartsViewModel;
                vm?.AddChartWithType(dlg.SelectedType.Value);
            }
        }

        private void ExportAllChartsAsImage(object sender, EventArgs e)
        {
            var charts = FindVisualChildren<CartesianChart>(this).ToList();

            if (charts.Count == 0)
            {
                MessageBox.Show("当前没有图表可以导出。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Charts");
            Directory.CreateDirectory(folder);

            for (int i = 0; i < charts.Count; i++)
            {
                string filePath = Path.Combine(folder, $"Chart_{i + 1}.png");
                SaveFrameworkElementAsPng(charts[i], filePath);
            }

            MessageBox.Show($"成功导出 {charts.Count} 张图表至 Charts 文件夹。", "导出完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSingleChartAsImage(object sender, ChartsViewModel.ChartItem item)
        {
            var dlg = new ExportFormatDialog
            {
                Owner = Window.GetWindow(this)
            };

            if (dlg.ShowDialog() != true || dlg.SelectedFormat == ExportFormatDialog.ExportFormat.None)
                return;

            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Charts");
            Directory.CreateDirectory(folder);
            string baseName = $"{item.Title}_{DateTime.Now:HHmmss}";

            if (dlg.SelectedFormat == ExportFormatDialog.ExportFormat.PNG)
            {
                var chartControl = FindVisualChildren<CartesianChart>(this)
                    .FirstOrDefault(chart => chart.DataContext == item);

                if (chartControl == null)
                {
                    MessageBox.Show("未找到对应图表控件。", "导出失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string filePath = Path.Combine(folder, $"{baseName}.png");
                SaveFrameworkElementAsPng(chartControl, filePath);

                MessageBox.Show($"图表已导出为 PNG 至 Charts 文件夹：{Path.GetFileName(filePath)}", "导出成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                float[] samples = item switch
                {
                    ChartsViewModel.WaveformChartItem waveform => waveform.VM.GetSamples(),
                    ChartsViewModel.SpectrumChartItem spectrum => spectrum.VM.GetTimeDomainSamples(),
                    ChartsViewModel.FluctuationChartItem fluct => fluct.VM.GetSamples(),
                    _ => null
                };

                if (samples == null || samples.Length == 0)
                {
                    MessageBox.Show("该图表无可导出的原始数据。", "导出失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (dlg.SelectedFormat == ExportFormatDialog.ExportFormat.MAT)
                {
                    string matPath = Path.Combine(folder, $"{baseName}.mat");
                    SaveSamplesToMatFile(samples, matPath);
                    MessageBox.Show($"数据已导出为 MAT 至 Charts 文件夹：{Path.GetFileName(matPath)}", "导出成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (dlg.SelectedFormat == ExportFormatDialog.ExportFormat.CSV)
                {
                    string csvPath = Path.Combine(folder, $"{baseName}.csv");
                    SaveSamplesToCsvFile(samples, csvPath);
                    MessageBox.Show($"数据已导出为 CSV 至 Charts 文件夹：{Path.GetFileName(csvPath)}", "导出成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }



        private static void SaveFrameworkElementAsPng(FrameworkElement element, string filePath)
        {
            if (element == null || element.ActualWidth == 0 || element.ActualHeight == 0)
                return;

            var width = (int)element.ActualWidth;
            var height = (int)element.ActualHeight;

            var renderTarget = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            renderTarget.Render(element);

            var pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                pngEncoder.Save(stream);
            }
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T t)
                    yield return t;

                foreach (var descendant in FindVisualChildren<T>(child))
                    yield return descendant;
            }
        }
        private static void SaveSamplesToCsvFile(float[] samples, string filePath)
        {
            var lines = samples.Select((val, index) => $"{index},{val}");
            File.WriteAllLines(filePath, lines);
        }

        /// <summary>
        /// 保存数据为 MATLAB v4 格式
        /// </summary>
        private static void SaveSamplesToMatFile(float[] samples, string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Create))
            using (var bw = new BinaryWriter(fs))
            {
                // 写入头部 (type = 1000 表示 double 类型)
                bw.Write(1000);                      // type: double
                bw.Write(1);                         // mrows
                bw.Write(samples.Length);            // ncols
                bw.Write(0);                         // imagf
                bw.Write(Encoding.ASCII.GetBytes("samples".PadRight(20, '\0'))); // var name (20 bytes)

                // 写入数据
                foreach (var val in samples)
                {
                    bw.Write((double)val);
                }
            }
        }
    }
}
