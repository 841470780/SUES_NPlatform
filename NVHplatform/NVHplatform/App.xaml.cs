using NVHplatform.ViewModels;
using NVHplatform.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NVHplatform
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        // App.xaml.cs

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var waveformVM = new WaveformChartViewModel();
            var spectrumVM = new SpectrumChartViewModel();
            var fluctuationVM = new FluctuationChartViewModel();

            var mainVM = new MainWindowViewModel();

            MainWindow = new MainWindow
            {
                DataContext = mainVM
            };
            MainWindow.Show();




            //base.OnStartup(e);

            //// 创建并显示TestWindow
            //var test = new test();
            //test.Show();

            //// 设置为应用主窗口（可选，通常推荐）
            //this.MainWindow = test;
        }

    }
}
