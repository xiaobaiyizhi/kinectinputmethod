   M S B A M L   `   `   `  ����  
newproject    newproject.MiniWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   \ XMicrosoft.Kinect.Toolkit.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullS OMicrosoft.Kinect.Toolkit, Version=1.7.0.0, Culture=neutral, PublicKeyToken=null2k(http://schemas.microsoft.com/kinect/2013   5   	   <d2http://schemas.microsoft.com/expression/blend/2008   5   	   Fmc;http://schemas.openxmlformats.org/markup-compatibility/2006   6F   -   5          � Title$  
MiniWindow��5   	   $	��768��6   $
��1366��6)    � WindowStyle$
 None=�66    � AllowsTransparency . ���	#003894E7���	#FF3894E7���	#FF3894E7$
��Left=�6�   $	��Top=�6�   .��5      � $	��300��6   $��Right=�6   .+�5	   
   4  .Microsoft.Kinect.Toolkit.Controls.KinectRegion  -   5          Name$ kinectRegion1��5	      $��0,40,0,0q�6T   $��Right=�6f   $	��120��6�   .��5
      �� $	��120��6   .+�5      8  2Microsoft.Kinect.Toolkit.Controls.KinectUserViewer  -   5          Name$ 
UserViewer��5      $��Center=�65   $	��120��6�      KinectRegion 6R   ��  �� ElementName$ kinectRegion1��6�   8  2Microsoft.Kinect.Toolkit.Controls.KinectTileButton  5      -   5        $��文字输入��5      $	��114��6.   $��Center=�6;   $
��Auto��6X   
�������$��24(�6�   $
��Bold"�6�   
���   �6�     5      -   5        $��悬停·按压��5      $��0q�6/   $
��Auto��6:   $	��120��6G   
�������$��24(�6g   $
��Bold"�6u   ���	#FF58D6146�     5      $��	放大镜��6   $��0q�6-   $
��Auto��68   $	��120��6E   
�������$��24(�6e   $
��Bold"�6s   ���	#FF815BED6�     5      -   5        $��上翻��5      $��0q�6,   $
��Auto��67   $	��120��6D   
�������$��24(�6d   $
��Bold"�6r   $	��Top=�6�   ���	#FF4896DE6�     5      -   5        $��下翻��5      $��0q�6,   $
��Auto��67   $	��120��6D   
�������$��24(�6d   $
��Bold"�6r   ���	#FFEB4A7E6�   5      5      �� 5   
   -   5         ��Name$ textBox1��5   M   $��56��6   $
��Left=�6   $��
0,35.5,0,0q�69   $	��Top=�6_   $	��180��6w   $��26(�6�   �� 5   
   -	   5        $ textBox2��5   3   $��60��6   $��0,101.5,0,0q�6   $	��Top=�6E   $��26(�6]   $�� ��6k   $
��Left=�6s   $	��180��6�   �� 5   
   -
   5        $ textBox3��5   3   $��26(�6   $��	0,175,0,0q�6    $�� ��6E   $��60��6M   $	��Top=�6Y   $
��Left=�6q   $	��180��6�   5      5      