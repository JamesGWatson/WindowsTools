   M S B A M L   `   `   `  ����F  BHashProgram, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:HashProgramHashProgram  5   	       HashProgram.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:HashProgram     5   	   -   5          �Name$
  Form��5   	    � Title$
 VeCo��5   	   $	��596��6   $	��797��6#   4  .System.Windows.Automation.AutomationProperties   Name$
 Test��6/    � WindowStartupLocation$ CenterScreen=�6P    � WindowStyle$ SingleBorderWindow=�6u    � Icon$ /veritascopy.ico��6�   ���	#FF26282C$��Stretch=�6�   ���	#FF8D8D8D � TaskbarItemInfo 5	      *  $System.Windows.Shell.TaskbarItemInfo  5
   
   .��5      ]� -   5         ]�Name$
 dock��5      $��Stretch=�6   $��30,30,30,30q�6<   $��Stretch=�6Q   .+�5   
   �� $��Configuration��6   $��Stretch=�6*   $��Stretch=�6O   $	��265��6r   $��Right=�6~   $��1,1,1,1q�6�   $��8,0,0,0q�6�   ���	#FF131617$
��True��6�   .��5      �� $
��Left=�6   $
��True��6=   $
��Auto=�6U   .��5      �� -   5         ��Name$ panel_configHolder��5      $
��True��69   	 �� CanVerticallyScroll$
	 True��6^   $	��228��6y   .+�5      �� -   5        
 ��Name$
 cb_copyOnDrop��5      $$��Start copy immediately on drop��66    d� 	IsChecked$
 TrueY�6_   ���	#FF8D8D8D$��0,10,0,0q�6�   �� 5      -   5         ��Name$ gb_hashOptions��5      $��Hashing��67   $	��110��6H   $��0,10,0,0q�6U   $��0,10,0,0q�6h   .��5      �� -   5        $ sp_HashSelection��5   %   $��76��6?   $	��Top=�6K   .+�5      �� -   5        $
 
cb_hashMD5��5   '   $	��MD5��6;   $
 TrueY�6I   ���	#FF8D8D8D6Z   �� 5      -   5        $
 cb_hashSHA1��5   '   $
��SHA1��6<   ���	#FF8D8D8D6K   �� 5      -	   5        $
 cb_hashSHA256��5   '   $��SHA-256��6>   ���	#FF8D8D8D6P   �� 5      -
   5        $
 cb_hashSHA384��5   '   $��SHA-384��6>   ���	#FF8D8D8D6P   �� 5      -   5        $
 cb_hashSHA512��5   '   $��SHA-512��6>   ���	#FF8D8D8D6P   5      5      �� 5      -   5        $ gb_destination��5      $��Destination��67   $	��145��6L   $��0,15,0,0q�6Y   .��5      �� -   5        $ sp_Destination��5   %   .+�5      	� -   5         	�Name$ rb_destinationDefault��5   *   $��Use default��6I   $��0,6,0,0q�6_   $
 TrueY�6p   ���	#FF8D8D8D6�   �� 5      $��	TextBlock��6(   $
��Wrap=�69   $��15��6M   $	��205��6Y   $
��Left=�6e   	� 5      -   5        $ rb_destinationTree��5   *   $��Use Decision Tree��6F   $��0,10,0,0q�6b   ���	#FF8D8D8D6t   � 5       $��17��6#    � ColumnDefinitions 5!   "   �� 5"   &    � Language$ en-gb�67   $��30��6H   $
��Auto �6V   $	��172��6c   �� 5#   &   $
��Auto �67   $��46.623��6D   .+�5%   "   �� $
��Left=�6,   $��No tree selected��6G   $
��Wrap=�6_   $��Center=�6s   $��15��6�   $	��162��6�   �� 5&   "   $��Button��6)   $��1��6:   $��Center=�6J   $��Center=�6g   $��16��6�   $��40��6�   5'      	� 5(      -   5        $ rb_destinationOneOff��5(   *   $��Use default��6H   $��0,6,0,0q�6^   ���	#FF8D8D8D6o   � 5)      $��17��6#    5*   "   �� 5+   &   $ en-gb�67   $��30��6H   $
��Auto �6V   $	��172��6c   �� 5,   &   $
��Auto �67   $��46.623��6D   .+�5.   "   �� $
��Left=�6,   $��	TextBlock��6G   $
��Wrap=�6X   $��Center=�6l   $��15��6�   $	��162��6�   �� 5/   "   $��Button��6)   $��1��6:   $��Center=�6J   $��Center=�6g   $��16��6�   $��40��6�   50      51      52      �� 53      -   5        $
 cb_logHashes��53      $��
Log hashes��65   $
 TrueY�6J   $��0,20,0,0q�6[   ���	#FF8D8D8D6m   �� 54      -   5        $ gb_logOptions��54      $��Logging��66   $	��111��6G   $	��Top=�6T   .��55      �� $��0,7,0,0q�6%   .+�56      	� -   5        $ rb_logShowOnly��56   *   $��Display only��6B   $
 TrueY�6Y   ���	#FF8D8D8D$
��Left=�6�   $��19��6�   $	��216��6�    	� 	GroupName$
 logs��6�   	� 57      -   5        $ rb_logSaveHash��57   *   $ ��Hash result in destination��6B   ���	#FF8D8D8D$
��Left=�6~   $��19��6�   $	��216��6�   $
 logs��6�   	� 58      -   5        $ rb_logSaveLog��58   *   $��Log file in destination��6A   ���	#FF8D8D8D$
��Left=�6z   $��19��6�   $	��216��6�   $
 logs��6�   � 59      $��19��6#    5:   "   �� 5;   &   $ en-gb�67   $��30��6H   $
��Auto �6V   $	��153��6c   �� 5<   &   $
��Auto �67   $��65.623��6D   .+�5>   "   �� $��Config��6)   $��1��6:   $��Center=�6J   $��Center=�6g   $��24��6�   $��66��6�   	� 5?   "   -   5        $ rb_logSaveServer��5?   .   $��Log to server��6H   ���	#FF8D8D8D$
��Left=�6w   $��0,0,0,-5q�6�   $
��Left=�6�   $	��121��6�   $
 logs��6�   $��1,1,1,1q�6�   5@      5A      5B      5C      5D      5E      �� 5F   
   $��Stretch=�6   $��Stretch=�6<   .��5G      � -   5         �Name$ 
wrapPanel1��5G      $��0,0,0,0q�6,   $
��True��6=   ���	#FF343434 J� 	AllowDrop$
 True��6y   6�   5H      5I      5J      