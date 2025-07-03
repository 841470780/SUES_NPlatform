using System.Windows;

namespace NVHplatform.Views
{
    public partial class ExportFormatDialog : Window
    {
        public enum ExportFormat
        {
            None,
            PNG,
            MAT,
            CSV
        }

        public ExportFormat SelectedFormat { get; private set; } = ExportFormat.None;

        public ExportFormatDialog()
        {
            InitializeComponent();
        }

        private void Png_Click(object sender, RoutedEventArgs e)
        {
            SelectedFormat = ExportFormat.PNG;
            DialogResult = true;
        }

        private void Mat_Click(object sender, RoutedEventArgs e)
        {
            SelectedFormat = ExportFormat.MAT;
            DialogResult = true;
        }

        private void Csv_Click(object sender, RoutedEventArgs e)
        {
            SelectedFormat = ExportFormat.CSV;
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
