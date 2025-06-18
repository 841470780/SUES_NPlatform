// Views/SpectrumChartView.xaml.cs
using System.Windows.Controls;
using NVHplatform.ViewModels;

namespace NVHplatform.Views
{
    public partial class SpectrumChartView : UserControl
    {
        public SpectrumChartViewModel ViewModel { get; set; }

        public SpectrumChartView()
        {
            InitializeComponent();
            ViewModel = new SpectrumChartViewModel();
            DataContext = ViewModel;
        }

        public void ReceiveSpectrumSamples(float[] samples)
        {
            ViewModel.UpdateSpectrum(samples);
        }
    }
}
