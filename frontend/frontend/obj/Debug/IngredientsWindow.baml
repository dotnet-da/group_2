   M S B A M L   `   `   `  ????C  ?frontend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:frontendfrontend  5   	        frontend.IngredientsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:frontend     5   	     ? Title$  Ingredients??5   	   $	??450??6   $	??800??6*   .??5	      ?  ? ColumnDefinitions 5
   
   ?? 5      $??5* ?6   ?? 5      $??6* ?6   ?? 5      $??5* ?6    ? RowDefinitions 5   
   ?? 5      $	??25* ?6   ?? 5      $
??142* ?6   ?? 5      $	??50* ?6   .+?5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid??5      $??3??6P   $??2??6d   $??0,50,0,0q?6u   ?? 5   
   -   5         ??Name$ 	textBlock??5      $??1??6'   $
??Wrap=?67   $??Ingredients??6K   ?? 5   
   -   5         ??Name$ labelConfirm??5      $??2??6&   $??Label??66   $??2??6F   $y?Hidden=?6S   ?? 5   
   -   5         ??Name$ textBoxAmount??5      $??Center=?6)   $??2??6F    ?? TextWrapping$
 Wrap=?6S   $?? ??6g   $??Center=?6o   $	??146??6?   $??38??6?   ?? 5   
   -   5        $ label1??5      $??Amount:??6    $??Center=?62   $??0,5,0,0q?6O   $??2??6`   $	??Top=?6m   $	??146??6?    J? RenderTransformOrigin$ 0.537,-0.039&?6?   ?? 5   
   -   5        	 ??Name$	 buttonPurchase??5      $??1??6)   $??Purchase??69   $??2??6L   6Y   ?? 5   
   -   5        $	 
buttonHome??5      $??2??6%   $
??Home??65   6D   5      5      