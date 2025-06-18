using MahApps.Metro.Controls;
using NVHplatform.ViewModels;

namespace NVHplatform.Views
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}
