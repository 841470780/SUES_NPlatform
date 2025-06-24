using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace NVHplatform.ViewModels
{
    public class ChartsViewModel : INotifyPropertyChanged
    {
        // 命令定义
        public IRelayCommand<ChartItem> DeleteChartCommand { get; }
        public IRelayCommand RefreshChartsCommand { get; }
        public IRelayCommand ResetChartsCommand { get; }
        public IRelayCommand ExportChartsCommand { get; }
        public IRelayCommand<ChartItem> ExportSingleChartCommand { get; }

        // 事件定义
        public event EventHandler ExportRequested;
        public event EventHandler<ChartItem> ExportSingleChartRequested;

        // 保存音频数据
        private float[] lastSamples = null;

        public enum ChartType
        {
            Waveform,
            Spectrum,
            Fluctuation
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private ObservableCollection<ChartItem> _charts = new ObservableCollection<ChartItem>();
        public ObservableCollection<ChartItem> Charts
        {
            get => _charts;
            set
            {
                if (_charts != value)
                {
                    _charts = value;
                    OnPropertyChanged(nameof(Charts));
                }
            }
        }

        public ChartsViewModel()
        {
            // 初始化默认图表
            Charts.Add(new WaveformChartItem());
            Charts.Add(new SpectrumChartItem());
            Charts.Add(new FluctuationChartItem());

            // 初始化命令
            DeleteChartCommand = new RelayCommand<ChartItem>(DeleteChart);
            RefreshChartsCommand = new RelayCommand(RefreshCharts);
            ResetChartsCommand = new RelayCommand(ResetChartsData);
            ExportChartsCommand = new RelayCommand(() => ExportRequested?.Invoke(this, EventArgs.Empty));
            ExportSingleChartCommand = new RelayCommand<ChartItem>(item =>
            {
                ExportSingleChartRequested?.Invoke(this, item);
            });
        }

        public void AddChartWithType(ChartType type)
        {
            switch (type)
            {
                case ChartType.Waveform: Charts.Add(new WaveformChartItem()); break;
                case ChartType.Spectrum: Charts.Add(new SpectrumChartItem()); break;
                case ChartType.Fluctuation: Charts.Add(new FluctuationChartItem()); break;
            }
        }

        public void RemoveChart()
        {
            if (Charts.Count > 0) Charts.RemoveAt(Charts.Count - 1);
        }

        private void DeleteChart(ChartItem item)
        {
            if (item != null && Charts.Contains(item))
                Charts.Remove(item);
        }

        public void UpdateAllCharts(float[] samples)
        {
            lastSamples = samples;
            foreach (var chart in Charts)
                chart.UpdateData(samples);
        }

        private void RefreshCharts()
        {
            if (lastSamples != null && lastSamples.Length > 0)
                UpdateAllCharts(lastSamples);
            else
                foreach (var chart in Charts)
                    chart.UpdateData(Array.Empty<float>());
        }

        private void ResetChartsData()
        {
            foreach (var chart in Charts)
                chart.ResetData();
        }

        // -------- ChartItem 基类与子类 --------
        public abstract class ChartItem : INotifyPropertyChanged
        {
            public abstract string Title { get; }
            public abstract object Series { get; }
            public abstract object XAxes { get; }
            public abstract object YAxes { get; }
            public abstract void UpdateData(float[] samples);
            public abstract void ResetData();

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class WaveformChartItem : ChartItem
        {
            public WaveformChartViewModel VM { get; } = new WaveformChartViewModel();
            public override string Title => "波形图";
            public override object Series => VM.WaveformSeries;
            public override object XAxes => VM.XAxes;
            public override object YAxes => VM.YAxes;
            public override void UpdateData(float[] samples) => VM.UpdateWaveform(samples);
            public override void ResetData() => VM.ClearWaveform();
        }

        public class SpectrumChartItem : ChartItem
        {
            public SpectrumChartViewModel VM { get; } = new SpectrumChartViewModel();
            public override string Title => "频谱图";
            public override object Series => VM.SpectrumSeries;
            public override object XAxes => VM.XAxes;
            public override object YAxes => VM.YAxes;
            public override void UpdateData(float[] samples) => VM.UpdateSpectrum(samples);
            public override void ResetData() => VM.ClearSpectrum();
        }

        public class FluctuationChartItem : ChartItem
        {
            public FluctuationChartViewModel VM { get; } = new FluctuationChartViewModel();
            public override string Title => "波动强度图";
            public override object Series => VM.FluctuationSeries;
            public override object XAxes => VM.XAxes;
            public override object YAxes => VM.YAxes;
            public override void UpdateData(float[] samples) => VM.UpdateFluctuation(samples);
            public override void ResetData() => VM.ClearFluctuation();
        }
    }
}
