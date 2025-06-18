using System.Windows.Controls;
using NVHplatform.ViewModels;

namespace NVHplatform.Views
{
    public partial class WaveformChartView : UserControl
    {
        public WaveformChartViewModel ViewModel { get; set; }

        public WaveformChartView()
        {
            InitializeComponent();
            ViewModel = new WaveformChartViewModel();
            DataContext = ViewModel;
        }

        public void ReceiveWaveformSamples(float[] samples)
        {
            ViewModel.UpdateWaveform(samples);
        }
    }
}
