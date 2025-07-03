using System.Windows;

namespace NVHplatform.Views
{
    public partial class InputFileNameDialog : Window
    {
        public string FileName => InputBox.Text.Trim();

        public InputFileNameDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(FileName))
            {
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("文件名不能为空！");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
