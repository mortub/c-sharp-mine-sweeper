   M S B A M L   `   `   `  ����C  ?MSClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:MSClientMSClient  5   	       MSClient.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:MSClient     5   	   
���   �  � Title$  Login��5	   	   $	��450��6   $	��800��6$    � 
ResizeMode$ NoResize=�60   .��5
      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   r� -   5        $��mediaElement1��5   	   $	��114��5      $
��Left=�6%   $��185.333,10,0,0q�6@   $	��Top=�5      $	��310��66    r� Source$F @C:\Users\mor_t\Downloads\c# projects\MSClient\MSClient\giphy.gif?�6B    r� LoadedBehavior$
 Play=�5   	    r� Stretch$
 Fill=�6    r� 
SpeedRatio$ 1Q�6.    r� IsMuted .  $��2��6M   $��1��6a   $��1��6q   �� 5   
   $��0��6   $��3��6   
�������.��6F   	Username:6Q   �� 5   
   -   5        $��
tbUsername��5   7   $��10,10,236,145q�6   $��1��6)   $��2��6J   �� 5   
   $��1��6   $��3��6   $��0,66,-0.333,100q�62   
�������.��6^   	Password:6i   �� 5   
   -   5        $��
tbPassword��5   7   $��10,101,236,54q�6   $��1��6)   $��2��6J   �� 5   
   -   5        $��Login��5      $��2��6"   $��49,10,236,106q�6/   $��Oblique$�6[   $��DemiBold"�6o   
����·�$��2��6�   �� 5   
   $��1��6   $��10,156,112.667,10q�6   $��2��68   
�������.��6[   not registered?6l   �� 5   
   -   5        $��Register��5      $��2��6%   $��216.333,10,68.667,106q�62   $��Oblique$�6p   $��DemiBold"�6�   
����·�$��1��6�   $��2��6�   5      5       