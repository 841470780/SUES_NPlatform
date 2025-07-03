using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using NVHplatform.Models;
using NVHplatform.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;


namespace NVHplatform.ViewModels
{
    public class RecordingViewModel : ObservableObject
    {
        public ChartsViewModel ChartsVM { get; }
        //录音信息
        public AudioFileInfoViewModel AudioFileInfoVM { get; } = new AudioFileInfoViewModel();
        //日志
        public LogViewModel Logger { get; } = new LogViewModel();
        // 图表 ViewModel
        public WaveformChartViewModel WaveformVM { get; }
        public SpectrumChartViewModel SpectrumVM { get; }
        public FluctuationChartViewModel FluctuationVM { get; }
        public IRelayCommand RefreshDevicesCommand { get; }
        public IRelayCommand StartRecordingCommand { get; }
        public IRelayCommand StopRecordingCommand { get; }

        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set
            {
                if (SetProperty(ref isRecording, value))
                {
                    // 关键！刷新两个按钮的可用状态
                    (StartRecordingCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    (StopRecordingCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }

        }

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
            ChartsViewModel chartsVM,
            WaveformChartViewModel waveformVM,
            SpectrumChartViewModel spectrumVM,
            FluctuationChartViewModel fluctuationVM)
        {
            ChartsVM = chartsVM;
            WaveformVM = waveformVM;
            SpectrumVM = spectrumVM;
            FluctuationVM = fluctuationVM;

            RefreshDevicesCommand = new RelayCommand(RefreshDevices);
            StartRecordingCommand = new RelayCommand(StartRecording, () => !IsRecording);
            StopRecordingCommand = new RelayCommand(StopRecording, () => IsRecording);


            recorder = new AudioRecorder();
            recorder.RawAudioBufferAvailable += OnRawAudioBufferAvailable;
            recorder.VolumeLevelChanged += OnVolumeLevelChanged;
            recorder.RecordingFileSaved += OnRecordingFileSaved;

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

        // 音量设置失败时记录日志
        private void SetRecordingVolume(float scalar)
        {
            try
            {
                recorder.SetSystemRecordingVolume(scalar / 100f);
            }
            catch (Exception ex)
            {
                var msg = "音量设置失败：" + ex.Message;
                System.Diagnostics.Debug.WriteLine(msg);
                StatusText = msg;
                Logger.AddLog(msg, LogLevel.Error);
            }
        }



        private void OnVolumeLevelChanged(object sender, float volume)
        {
            VolumeLevel = volume * 100;
        }

        private void OnRawAudioBufferAvailable(object sender, float[] samples)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ChartsVM?.UpdateAllCharts(samples); 
            });
        }



        // 开始录音添加日志
        public void StartRecording()
        {
            if (SelectedDeviceIndex < 0 || SelectedDeviceIndex >= AudioDevices.Count)
            {
                StatusText = "请选择有效的录音设备。";
                Logger.AddLog("请选择有效的录音设备。", LogLevel.Warning);
                return;
            }

            // 弹出输入文件名窗口，并居中显示
            var dialog = new InputFileNameDialog
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (dialog.ShowDialog() != true)
            {
                Logger.AddLog("用户取消录音", LogLevel.Info);
                return;
            }

            string fileName = dialog.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                Logger.AddLog("文件名无效", LogLevel.Warning);
                return;
            }

            try
            {
                // ✅ 清空上一段录音信息
                AudioFileInfoVM.CurrentFileInfo = null;

                recorder.DeviceNumber = SelectedDeviceIndex;
                recorder.StartRecording(fileName);  // 传入文件名
                IsRecording = true;
                StatusText = "正在录音...";
                Logger.AddLog($"开始录音，文件名：{fileName}", LogLevel.Info);

            }
            catch (Exception ex)
            {
                var msg = "启动录音失败：" + ex.Message;
                StatusText = msg;
                Logger.AddLog(msg, LogLevel.Error);
            }
        }



        // 停止录音添加日志
        public void StopRecording()
        {
            try
            {
                recorder.StopRecording();
                Logger.AddLog("停止录音命令已发送", LogLevel.Info);
            }
            catch (Exception ex)
            {
                var msg = "停止录音失败：" + ex.Message;
                StatusText = msg;
                Logger.AddLog(msg, LogLevel.Error);
            }
        }

        //停止录音事件处理函数(防止bug)
        private void OnRecordingFileSaved(object sender, string filePath)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                IsRecording = false;
                StatusText = "录音已停止";
                Logger.AddLog("录音已成功保存", LogLevel.Info);
                UpdateAudioFileInfo(filePath);
            });
        }


        //更新录音文件信息
        public void UpdateAudioFileInfo(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                AudioFileInfoVM.CurrentFileInfo = null;
                return;
            }

            using var reader = new AudioFileReader(filePath);
            AudioFileInfoVM.CurrentFileInfo = new Models.AudioFileInfo
            {
                SourceType = "录音",
                FileName = Path.GetFileName(filePath),
                FilePath = filePath,
                Duration = reader.TotalTime,
                SampleRate = reader.WaveFormat.SampleRate,
                Channels = reader.WaveFormat.Channels,
                BitRate = reader.WaveFormat.BitsPerSample * reader.WaveFormat.SampleRate * reader.WaveFormat.Channels,
                Format = reader.WaveFormat.Encoding.ToString()
            };
        }

        // 刷新设备添加日志
        public void RefreshDevices()
        {
            LoadAudioDevices();
            Logger.AddLog("刷新音频设备列表", LogLevel.Info);

            if (AudioDevices.Count == 0)
            {
                Logger.AddLog("未检测到任何音频设备", LogLevel.Warning);
            }
        }

    }
}
