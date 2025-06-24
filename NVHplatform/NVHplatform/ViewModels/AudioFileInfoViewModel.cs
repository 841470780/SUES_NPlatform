using CommunityToolkit.Mvvm.ComponentModel;
using NAudio.Wave;
using NVHplatform.Models;
using System;

public class AudioFileInfoViewModel : ObservableObject
{
    private AudioFileInfo _currentFileInfo;
    public AudioFileInfo CurrentFileInfo
    {
        get => _currentFileInfo;
        set => SetProperty(ref _currentFileInfo, value);
    }

    /// <summary>
    /// 加载音频文件信息（适用于导入/录音后的文件）
    /// </summary>
    /// <param name="filePath">音频文件完整路径</param>
    /// <param name="sourceType">来源类型：“录音”或“导入”</param>
    public void LoadFileInfo(string filePath, string sourceType)
    {
        // 可根据不同格式选择不同reader，如MP3、WAV
        using var reader = new AudioFileReader(filePath);
        CurrentFileInfo = new AudioFileInfo
        {
            SourceType = sourceType,
            FileName = System.IO.Path.GetFileName(filePath),
            FilePath = filePath,
            Duration = reader.TotalTime,
            SampleRate = reader.WaveFormat.SampleRate,
            Channels = reader.WaveFormat.Channels,
            BitRate = reader.WaveFormat.BitsPerSample * reader.WaveFormat.SampleRate * reader.WaveFormat.Channels,
            Format = reader.WaveFormat.Encoding.ToString()
        };
    }
}
