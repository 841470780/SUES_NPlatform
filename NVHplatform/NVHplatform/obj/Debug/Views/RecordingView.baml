   M S B A M L   `   `   `  ����F  BNVHplatform, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:NVHplatform.ViewModelsNVHplatform.ViewModels  5      B >AduSkin, Version=1.1.1.9, Culture=neutral, PublicKeyToken=nullP5clr-namespace:AduSkin.Controls.Metro;assembly=AduSkinAduSkin.Controls.Metro 5      5clr-namespace:NVHplatform.ViewsNVHplatform.Views  5      5clr-namespace:NVHplatform.ViewsNVHplatform.Views  5	      %    NVHplatform.Views.RecordingView   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      3vm$clr-namespace:NVHplatform.ViewModels     5      IAduSkin5clr-namespace:AduSkin.Controls.Metro;assembly=AduSkin     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      1viewsclr-namespace:NVHplatform.Views     5      1localclr-namespace:NVHplatform.Views     5	        � 	Resources  5         VerticalGradientBrush �� 
StartPoint �� EndPoint � Color � Offset��%�   &	        �� 5   
   $	 0,0&�6<   $	 0,1&�6M   .��5      � $ 	#FFB0BEC5��6   $	 0.1Q�6-   � 5      $ 	#FFECEFF1��6   $	 0.5Q�6-   � 5      $ 	#FFB0BEC5��6   $	 1.0Q�6-   5      5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��3.5* �6   $	��115��6,   �� 5      $��10 �6   �� 5      $
��6.5* �6   $	��400��6,   .+�5   
   �� ���#D3D8D9$	��1.5q�6'    �� CornerRadius$ 15��6=    �� Padding$ 10q�6O   $��15,15,10,15q�6\   ._�5      � $��20,10,20,10q�6   $��0��6)    � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5       $
��Auto �6$   �� 5!      $
��Auto �6$   �� 5"      $
��Auto �6$   �� 5#      $��* �6$   $	��210��6/   �� 5$      $
��Auto �6$   .+�5(      �� $��0��6   $��输入音频录制��6)   $��24(�5)      $
��Bold"�6*   $��方正粗黑宋简体)�6<   $��Center=�5*      $��0,0,0,20q�69   (  "AduSkin.Controls.Metro.AduGroupBox  5-      $��1��6&   $��音频输入设备��63   ���#F9FBFC.��5.      � $��20,10,20,30q�6    5/      �� 50      $��4* �6/   �� 51      $
��1.5* �6/   .+�54      �� $��0��6#   	 �� DisplayMemberPath$	 ProductName��56   #   ��55   #   �� *AudioDevices+
 �� SelectedIndex
 57   #   �� *SelectedDeviceIndex+�� 59      $��1��6!   $��刷新设备��5:   !   $��10q�5;   !   $��10,0,0,0q�5<   !   ��5=   !   �� *RefreshDevicesCommand+5>      5?      �� 5B      $��2��6    �� Orientation$ 
Horizontal=�6*   $��0,15,0,0q�6C   $��Center=�6U   .+�5C      �� $��录音音量：��6    $��14(�6-   ���#5D7A99$��Center=�6P   $��0,0,10,0q�6k   �� 5D       � Minimum$ 0Q�6    � Maximum$	 100Q�6)   $	��200��6Y   $��16��6e    � Value 67   �� *RecordingVolume+6e   �� 5E      ���#3E588F$��Center=�5G      $��10,0,0,0q�68   ��5E       �� *RecordingVolume+ �� StringFormat$  {0:F0}%��5G   8   5H      �� 5L      $��3��6   $ 
Horizontal=�6*   $��Center=�6C   $��0,10,0,0q�6`   .+�5M      �� $��开始录音��6   $	��120��5N      $��40��6%   $��10q�61   ��5O      �� *StartRecordingCommand+�� 5P      $��停止录音��6   $	��120��5Q      $��40��6%   $��10q�61   ��5R      �� *StopRecordingCommand+5S        5W      $��4��6&   $��录音状态��63   $��0,5,0,0q�6A   ���#F9FBFC.��5X      � $��10q�6    5Y      �� 5Z      $
��Auto �6,   �� 5[      $
��Auto �6,   �� 5\      $��* �6,   .+�5_      �� $��0��6$   $��16(�6M   $��宋体)�6[   $��5,0,5,10q�6k   ��61   �� *
StatusText+6k   � 5a      $��1��6   $��5q�6,    5b      �� 5c   "   $
��Auto �63   �� 5d   "   $��* �63   �� 5e   "   $
��Auto �63   .+�5h      �� $��当前音量：��6(   $��Center=�65   $��0,0,10,0q�6P   � 5i      $��20��6*   $ 0Q�66   $	 100Q�6B   $��1��6n    6P   �� *VolumeLevel+6n   �� 5j      $��Center=�5k   (   $��2��6C   $
��10,0q�6S   ��5j   (   �� *VolumeLevel+$  {0:F0}%��5k   S   5l         NVHplatform.Views.LogView  5n      $��2��6G    � DataContext 6(   �� *Logger+6G   5p      5q      %   NVHplatform.Views.AudioFileInfo  5s      $��5��6&   $��0,15,0,0q�63    5t      �� *AudioFileInfoVM+5u      5v      �� 5x   
   $��1��6   $��2��5y   	   $
��0,15q�5z   	   #��[  5{   	   $ 1��5|   	   $��Center=�5}   	   � 5   
   $��2��6    5�      �� 5�      $��* �6    .+�5�      "   NVHplatform.Views.ChartsView  $��0��6    6,   �� *
ChartsVM+5�      5�      5�      