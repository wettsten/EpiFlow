   M S B A M L   `   `   `  ����  EpiFlow.ManualUI#    EpiFlow.ManualUI.LookupWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QQuickConverter, Version=1.2.3.0, Culture=neutral, PublicKeyToken=9c892aa7bc2af2cf,qc#http://QuickConverter.CodePlex.com/  5   	   -   5          �Name$  _this��5       � Title$ Lookup Series��5   	   $	��600��6   $	��800��6,    � 	Resources 5	         
LinkButton �� 
TargetType �� Property �� Value �� 
TargetType  QuickConverter.Binding   P �� Path	 �� Triggers
 P� Property P� Value  episodeTemplate  seasonTemplate �� ItemTemplate �� ItemsSource  seriesTemplate��%�  &	        &	     7   &	 g    7  7   &	 �    7  7   �� 5
   
   " ��6#   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� �� 5      5      5      5      �� 5       � {� 6*     *!$P+ �� $ 
IsSelected��� 5       � ��$
 Bold"�6+   �� 5       � ��$ 18(�6)   �� 5       � ��
 ��  �6+   	 5      P� 5      
 �@J�IsMouseOver . . �5      ��  � ��
 �  ��63   5      P� 5      
 � {� .  . �5      ��  � ��
 � ���63   5      5       �� 5!   
   .R�5"      �� -   5        8��  ��5"      �� *
ItemString+6X   5#      �� 5$   
   8    6:   �� *
Episodes+.R�5%      �� -   5        8�� ��5%      �� *
ItemString+6X   5&      �� 5'   
   8    6:   �� *	Seasons+.R�5(      �� -   5        8�� ��5(      �� *
ItemString+6X   5)      5*      .��5+      ]� ��. .+�5,   
   �� $	��Top=�6    �� Padding$ 10q�6&   ._�5-      ]� ��. .+�5.      �� $
��Left=�6   .��6.   	Search:67   �� 5/      -   5        $��Right=�5/      $��75��60   .��6P   Search6X   �� 50      -   5         ��Name$ 
searchText��50      $��Center=�6.   $��	10,0,10,0q�6I   51      52      �� 53   
   $
��Left=�6   $ 5q�6'   ._�54      �� .+�55      �� #��[  6Y   ��6   �� $ SelectedSeries.SeriesName� �� Mode$ TwoWay=�6Y   �� 56      $��1q�6   
���   �$ 5q�6A   $��0,0,0,10q�6M   ._�57      �� ��. $��WrapWithOverflow=�58       $	��200��6@   $	��200��6L   ��6Y   ��  �� ElementName$ _this��$ SelectedSeries.Overview�$ TwoWay=�59      �� 5:      .��6   Episode Description:6.   �� 5;      $��1q�6   
���   �$ 5q�6A   ._�5<      �� ��. $��WrapWithOverflow=�5=       $	��200��6@   $	��200��6L   ��6Y   �� $ _this��$ SelectedEpisode.Overview�$ TwoWay=�5>      5?      5@      �� 5A   
   $��1q�6   
���   �$ 5q�69   ._�5B      R� -   5        $��5q�5B      #��[ 6]   ��6#   �� $ _this��$ 	AllSeries���5C      �� 5D      " Q�6   .�5E      ��  �@Q�
IsSelected 67   �� *
IsSelected+�� 5F       �@Q�
IsExpanded 67   �� *
IsExpanded+5G      5I      5J      5K      5L      