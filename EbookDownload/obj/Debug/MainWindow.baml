   M S B A M L   `   `   `  ����H  DEbookDownload, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:EbookDownloadEbookDownload  5   	   9!clr-namespace:EbookDownload.viewsEbookDownload.views  5	   	       EbookDownload.MainWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:EbookDownload     5   	   H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;controls,http://metro.mahapps.com/winfx/xaml/controls  5   	   R NMahApps.Metro.IconPacks, Version=2.3.0.4, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5   	   -Views!clr-namespace:EbookDownload.views  5	   	     � Title$  电子书聚合神器��5   	   $	��680��6   $	��720��6&    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Resources/Icons.xaml?�6%   %    5      .��5      �  � ColumnDefinitions 5   
   �� 5      $
��525* �6   �� 5      $
��187* �6    � RowDefinitions 5   
   �� 5      $	��120 �6   �� 5      .+�5!   
   �� -   5         ��Name$ 
tabControl��5!      $��1��6)   $��0��66   $��
10,10,10,0q�5$      $	��Top=�6+   ��5"      ��  �� ElementName$
 grid�� �� Path$ ActualHeight���5#      �� $
 grid��$ ActualWidth���5%      �� 5'      � 5)   
   $��52,40,52,39q�6   $��0��6%   $
��Auto��65    5*      �� 5+      $
��183* �6#   �� 5,      $	��22* �6#   .+�5.      �� $��0��6   $��0,0,69,0q�6(   $
��Auto��6:   �� 5/      -   5        	 ��Name$	 textBox��5/      $��26��6'   
 �� TextWrapping$

 Wrap=�64   *  $MahApps.Metro.Controls.TextBoxHelper   ClearTextButton .    	Watermark$ Search��51      $��Bottom=�52       J� RenderTransformOrigin$ 0.5,0.5&�6,   $��6,0,1,8q�6L   $��0��6]   .��6m   z�53      V� 54      .~�55      �� �� 56       �� AngleX$ -0.006Q�6(   �� 57       �� Angle$ 0.094Q�6*   T� 58      59      .��5:   +   5;      �� 5<      $��26,0,0,0q�6   $��1��6*   $ 0.5,0.5&�6:   z�5=      V� 5>      .~�5?      ��  �� ScaleX$ -1Q�6)   �� 5@      �� 5A      T� 5B      5C      5E      d� 5F      -   5         d�Name$ toggleButton��5F      $ 5.862,-2.28&�62   $��0,0,0,8q�6V   $��26��6g   $��Bottom=�6s   $
��Left=�6�   $��26��6�   $��1��6�   .��5H      �� -   5        $��15��5H      $��15��6'   ��5I      !� 5J         appbar_book !� Visual# [  6&   5L      5M      5N      5O      5Q      