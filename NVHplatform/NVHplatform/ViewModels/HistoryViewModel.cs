using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace NVHplatform.ViewModels
{
    public class HistoryViewModel : ObservableObject
    {
        private bool isOrganizeMode;
        public bool IsOrganizeMode
        {
            get => isOrganizeMode;
            set => SetProperty(ref isOrganizeMode, value);
        }

        public IRelayCommand EnterOrganizeModeCommand { get; }
        public IRelayCommand ExitOrganizeModeCommand { get; }

        public HistoryViewModel()
        {
            EnterOrganizeModeCommand = new RelayCommand(EnterOrganizeMode);
            ExitOrganizeModeCommand = new RelayCommand(ExitOrganizeMode);
        }

        private void EnterOrganizeMode() => IsOrganizeMode = true;
        private void ExitOrganizeMode() => IsOrganizeMode = false;
    }
}
