   M S B A M L   `   `   `  ����g  cLiveChartsCore.SkiaSharpView.WPF, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc92a3e13d40c573}Xclr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF LiveChartsCore.SkiaSharpView.WPF  5      F BNVHplatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:NVHplatform.ViewModelsNVHplatform.ViewModels 5      '   !NVHplatform.Views.HistoryMainView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      hlvcXclr-namespace:LiveChartsCore.SkiaSharpView.WPF;assembly=LiveChartsCore.SkiaSharpView.WPF     5      3vm$clr-namespace:NVHplatform.ViewModels     5      .��5      � $��20q�6     � ColumnDefinitions  5   
   �� 5      $
��2.2* �6   �� 5      $
��0.3* �6   �� 5      $
��5.5* �6   .+�5   
   � $��0��6   $��0,0,10,0q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    .+�5      �� ���#DDD$��1q�6(    �� CornerRadius$ 6��6<    �� Padding$ 10q�6M   ���#FAFAFA$��0 0 0 10q�6o   ._�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   .+�5      �� $��整理文件��6   $��90��5      $��30��6(   $��FileOrganize��5!      ��5       �� *DataContext.NavigateCommand+ �� RelativeSource ��  �� AncestorType$ WindowL�5!      5"      5#      �� 5&      $��1��6   $��录音文件列表��6$   $
��Bold"�64   $��0,10,0,0q�6F   .��5'      �� $
��Auto=�6   .��5(      ��  �� DisplayMemberPath$ FileName��5*      $��10q�5+      $	��300��5,      ��5(      �� *AudioFileHistory+ �� SelectedItem 5)      �� *SelectedFile+	 �� Mode$	 TwoWay=�5,      5-      5.      5/      �� 54   
   $��1��6   $��8��6'   $��Stretch=�61   $��Stretch=�6O   ���#EEE6k   � 57   
   $��2��6    58      �� 59      $
��Auto �6    �� 5:      $��* �6    .+�5>      �� ���#CCC$��1q�6(   $ 6��6<   $ 10q�6M   $��0 0 0 10q�6Z   ���#F9F9F9._�5?      �� $ Vertical=�6   .+�5@      �� $��16(�6G   $
��Bold"�6U   ��6    �� *SelectedFile.FileName+6U   �� 5A      $��12(�6G   
�������$��CharacterEllipsis=�6g   ��6    �� *SelectedFile.FilePath+6g   � 5B      $��0,8,0,0q�6    .+�5C      �� $��采样率：��6$   $
��Bold"�60   �� 5D      $	��5,0q�6M   ��6$   �� *SelectedFile.SampleRate+6M   �� 5E      $��Hz    ��6$   $
��15,0q�62   �� 5F      $��通道数：��6$   $
��Bold"�60   �� 5G      $	��5,0q�6K   ��6$   �� *SelectedFile.Channels+6K   �� 5H      $��	时长：��6$   $
��Bold"�6/   $
��15,0q�6A   �� 5I      $	��5,0q�6K   ��6$   �� *SelectedFile.Duration+6K   5J      5K      5L      �� 5O      $��1��6   .@�5P      �� $��	波形图��6   .��5Q      5   /LiveChartsCore.SkiaSharpView.WPF.CartesianChart  
   Series
 6)   �� *WaveformSeries+5R      �� 5S      $��	频谱图��6   .��5T        
 6)   �� *SpectrumSeries+5U      �� 5V      $��波动强度��6   .��5W        
 6)   �� *FluctuationSeries+5X      5Y      5Z      5[      5\      