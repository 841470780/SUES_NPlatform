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
    }
}
