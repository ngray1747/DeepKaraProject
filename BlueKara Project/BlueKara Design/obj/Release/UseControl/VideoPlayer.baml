   M S B A M L   `   `   `  ���� J  FBlueKara Design, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:BlueKara_Design.UseControlBlueKara_Design.UseControl  ,    &BlueKara_Design.UseControl.VideoPlayer   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     :local(clr-namespace:BlueKara_Design.UseControl     T PMahApps.Metro, Version=1.3.0.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4f9Custom,http://metro.mahapps.com/winfx/xaml/controls  -     >�Name$  Usecontrolvideoplayer��$	��693�� � 	Resources  �� 
TargetType   Customslider  MetroSlider �� BasedOn��%   &	        7  �� " ��8   .��� .+�r� -    r�Name$ mediaElement�� r� Stretch$
 Fill=� r� LoadedBehavior$ Manual=� r� UnloadedBehavior$
 Stop=�$
��Left=�$	��630��$	��Top=�$
��1350���� -    ��Name$ lbtenbaihat��
��� � �	 �� TextAlignment$	 Center=�$��Căn Nhà Màu Tím��$��30(�$
��Left=�$��780,630,0,0q�$	��Top=�$��63��$	��570���� -   
 ��Name$
 Pause�� � Cursor$
 Hand��$��../Image/pause.png��$
��Left=�$��44��$��121,639,0,0q�$	��Top=�$��47���� -   $

 Play��$
 Hand��$y�Hidden=�$��../Image/play.png��$
��Left=�$��44��$��108,639,0,0q�$	��Top=�$��47���� -   $

 Next��$
 Hand��$��../Image/next.png��$
��Left=�$��44��$��173,639,0,0q�$	��Top=�$��50���� -    ��Name$ slider��$
 Hand�� � Value$ 0Q�#��[  $
��Left=�$��297,655,0,0q�$	��Top=�$	��389��$��22��)  #MahApps.Metro.Controls.SliderHelper   ChangeValueBy$ LargeChange=�   EnableMouseWheel$ ControlFocused=�   SliderSelectionBackground �� Color��%   &	       �� $ Green���� -   $ tblTimePass��$��17(�$
��Left=�$��198,650,0,0q�$
��Wrap=�$
��----��$	 Center=�$	��Top=�$��27��$��97�� J� RenderTransformOrigin$ 0.5,0.5&�z�V� .~��� �� ��  �� Angle$ 0.194Q�T� �� -	   $

 Stop��$
 Hand��$/��)/BlueKara Design;component/Image/stop.png��$
��Left=�$��39��$��
79,644,0,0q�$	��Top=�$��37���� -
   $ tblTimeTotal��$��17(�$
��Left=�$��691,650,0,0q�$
��Wrap=�$
��----��$	 Center=�$	��Top=�$��27��$��98��$ 0.5,0.5&�z�V� .~��� �� �� $ 0.418Q�T� �� -   $
 
playrecord��$
 Hand��$5��//BlueKara Design;component/Image/playrecord.png��$
��Left=�$��39��$��
24,644,0,0q�$	��Top=�$��50���� -   $
 
stoprecord��$y�Hidden=�$
 Hand��$4��./BlueKara Design;component/Image/recording.png��$
��Left=�$��39��$��
24,644,0,0q�$	��Top=�$��50��