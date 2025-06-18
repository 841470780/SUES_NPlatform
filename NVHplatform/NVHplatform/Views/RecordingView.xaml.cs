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
        public RecordingView()
        {
            InitializeComponent();
        }

        public RecordingView(RecordingViewModel recordingVM)
        {
            DataContext = recordingVM;
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
