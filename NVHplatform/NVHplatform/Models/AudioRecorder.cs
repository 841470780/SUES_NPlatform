using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void StartRecording()
        {
            try
            {
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recordings");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string outputFilePath = Path.Combine(folderPath, $"recorded_{timestamp}.wav");
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
