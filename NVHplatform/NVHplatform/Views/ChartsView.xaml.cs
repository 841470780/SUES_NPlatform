using LiveChartsCore.SkiaSharpView.WPF;
using NVHplatform.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // 初始化时绑定 ViewModel 的事件
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
            Directory.CreateDirectory(folder); // 确保文件夹存在

            for (int i = 0; i < charts.Count; i++)
            {
                string filePath = Path.Combine(folder, $"Chart_{i + 1}.png");
                SaveFrameworkElementAsPng(charts[i], filePath);
            }

            MessageBox.Show($"成功导出 {charts.Count} 张图表至Charts文件夹。", "导出完成", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExportSingleChartAsImage(object sender, ChartsViewModel.ChartItem item)
        {
            var chartControl = FindVisualChildren<CartesianChart>(this)
                .FirstOrDefault(chart => chart.DataContext == item);

            if (chartControl == null)
            {
                MessageBox.Show("未找到对应图表控件。", "导出失败", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Charts");
            Directory.CreateDirectory(folder); // 创建目录（如不存在）

            string filePath = Path.Combine(folder, $"{item.Title}_{DateTime.Now:HHmmss}.png");
            SaveFrameworkElementAsPng(chartControl, filePath);


            SaveFrameworkElementAsPng(chartControl, filePath);

            MessageBox.Show($"图表已导出至Charts文件夹：{Path.GetFileName(filePath)}", "导出成功", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
