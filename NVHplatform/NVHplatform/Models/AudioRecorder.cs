using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.IO;
using LiveChartsCore.Defaults;
using NVHplatform.ViewModels;
using NVHplatform.Interop;

namespace NVHplatform.Models
{
    public class AudioRecorder
    {
        public event EventHandler<string> RecordingFileSaved;
        public string LatestFilePath { get; private set; }

        private WaveInEvent waveIn;
        private WaveFileWriter writer;
        public int DeviceNumber { get; set; } = 0;
        public float SoftwareGain { get; set; } = 1.0f;

        public event EventHandler<float[]> RawAudioBufferAvailable;
        public event EventHandler<float> VolumeLevelChanged;

        public AWeightingChartViewModel AWeightingChartViewModel { get; set; }

        public FluctuationChartViewModel FluctuationChartViewModel { get; set; }

        public void StartRecording(string fileName)
        {
            try
            {
                // ✅ 停止前一段录音（若未手动 Stop）
                waveIn?.StopRecording();
                writer?.Dispose();
                writer = null;
                waveIn?.Dispose();
                waveIn = null;

                // ✅ 清空上一段的路径
                LatestFilePath = null;

                // 确保目录存在
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recordings");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // 处理非法字符并加后缀
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }

                if (!fileName.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                {
                    fileName += ".wav";
                }

                string outputFilePath = Path.Combine(folderPath, fileName);
                LatestFilePath = outputFilePath;

                waveIn = new WaveInEvent();
                waveIn.DeviceNumber = DeviceNumber;
                waveIn.WaveFormat = new WaveFormat(44100, 16, 1);
                waveIn.DataAvailable += OnDataAvailable;
                waveIn.RecordingStopped += OnRecordingStopped;

                writer = new WaveFileWriter(outputFilePath, waveIn.WaveFormat);
                waveIn.StartRecording();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"启动录音失败: {ex.Message}");
            }
        }



        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            try
            {
                if (SoftwareGain != 1.0f)
                {
                    ApplySoftwareGain(e.Buffer, e.BytesRecorded);
                }

                writer?.Write(e.Buffer, 0, e.BytesRecorded);

                float max = 0;
                float[] samples = new float[e.BytesRecorded / 2];
                for (int i = 0; i < e.BytesRecorded; i += 2)
                {
                    short sample = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i]);
                    float sample32 = sample / 32768f;
                    if (sample32 < 0) sample32 = -sample32;
                    if (sample32 > max) max = sample32;
                    samples[i / 2] = sample32;
                }

                VolumeLevelChanged?.Invoke(this, max);
                RawAudioBufferAvailable?.Invoke(this, samples);

                // ✅ 新增：计算 RMS 并添加至波动图
                if (samples.Length > 0 && FluctuationChartViewModel != null)
                {
                    double sum = 0;
                    foreach (var s in samples)
                        sum += s * s;
                    double rms = Math.Sqrt(sum / samples.Length);

                    FluctuationChartViewModel.AddRealtimeFluctuationPoint(rms);
                }

                // ✅ 接入 A计权图表逻辑
                if (samples.Length > 0 && AWeightingChartViewModel != null)
                {
                    // 转 double[]
                    double[] doubleSamples = new double[samples.Length];
                    for (int i = 0; i < samples.Length; i++)
                        doubleSamples[i] = samples[i];

                    // 调用 DLL 获取 A计权 RMS
                    double rms = AWeightingInterop.ApplyAWeightingAndComputeRMS(doubleSamples, doubleSamples.Length);

                    // 转换为 dB(A)
                    double spl = AWeightingInterop.ComputeSPLA(rms);

                    // 加入图表
                    AWeightingChartViewModel.AddRealtimePoint(spl);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"处理音频数据时出错: {ex.Message}");
            }
        }

        public void SetSystemRecordingVolume(float volumeLevel)
        {
            MMDeviceEnumerator enumerator = null;
            try
            {
                enumerator = new MMDeviceEnumerator();
                var defaultRecordDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);

                if (defaultRecordDevice != null)
                {
                    volumeLevel = Math.Max(0f, Math.Min(1f, volumeLevel));
                    defaultRecordDevice.AudioEndpointVolume.MasterVolumeLevelScalar = volumeLevel;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"音量设置失败: {ex.Message}");
            }
            finally
            {
                enumerator?.Dispose();
            }
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            writer?.Dispose();
            writer = null;
            waveIn?.Dispose();
            RecordingFileSaved?.Invoke(this, LatestFilePath);
        }

        public void StopRecording()
        {
            waveIn?.StopRecording();
        }

        private void ApplySoftwareGain(byte[] buffer, int bytesRecorded)
        {
            for (int i = 0; i < bytesRecorded; i += 2)
            {
                short sample = (short)((buffer[i + 1] << 8) | buffer[i]);
                float amplified = sample * SoftwareGain;
                amplified = Math.Max(short.MinValue, Math.Min(short.MaxValue, amplified));
                short result = (short)amplified;
                buffer[i] = (byte)(result & 0xFF);
                buffer[i + 1] = (byte)(result >> 8);
            }
        }
    }
}
