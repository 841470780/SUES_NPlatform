using CommunityToolkit.Mvvm.Input;
using NVHplatform.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NVHplatform.Views
{
    public partial class RecordingView : UserControl
    {
        public LogViewModel Logger { get; } = new LogViewModel();

        public RecordingView()
        {
            InitializeComponent();

            // 设置默认 DataContext，如果使用 MVVM 注入则可删除
            if (DataContext is RecordingViewModel vm)
            {
                HookCommands(vm);
            }
            else
            {
                Loaded += (s, e) =>
                {
                    if (DataContext is RecordingViewModel loadedVm)
                        HookCommands(loadedVm);
                };
            }
        }

        public RecordingView(RecordingViewModel recordingVM)
        {
            InitializeComponent();
            DataContext = recordingVM;
            HookCommands(recordingVM);
        }

        private void HookCommands(RecordingViewModel vm)
        {
            vm.StartRecordingCommand.NotifyCanExecuteChanged();
            vm.StopRecordingCommand.NotifyCanExecuteChanged();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                var vm = DataContext as NVHplatform.ViewModels.RecordingViewModel;
                if (vm == null) return;

                var path = vm.RecordingSavePath;
                if (Directory.Exists(path))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = $"\"{path}\"",
                        UseShellExecute = true
                    });
                }
                else
                {
                    System.Windows.MessageBox.Show("目录不存在：" + path);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("打开目录失败：" + ex.Message);
            }

            e.Handled = true;
        }
    }
}
