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
        }

        public void ReceiveWaveformSamples(float[] samples)
        {
            ViewModel.UpdateWaveform(samples);
        }
    }
}
