using NVHplatform.ViewModels; // 替换为你实际的 ViewModel 命名空间
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace NVHplatform.Views
{
    public partial class AudioFileInfo : UserControl
    {
        public AudioFileInfo()
        {
            InitializeComponent();
            this.DataContext = new AudioFileInfoViewModel();
        }

        private void FilePath_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var filePath = e.Uri.LocalPath;
            if (File.Exists(filePath))
            {
                Process.Start("explorer.exe", $"/select,\"{filePath}\"");
            }
            else if (Directory.Exists(filePath))
            {
                Process.Start("explorer.exe", $"\"{filePath}\"");
            }
            else
            {
                System.Windows.MessageBox.Show("路径不存在：" + filePath);
            }
            e.Handled = true;
        }
    }
}
