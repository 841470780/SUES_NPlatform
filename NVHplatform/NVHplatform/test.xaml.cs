using System.Windows;
using NAudio.Wave;

namespace NVHplatform
{
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeviceList.Items.Clear();
            int count = WaveIn.DeviceCount;
            if (count == 0)
            {
                DeviceList.Items.Add("未检测到音频输入设备！");
                return;
            }
            for (int i = 0; i < count; i++)
            {
                var cap = WaveIn.GetCapabilities(i);
                DeviceList.Items.Add($"[{i}] {cap.ProductName}");
            }
        }
    }
}
