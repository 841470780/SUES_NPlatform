using CommunityToolkit.Mvvm.ComponentModel;
using NAudio.Wave;
using NVHplatform.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.Input;


namespace NVHplatform.ViewModels
{
    public class RecordingViewModel : ObservableObject
    {
        // 图表 ViewModel
        public WaveformChartViewModel WaveformVM { get; }
        public SpectrumChartViewModel SpectrumVM { get; }
        public FluctuationChartViewModel FluctuationVM { get; }
        public IRelayCommand RefreshDevicesCommand { get; }
        public IRelayCommand StartRecordingCommand { get; }
        public IRelayCommand StopRecordingCommand { get; }


        // 录音保存路径
        public string RecordingSavePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recordings");

        private readonly AudioRecorder recorder;

        // 音频设备列表
        private ObservableCollection<WaveInCapabilities> audioDevices = new ObservableCollection<WaveInCapabilities>();
        public ObservableCollection<WaveInCapabilities> AudioDevices
        {
            get => audioDevices;
            set => SetProperty(ref audioDevices, value);
        }

        private int selectedDeviceIndex;
        public int SelectedDeviceIndex
        {
            get => selectedDeviceIndex;
            set => SetProperty(ref selectedDeviceIndex, value);
        }

        private string statusText = "准备就绪";
        public string StatusText
        {
            get => statusText;
            set => SetProperty(ref statusText, value);
        }

        private double volumeLevel;
        public double VolumeLevel
        {
            get => volumeLevel;
            set => SetProperty(ref volumeLevel, value);
        }

        private float recordingVolume = 100f;
        public float RecordingVolume
        {
            get => recordingVolume;
            set
            {
                if (SetProperty(ref recordingVolume, value))
                {
                    SetRecordingVolume(value);
                }
            }
        }

        public RecordingViewModel(
            WaveformChartViewModel waveformVM,
            SpectrumChartViewModel spectrumVM,
            FluctuationChartViewModel fluctuationVM)
        {
            WaveformVM = waveformVM;
            SpectrumVM = spectrumVM;
            FluctuationVM = fluctuationVM;

            RefreshDevicesCommand = new RelayCommand(RefreshDevices);
            StartRecordingCommand = new RelayCommand(StartRecording);
            StopRecordingCommand = new RelayCommand(StopRecording);

            recorder = new AudioRecorder();
            recorder.RawAudioBufferAvailable += OnRawAudioBufferAvailable;
            recorder.VolumeLevelChanged += OnVolumeLevelChanged;

            LoadAudioDevices();
            SetRecordingVolume(RecordingVolume);
        }

        private void LoadAudioDevices()
        {
            AudioDevices.Clear();
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var cap = WaveIn.GetCapabilities(i);
                System.Diagnostics.Debug.WriteLine($"检测到设备[{i}]：{cap.ProductName}");
                AudioDevices.Add(cap);
            }
            if (AudioDevices.Count > 0)
                SelectedDeviceIndex = 0;
            else
                System.Diagnostics.Debug.WriteLine("没有检测到任何设备！");
        }


        private void SetRecordingVolume(float scalar)
        {
            recorder.SetSystemRecordingVolume(scalar / 100f);
        }

        private void OnVolumeLevelChanged(object sender, float volume)
        {
            VolumeLevel = volume * 100;
        }

        private void OnRawAudioBufferAvailable(object sender, float[] samples)
        {
            WaveformVM?.UpdateWaveform(samples);
            SpectrumVM?.UpdateSpectrum(samples);
            //FluctuationVM?.UpdateFluctuation(samples);
        }

        public void StartRecording()
        {
            recorder.DeviceNumber = SelectedDeviceIndex;
            recorder.StartRecording();
            StatusText = "正在录音...";
        }

        public void StopRecording()
        {
            recorder.StopRecording();
            StatusText = "录音已停止";
        }

        public void RefreshDevices()
        {
            LoadAudioDevices();
        }
    }
}
