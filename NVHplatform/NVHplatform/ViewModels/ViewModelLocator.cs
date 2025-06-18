using CommunityToolkit.Mvvm.ComponentModel;

namespace NVHplatform.ViewModel
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => new MainViewModel();
    }

    public class MainViewModel : ObservableObject
    {
        // 这里放你自己的属性和命令
    }
}
