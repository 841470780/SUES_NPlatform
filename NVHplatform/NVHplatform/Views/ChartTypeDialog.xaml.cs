using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static NVHplatform.ViewModels.ChartsViewModel;

namespace NVHplatform.Views
{
    /// <summary>
    /// ChartTypeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ChartTypeDialog : Window
    {
        public ChartType? SelectedType { get; private set; }
        public ChartTypeDialog()
        {
            InitializeComponent();
        }

        private void Waveform_Click(object sender, RoutedEventArgs e)
        {
            SelectedType = ChartType.Waveform;
            DialogResult = true;
        }
        private void Spectrum_Click(object sender, RoutedEventArgs e)
        {
            SelectedType = ChartType.Spectrum;
            DialogResult = true;
        }
        private void Fluctuation_Click(object sender, RoutedEventArgs e)
        {
            SelectedType = ChartType.Fluctuation;
            DialogResult = true;
        }
    }

}
