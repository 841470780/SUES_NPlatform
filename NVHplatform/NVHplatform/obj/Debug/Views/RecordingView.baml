   M S B A M L   `   `   `  ����F  BNVHplatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:NVHplatform.ViewModelsNVHplatform.ViewModels  5      B >AduSkin, Version=1.1.1.9, Culture=neutral, PublicKeyToken=nullP5clr-namespace:AduSkin.Controls.Metro;assembly=AduSkinAduSkin.Controls.Metro 5      5clr-namespace:NVHplatform.ViewsNVHplatform.Views  5      5clr-namespace:NVHplatform.ViewsNVHplatform.Views  5	      %    NVHplatform.Views.RecordingView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3vm$clr-namespace:NVHplatform.ViewModels     5      IAduSkin5clr-namespace:AduSkin.Controls.Metro;assembly=AduSkin     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      1viewsclr-namespace:NVHplatform.Views     5      1localclr-namespace:NVHplatform.Views     5	      .��5      �   � ColumnDefinitions  5      �� 5      $��4* �6#   $	��115��6.   �� 5      $��10 �6#   �� 5      $��6* �6#   $	��400��6.   .+�5   
   �� ���#D3D8D9$	��1.5q�6'    �� CornerRadius$ 15��6=    �� Padding$ 10q�6O   $��15,15,10,15q�6\   ._�5      � $��20,10,20,10q�6   $��0��6)    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $��* �6$   �� 5      $
��Auto �6$   .+�5      �� $��0��6   $��输入音频录制��6)   $��24(�5      $
��Bold"�6*   $��方正粗黑宋简体)�6<   $��Center=�5       $��0,0,0,20q�69   (  "AduSkin.Controls.Metro.AduGroupBox  5#      $��1��6&   $��音频输入设备��63   
�������.��5$      � $��20,10,20,30q�6     5%      �� 5&      $��4* �6/   �� 5'      $
��1.5* �6/   .+�5*      �� $��0��6#    �� DisplayMemberPath$ ProductName��5,   #   ��5+   #   �� *AudioDevices+ �� SelectedIndex 5-   #   �� *SelectedDeviceIndex+�� 5/      $��1��6!   $��刷新设备��50   !   $��10q�51   !   $��10,0,0,0q�52   !   ��53   !   �� *RefreshDevicesCommand+54      55      �� 58      $��2��6    �� Orientation$ 
Horizontal=�6*   $��0,10,0,0q�6C   $��Center=�6U   .+�59      �� $��录音音量：��6    $��14(�6-   $��Center=�6;   $��0,0,10,0q�6V   �� 5:       � Minimum$ 0Q�6    � Maximum$	 100Q�6)   $	��200��6Y   	 � Value	 67   �� *RecordingVolume+6Y   �� 5;      $��Center=�5<       $��10,0,0,0q�6;   ��5;       �� *RecordingVolume+
 �� StringFormat$
  {0:F0}%��5<   ;   5=      �� 5@      $��3��6   $ 
Horizontal=�6*   $��Center=�6C   $��0,10,0,0q�6`   .+�5A      �� $��开始录音��6   $	��120��5B      $��40��6%   $��10q�61   ��5C      �� *StartRecordingCommand+�� 5D      $��停止录音��6   $	��120��5E      $��40��6%   $��10q�61   ��5F      �� *StopRecordingCommand+5G        5K      $��4��6&   $��录音状态��63   $��0,5,0,0q�6A   
�������.��5L      � $��10q�6    5M      �� 5N      $
��Auto �6,   �� 5O      $
��Auto �6,   �� 5P      $��* �6,   .+�5S      �� $��0��6$   $��16(�6M   $��宋体)�6[   $��5,0,5,10q�6k   ��61   �� *
StatusText+6k   � 5U      $��1��6   $��5q�6,     5V      �� 5W   "   $
��Auto �63   �� 5X   "   $��* �63   �� 5Y   "   $
��Auto �63   .+�5\      �� $��当前音量：��6(   $��Center=�65   $��0,0,10,0q�6P   � 5]      $��20��6*   $ 0Q�66   $	 100Q�6B   $��1��6n   	 6P   �� *VolumeLevel+6n   �� 5^      $��Center=�5_   (   $��2��6C   $
��10,0q�6S   ��5^   (   �� *VolumeLevel+$
  {0:F0}%��5_   S   5`         NVHplatform.Views.LogView  5b      $��2��6G    � DataContext 6(   �� *Logger+6G   5d      5e      �� 5h      $��5��6   $
��Left=�6)   $
��0,10q�6D   �� 5i      -   5        $��.?�5i       .�6\   .�5j      �� �6   �� *RecordingSavePath+ �� Mode$ OneWay=�$!
 录音文件保存在：{0}��5k      5l      5m      5n      � 5p   
   $��2��6    5q      �� 5r      $��* �6$   .+�5u      "   NVHplatform.Views.ChartsView  $��0��6#    60   �� 5v      5w      5y      