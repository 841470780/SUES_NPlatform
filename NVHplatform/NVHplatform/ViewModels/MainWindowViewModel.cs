using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NVHplatform.Views;

namespace NVHplatform.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private object currentView;
        public object CurrentView
        {
            get => currentView;
            set => SetProperty(ref currentView, value);
        }

        public IRelayCommand NavigateCommand { get; }

        // 图表子 ViewModels
        public WaveformChartViewModel WaveformVM { get; }
        public SpectrumChartViewModel SpectrumVM { get; }
        public FluctuationChartViewModel FluctuationVM { get; }

        // 主页面 ViewModels
        public RecordingViewModel RecordingVM { get; }
        public AnalysisPageViewModel AnalysisVM { get; }

        // View 缓存（只实例化一次）
        private readonly RecordingView recordingView;
        private readonly AnalysisView analysisView;

        public MainWindowViewModel()
        {
            var chartsVM = new ChartsViewModel();
            WaveformVM = new WaveformChartViewModel();
            SpectrumVM = new SpectrumChartViewModel();
            FluctuationVM = new FluctuationChartViewModel();

            RecordingVM = new RecordingViewModel(chartsVM, WaveformVM, SpectrumVM, FluctuationVM);
            AnalysisVM = new AnalysisPageViewModel();

            // 只实例化一次页面视图，并绑定 ViewModel
            recordingView = new RecordingView { DataContext = RecordingVM };
            analysisView = new AnalysisView { DataContext = AnalysisVM };

            NavigateCommand = new RelayCommand<string>(Navigate);

            // 设置默认页
            CurrentView = recordingView;
        }

        private void Navigate(string viewName)
        {
            switch (viewName)
            {
                case "Login":
                    CurrentView = new System.Windows.Controls.TextBlock
                    {
                        Text = "用户登录页面（待开发）",
                        FontSize = 20,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                    };
                    break;
                case "AudioRecord":
                    CurrentView = recordingView;
                    break;
                case "Analysis":
                    CurrentView = analysisView;
                    break;
                case "History":
                    CurrentView = new System.Windows.Controls.TextBlock
                    {
                        Text = "历史记录页面（待开发）",
                        FontSize = 20,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                    };
                    break;
                case "Setting":
                    CurrentView = new System.Windows.Controls.TextBlock
                    {
                        Text = "设置页面（待开发）",
                        FontSize = 20,
                        VerticalAlignment = System.Windows.VerticalAlignment.Center,
                        HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                    };
                    break;
            }
        }
    }
}
