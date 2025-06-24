using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Linq;
using System.Windows;

namespace NVHplatform.ViewModels
{
    public partial class AnalysisPageViewModel : ObservableObject
    {
        public ChartsViewModel ChartsVM { get; } = new ChartsViewModel();

        private int currentSampleRate = 44100;

        [RelayCommand]
        private void ImportAudio()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "WAV 文件|*.wav",
                Title = "选择一个 WAV 音频文件"
            };

            if (ofd.ShowDialog() == true)
            {
                var samples = LoadWavFile(ofd.FileName);
                if (samples != null)
                {
                    ChartsVM.UpdateAllCharts(samples); // ⬅️ 核心：统一更新三图
                }
            }
        }

        private float[] LoadWavFile(string filePath)
        {
            try
            {
                using var reader = new AudioFileReader(filePath);
                currentSampleRate = reader.WaveFormat.SampleRate;
                var buffer = new float[reader.Length / 4];
                int read = reader.Read(buffer, 0, buffer.Length);
                return buffer.Take(read).ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取音频文件失败：" + ex.Message);
                return null;
            }
        }
    }
}
