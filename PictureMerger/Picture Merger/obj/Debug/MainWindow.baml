   M S B A M L   `   `   `  ����I  EPicture Merger, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Picture_MergerPicture_Merger  5   	       Picture_Merger.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Picture_Merger     5   	   X TXceed.Wpf.Toolkit, Version=2.8.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  
MainWindow��5	   	   $	��350��6   $	��525��6)   .��5
      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5      $	��10* �6   �� 5      $��60 �6   .+�5   
   � $��0��6   
���   �.+�5      �� $��Merger��6   
�������6%   �� 5      $��Number of bits used : ��6   $	��Top=�65   
�������$��	0,0,150,0q�6`   $��Right=�6s   %  Xceed.Wpf.Toolkit.IntegerUpDown  5      -   5        $
��bits��5   !   $	��top=�6-   $��Right=�6E   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 1X�6a      Maximum$ 7X�6m      Value$ 1X�6y      DefaultValue$ 1X�6�   $	��130��6�   $��0,0,10,0q�6�   $��26��6�   �� 5      
�������$��Bottom=�6'   $��Right=�6B   $��10,10,165,5q�6^   $��Seed : ��6s   �� 5      -   5        $��ChkBoxInvert��5      $��Bottom=�6+   $��Right=�6F   $��Invert Bits��6b   
�������$��10,10,10,10q�6�   �� 5      -   5        $��	SelectPic��5      $��Bottom=�6&   $
��Left=�6A   $	��130��6\   $��	10,2,10,2q�6h   $��Host Pic��6|   $��
10,0,10,10q�6�   �� 5      -   5        $��SelectHidden��5      $��Bottom=�6)   $
��Left=�6D   $	��130��6_   $��	10,2,10,2q�6k   $��
Hidden Pic��6   $��150,10,0,10q�6�   6�   5      � 5   
   $��1��6   
������� � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��10 �6#   �� 5      $��* �6#   .+�5       � $��0��6    5!      �� 5"      $��* �6$   �� 5#      $��10 �6$   �� 5$      $��* �6$   .+�5&      �� -   5        $��original��5&      $��0��6(   � 5'      -   5        $��CryptBar��5'      $��1��6.   �� 5(      -   5        $��hidden��5(      $��2��6&   5)      �� 5*      -   5        $��changed��5*      $��2��6#   5+      � 5,   
   $��2��6   
���   �.+�5-      �� -	   5        $��Encrypt��5-      $��Encrypt��6$   $	��Top=�66   $��26��6N   $	��130��6Z   $
��Left=�6f   6�   �� 5.      -
   5        $��Decrypt��5.      $��Decrypt��6$   $��Bottom=�66   $��26��6Q   $	��130��6]   $
��Left=�6i   6�   �� 5/      -   5        $
��Save��5/      $
��Save��6!   $��Right=�60   $	��130��6L   6o   50      51      52      