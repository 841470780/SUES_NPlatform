using CommunityToolkit.Mvvm.ComponentModel;
using NVHplatform.Views;

namespace NVHplatform.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public WaveformChartViewModel WaveformVM { get; }
        public SpectrumChartViewModel SpectrumVM { get; }
        public FluctuationChartViewModel FluctuationVM { get; }

        public RecordingViewModel RecordingVM { get; }

        public object CurrentContent { get; set; }

        public MainWindowViewModel()
        {
            WaveformVM = new WaveformChartViewModel();
            SpectrumVM = new SpectrumChartViewModel();
            FluctuationVM = new FluctuationChartViewModel();

            // 传入参数，正确实例化 RecordingViewModel
            RecordingVM = new RecordingViewModel(WaveformVM, SpectrumVM, FluctuationVM);

            // 若后续 ChartsView 也要用这些 VM，可传递 DataContext
            CurrentContent = new ChartsView() { DataContext = this };
        }

    }
}
