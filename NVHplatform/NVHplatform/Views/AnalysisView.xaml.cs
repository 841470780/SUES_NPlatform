using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NVHplatform.Views
{
    public partial class AnalysisView : UserControl
    {
        public AnalysisView()
        {
            InitializeComponent();
        }

        private void OnImportAudioClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "WAV 文件|*.wav",
                Title = "选择一个 WAV 音频文件"
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    float[] samples = LoadWavFile(ofd.FileName);
                    if (samples != null)
                    {
                        WaveformChartView.ReceiveWaveformSamples(samples);   // 确保 x:Name 正确
                        SpectrumChartView.ReceiveSpectrumSamples(samples);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("音频导入失败：" + ex.Message, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private float[] LoadWavFile(string path)
        {
            using var reader = new AudioFileReader(path);
            float[] buffer = new float[reader.Length / 4];  // 每个 float 占 4 字节
            int read = reader.Read(buffer, 0, buffer.Length);
            return buffer.Take(read).ToArray();
        }
    }
}
