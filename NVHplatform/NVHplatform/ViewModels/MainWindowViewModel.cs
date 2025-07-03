using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NVHplatform.Views;
using Prism.Dialogs;
using System.Collections.ObjectModel;
using System.Windows;  // 用于 MessageBox
using NVHplatform.ViewModels;

namespace NVHplatform.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool autoStart;

        [ObservableProperty]
        private bool enableLogging;

        [ObservableProperty]
        private string defaultExportPath;

        [ObservableProperty]
        private ObservableCollection<string> chartFormats = new ObservableCollection<string> { "PNG", "JPG", "SVG" };

        [ObservableProperty]
        private string selectedChartFormat;  // ✅ 保留这一处即可

        // 命令
        public IRelayCommand BrowsePathCommand { get; }
        public IRelayCommand SaveSettingsCommand { get; }

        private string currentPageName;
        public string CurrentPageName
        {
            get => currentPageName;
            set => SetProperty(ref currentPageName, value);
        }

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
        public HistoryViewModel HistoryVM { get; }

        // View 缓存（只实例化一次）
        private readonly RecordingView recordingView;
        private readonly AnalysisView analysisView;
        private readonly SettingView settingView;
        private readonly HistoryView historyView;
        private readonly FileOrganizeView fileOrganizeView;
        public MainWindowViewModel()
        {
            var chartsVM = new ChartsViewModel();
            WaveformVM = new WaveformChartViewModel();
            SpectrumVM = new SpectrumChartViewModel();
            FluctuationVM = new FluctuationChartViewModel();
            fileOrganizeView = new FileOrganizeView
            {
                DataContext = new FileOrganizeViewModel()
            };


            RecordingVM = new RecordingViewModel(chartsVM, WaveformVM, SpectrumVM, FluctuationVM);
            AnalysisVM = new AnalysisPageViewModel();
            settingView = new SettingView();

            HistoryVM = new HistoryViewModel();
            historyView = new HistoryView { DataContext = HistoryVM };

            // 实例化页面视图并绑定 ViewModel
            recordingView = new RecordingView { DataContext = RecordingVM };
            analysisView = new AnalysisView { DataContext = AnalysisVM };

            NavigateCommand = new RelayCommand<string>(Navigate);

            // 默认页面
            CurrentView = recordingView;
        }

        private void Navigate(string viewName)
        {
            CurrentPageName = viewName;

            switch (viewName)
            {
                case "Login":
                    CurrentView = new System.Windows.Controls.TextBlock
                    {
                        Text = "用户登录页面（待开发）",
                        FontSize = 20,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    break;
                case "AudioRecord":
                    CurrentView = recordingView;
                    break;
                case "Analysis":
                    CurrentView = analysisView;
                    break;
                case "History":
                    CurrentView = historyView;
                    break;
                case "Setting":
                    CurrentView = settingView;
                    break;
                case "FileOrganize":
                    CurrentView = fileOrganizeView;
                    break;
            }
        }

    }
}
