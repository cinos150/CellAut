   M S B A M L   `   `   `  ����L  HCellularAutomaton, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:CellularAutomatonCellularAutomaton  5   	   %    CellularAutomaton.AddRuleWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:CellularAutomaton     5   	   X TXceed.Wpf.Toolkit, Version=2.7.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  Add new rule��5	   	   $	��500��6   $	��500��6+   .��5
      � .+�5   
   � -   5        $��positionBasedButtonGrid��5      $��10,74,193,147q�6.    � RowDefinitions 5      �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   5"      �� 5#   
   -   5         ��Name$ outputStateButton��5#      
�������$��Center=�6X   $��OutputState��6|   $��Center=�6�   $��392,174,24,221q�6�   $��76��6�   $��75��6�   �� 5$   
   -   5         ��Name$ label��5$      $��Output State��6   $
��Left=�66   $��390,143,0,0q�6Q   $	��Top=�6f   $��83��6~   � 5&   
   -   5        $��numberRulesGrid��5&      %  Xceed.Wpf.Toolkit.IntegerUpDown  5'   
   -   5        $��countControl��5'      $��67,335,365,90q�61   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Maximum$ 24X�6I      Minimum$ 0X�6V   �� 5(   
   -   5         ��Name$ countStateControl��5(      $
��Left=�6.   $��127,345,0,0q�6I   $	��Top=�6^   $��75��6v   �� 5)   
   -   5        $ 
firstLabel��5)      $��If there is��6$   $
��Left=�6:   $��	0,345,0,0q�6U   $	��Top=�6h   $��67��6�   �� 5*   
   -   5        $ 
cellsLabel��5*      $��cells��6$   $
��Left=�64   $��202,341,0,0q�6O   $	��Top=�6d   $��38��6|   �� 5/   
   -	   5        $ outputstatecontrol��5/      $
��Left=�6/   $��416,372,0,0q�6J   $	��Top=�6_   $	��75 ��6w   �� 50   
   -
   5        $ inputstateControl��50      $
��Left=�6.   $��274,372,0,0q�6I   $	��Top=�6^   $��75��6v   �� 51   
   -   5        $ label_Copy2��51      $��becomes��6%   $
��Left=�67   $��354,370,0,0q�6R   $	��Top=�6g   $��61��6   �� 52   
   -   5        $ label_Copy3��52      $
��then��6%   $
��Left=�64   $��231,370,0,0q�6O   $	��Top=�6d   $��68��6|   �� 53   
   -   5        $ button1��53      $��Add number based rule��6"   $
��Left=�6B   $��218,322,0,0q�6]   $	��Top=�6r   $	��173��6�   �� 54   
   -   5        $ button1_Copy��54      $��Add position based��6'   $
��Left=�6E   $��375,254,0,0q�6`   $	��Top=�6u   $	��107��6�   $��37��6�   57      58      