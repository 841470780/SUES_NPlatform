using System.Windows.Controls; 
using NVHplatform.ViewModels;  
using System.Windows;          

namespace NVHplatform.Views
{
    public partial class AnalysisView : UserControl
    {
        public AnalysisView()
        {
            InitializeComponent();                 
            this.DataContext = new AnalysisPageViewModel(); 
        }
    }
}
