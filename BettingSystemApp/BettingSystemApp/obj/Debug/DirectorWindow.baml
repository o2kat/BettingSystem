   M S B A M L   `   `   `  ����  BettingSystemApp%    BettingSystemApp.DirectorWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$0  *MAKMERIC | Панель директора��5   	   $	��700��5   	   $
��1000��6    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	   ���#FFF.��5
      �� ���#E0FFF7$��1q�6#    �� CornerRadius$ 8��67   $��10q�6H   ._�5   
   � $��15q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      �� $K��EУправление пользователями и ставками��6   ���#6A0DAD$��18(�5      $��SemiBold"�5      $��Center=�5      $��0,0,0,15q�5      �� 5      $��1��6   $
��Auto=�6(   .��5      �� .+�5      �� $��Пользователи:��6    ���#6A0DAD$��16(�5       $��SemiBold"�5        $��0,0,0,5q�5!       &   System.Windows.Controls.DataGrid  5#      -   5          Name$ UsersDataGrid��5#      $	��200��5$         AutoGenerateColumns .    
IsReadOnly .    HeadersVisibility$ Column=�5'      ���#B5EAD7$��1q�5)      	   RowBackground
	 �����
   AlternatingRowBackground
 �#F5F5FF   GridLinesVisibility$ 
Horizontal=�5,      $��0,0,0,15q�5-      �� 5/      $.��(Ставки пользователей:��6    ���#6A0DAD$��16(�51       $��SemiBold"�52       $��0,0,0,5q�53         55      -   5        $ UserBetsDataGrid��55      $	��200��56       .  . $ Column=�59      ���#B5EAD7$��1q�5;      
	 �����
 �#F5F5FF$ 
Horizontal=�5>      $��0,0,0,15q�5?      5@      5A      A� 5D      $��2��6    A� Rows$ 1��5E       A� Columns$ 4��5F      $��Center=�5G      $��0,10,0,0q�5H      .+�5I      �� -   5        $-��'Удалить пользователя��5I      $��5q�5J      $��35��5K      ���#C8A2C8���#333$��0q�5N      $��Medium"�5O      5P      �� 5R      -   5        $)��#Сформировать отчёт��5R      $��5q�5S      $��35��5T      ���#B5EAD7���#333$��0q�5W      $��Medium"�5X      5Y      �� 5[      -   5        $��Открыть отчёт��5[      $��5q�5\      $��35��5]      ���#B5EAD7���#333$��0q�5`      $��Medium"�5a      5b      �� 5d      -   5        $��
Назад��5d      $��5q�5e      $��35��5f      ���#CBC3E3���#333$��0q�5i      $��Medium"�5j      5k      5l      5m      5n      5o      