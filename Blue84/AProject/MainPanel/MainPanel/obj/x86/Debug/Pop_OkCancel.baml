   M S B A M L   `   `   `  ����  	MainPanel    MainPanel.Pop_OkCancel   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -   5          �Name$  
MainWindow��5      $	��480��5      $	��800��6    � WindowStyle$
 None=�6'    � 
ResizeMode$ NoResize=�6:    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6�    � Topmost . ��6j   Z�  � 	Resources 5         DownButtonStyle �� 
TargetType �� Property	 �� Value
 �� 
TargetType � 	Resources  Key �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value ��Name �� RadiusY �� RadiusX J� Opacity �� EndPoint �� 
StartPoint � Color � Offset J� SnapsToDevicePixels �� Triggers :� RoutedEvent �� BeginStoryboardName ��Name  OnMouseEnter1 P� Property P� Value��%�  &	        7  �� 5	      " ��6"   .�5
      ��  � ��
	 �����6"   �� 5       � ��$	 19(�6    �� 5       � ��	 5      �� 5      "
 ��6    5      ���� 5   	   OnMouseEnter1 .��5   
   U� $ (UIElement.Opacity)�6(   $ 	rectangle��6X   .\�5        $ 0:0:0.2��6    $ 1Q�62   5      5   
   5   	   .R�5      � $��0,1,0,2q�6   .+�5   	   �� $ 	rectangle��6   $ 4��6&   $ 2��62   $ 0Q�6>   ��5   
   �� 5      $ 0.5,1&�6   $ 0.5,0&�60   .��5      � $ 	#FF676F78��6   $ 0.017Q�6+   � 5      $ 	#FF0054C4��6   $ 1Q�6+   5      5      �� 5   	   #��z��6   ��. # z 6x   #��z��6�   5   	    5       :� 5!   	   $ Mouse.MouseLeave��6   .[�5"   
   �� $# OnMouseEnter1_BeginStoryboard��6   5#   
   :� 5$   	   $ Mouse.MouseEnter��6   .[�5%   
   �� $# OnMouseEnter1_BeginStoryboard��6   8��  6A   5&   
   P� 5'   	    �@J�	IsFocused . 6&   P� 5(   	    �@��IsDefaulted . 6(   P� 5)   	    �@J�IsMouseOver . 6(   P� 5*   	    � �� . 6&   P� 5+   	    � {� .  6&   5-      5/      50      51      .��52      � -   5         �Name$ OverLay��52      ���	#B2000000.+�53      � $	��280��6   $��Center=�6   $��Center=�65   $	��200��6P     � ColumnDefinitions  54      �� 55   	   $
��0.5* �6   �� 56   	   $
��0.5* �6   ! � RowDefinitions! 58      �� 59   	   $��0.73* �6   �� 5:   	   $��0.27* �6   .+�5<   
   �� -   5        $ 	rectangle��5<      
�������$ 4��66   $ 4��6B   $	��1.8��6N   ���	#B3000000$��2��6u   $��2��6�   6�   �� 5=      -   5        " ��Name$
" line��5=      	# �� X2$	# 280��6   	$ �� Y1$	$ 146��6#   $	��1.8��6�   $��2��6�   $��2��6�   	% �� Y2% 6,   �� *Y1+& �� ElementName$
& line����6P   �� *Stroke+$& 	rectangle��6�   �� 5>      -   5        $" line2��5>      $	��1.8��6q   $	$ 146��6�   $	% 215��6�   	' �� X1$	' 141��6�   $��2��6�   $��2��6�   ��6   �� *Stroke+$& 	rectangle��# 6L   �� *X1+$& line2��6�   �� 5?      -   5        ( ��Name$( CC_Main��5?      $��2��6'   $��3,3,3,0q�6;   
�������$��18.667(�6_   $��Center=�6q   $��Center=�6�   .��5@   	   �� -   5        ) ��Name$) TBK_Content��5@      $��Center=�6(   $��Center=�6E   $	��267��6`   $
��Wrap=�6o   5B      5C      �� 5D      -   5        * ��Name$* BTN_Ok��5D      $��确定��6   $��1��6+   #���   68   $��黑体)�6c   $��2,0,0,0q�6s   �� 5E      -	   5        $* 
BTN_Cancel��5E      $��取消��6#   $��1��60   $��1��6=   #���   6M   $	��2,0q�6w   6�   5F      5G      5H      