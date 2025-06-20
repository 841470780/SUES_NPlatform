// ViewModels/LogViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using NVHplatform.Models;
using System;
using System.Collections.ObjectModel;

namespace NVHplatform.ViewModels
{
    public class LogViewModel : ObservableObject
    {
        public ObservableCollection<LogMessage> LogMessages { get; } = new ObservableCollection<LogMessage>();

        public void AddLog(string message, LogLevel level = LogLevel.Info)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                LogMessages.Add(new LogMessage
                {
                    Timestamp = DateTime.Now,
                    Content = $"[{DateTime.Now:HH:mm:ss}] [{level}] {message}",
                    Level = level
                });
            });
        }
    }
}
