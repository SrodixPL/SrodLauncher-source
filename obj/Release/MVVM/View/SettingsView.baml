   M S B A M L   `   `   `  ���� L  HSrodLauncher v2.0, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:SrodLauncher_v2._0.MVVM.ViewSrodLauncher_v2._0.MVVM.View  /    )SrodLauncher_v2._0.MVVM.View.SettingsView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     <local*clr-namespace:SrodLauncher_v2._0.MVVM.View       � 	Resources     CustomTextBoxStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� CornerRadius ��Name �� Triggers	 P� Property
 P� Value �� 
TargetName  SectionLabelStyle  InfoTextStyle  LoginButtonStyle J� SnapsToDevicePixels ��Name  SignOutButtonStyle  LoggedInTextStyle��%N  &	        &	 =    &	 �    &	 Z    &	 n    &	 �    �� " ��.���  � ��$ 32����  � ��$ 16(���  � ��$ 8,0,8,0q���  � ��$ Center=���  � ��$ Center=���  � �� �#F5F5F5��  � �� �#333333��  � ��$ 0q���  � �� �� " ��.R��� $ border��$ 8��#��z�����#E0E0E0$��1q�._��� $ PART_ContentHost��|�.  $��Center=�#��z��$��Hidden=�$��Hidden=� P� 	 �@J�IsMouseOver
 . . ��� $ border�� � �� �#FFC7EFP� 	 �@J�	IsFocused
 . . ��� $ border�� � �� �#D489B9�� $ border�� � ��$	 1.5q��� " ��.���  � ��$ 20(���  � �� �#333333��  � ��$ Medium"���  � ��$ Center=���  � ��$
 Left=���  � ��$ 0,0,10,0q��� " ��.���  � ��$ 22(���  � �� �#333333��  � ��$ Center=���  � ��$ 	0,15,0,15q��� " ��.���  � ��$ 16(���  � ��
 �������  � �� �#D489B9��  � ��$ 0q���  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��$ 8��#��z��#��z��#��z�� . ._��� $ contentPresenter��|�.  #��z��#��z����. # z #��z�� P� 	 �@J�IsMouseOver
 . . ��� $ border�� � �� �#C076A6P� 	 � ��
 . . ��� $ border�� � �� �#AB6792P� 	 � {�
 .  . ��� $ border�� � �� �#CCCCCC��  � �� �#888888�� " ��.���  � ��$ 16(���  � ��
 �������  � �� �#888888��  � ��$ 0q���  �@�Cursor$
 Hand����  � �� �� " ��.R��� $ border��$ 8��#��z��#��z��#��z�� . ._��� $ contentPresenter��|�.  #��z��#��z����. # z #��z�� P� 	 �@J�IsMouseOver
 . . ��� $ border�� � �� �#777777P� 	 � ��
 . . ��� $ border�� � �� �#666666P� 	 � {�
 .  . ��� $ border�� � �� �#CCCCCC��  � �� �#888888�� " ��.���  � ��$ 16(���  � �� �#D489B9��  � ��$ Medium"���  � ��$ Center=���  � ��$ 	0,10,0,15q�.���  � ColumnDefinitions �� $��* ��� $
��Auto ��� $��* �.+��� $��0�����#E6E6E6$ 8��$��
10,10,5,10q�._��� $��15q�.+��� $��User Settings��$��22(�$��SemiBold"����#D489B9$��0,0,0,15q��� -    ��Name$ loggedInStatusText��$��Not logged in��#��[ � $��0,5,0,25q� �� $��* ��� $��* �.+��� -    ��Name$ loginButton��$��Login��#��[ $	��110��$��50��$��0��$��0,0,0,0q�$��Center=�$��Center=�$��0q��� -   $ signOutButton��$��Sign Out��#��[ $	��110��$��50��$��1��$��0,0,0,0q�$��Center=�$��Center=�$��0q�$y�	Collapsed=�� $��0,8,0,8q� �� $
��Auto ��� $��* �.+��� $��
Username: ��#��[ $	��100��$��0���� -    ��Name$ setUsername��$��1��#��[  � $��0,8,0,8q� �� $
��Auto ��� $��* �.+��� $��
RAM (MB): ��#��[ $	��100��$��0���� -   $ setRAM��$��1��#��[  �� $��1��$��1�����#DFDFDF$��Stretch=�$��	0,10,0,10q��� $��2�����#E6E6E6$ 8��$��
5,10,10,10q�._��� ���#F5F5F5$ 6��$��0q�$��10q�._��� $��Center=�$��10q�.+��� $��Launcher Information��$��22(�$��SemiBold"����#D489B9$��0,0,0,15q�$��Center=��� #��[ $��0,8,0,8q��� $�	Version: ��$��SemiBold"� �� -    ��Name$ LauncherVersion��$�1.0.0���� #��[ $��0,8,0,8q��� $�Build: ��$��SemiBold"� �� -   $ LauncherBuild��$	�143���� #��[ $��0,8,0,8q��� $�Date: ��$��SemiBold"� �� -   $ LauncherDate��$�
April 2025��