   M S B A M L   `   `   `  ���� L  HSrodLauncher v2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:SrodLauncher_v2._0.MVVM.ViewSrodLauncher_v2._0.MVVM.View  0    *SrodLauncher_v2._0.MVVM.View.MinecraftView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     <local*clr-namespace:SrodLauncher_v2._0.MVVM.View       � 	Resources     StyledComboBox �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius d�Name �� 	ClickMode	 d� 	IsChecked
 �� Mode �� RelativeSource � ColumnDefinitions �� SelectionBoxItem �� SelectionBoxItemTemplate �Name �� IsDropDownOpen � AllowsTransparency �� Triggers P� Property P� Value �� 
TargetName �� Padding��%�  &	        �� " ��.���  � �� �#F5F5F5��  � �� �#FFC7EF��  � ��$ 2q���  � ��
 �   ���  � ��$ 20(���  � ��$ 5q���  � ��$ 39����  � ��$	 150����  � ��$ Center=���  � �� �� " ��.R�� .+��� $ 
MainBorder��#��z��#��z��#��z��$ 5��._�� .+�d� $ ToggleButton��$��0q�
������ $ Press=�	 �� *IsDropDownOpen+$
 TwoWay=� �� *TemplatedParent+.���  �� $��* ��� $
��Auto �.+��� $��0��#��z #��z $��Stretch=�$��Center=�#��z��F� $��1��"����        � 	=  	= � z     	
���   �$��Center=�$��Center=�� $ Popup��#��z $��Bottom=� . .���� 
����������#FFC7EF$��2q�$ 5��$	��185��._��� .����  P�  �@J�IsMouseOver . . ��� $ 
MainBorder�� � �� �	#FFB0D8FFP�  �@��IsDropDownOpen . . ��� $ 
MainBorder�� � �� �	#FFB0D8FF��  � �� �� " ��.���  � ��
 �������  � ��$ 5q���  � �� �� " ��.R��� #��z��# z��._���  P�  �@J�IsMouseOver . . ���  � �� �	#FFE6E6E6P�  �@��
IsSelected . . ���  � �� �#FFC7EF��  � ��
 �����.���  � RowDefinitions �� $��* ��� $	��100 �.+��  �� $��* �.+��� $��Center=��� -    ��Name$ loadingText��$��1��$	��500��$��30��$��20,0,0,0q�$��Center=�$��17(�$��Center=�$y�Hidden=�$��
Loading...���� -    ��Name$ 
playButton��$
��Play��$��38(�  ButtonsTheme#��[ $��1��$	��200��$��60��$��Center=�$��0,0,20,0q��� -    ��Name$ versionSelect��$��1��$��0,0,20,0q�$��Center=�$��Right=�#��[  $��Stretch=�$��Center=�