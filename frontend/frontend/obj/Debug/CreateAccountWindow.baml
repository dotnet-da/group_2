   M S B A M L   `   `   `  ????C  ?frontend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:frontendfrontend  5   	       frontend.CreateAccount   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:frontend     5   	     ? Title$  CreateAccount??5   	   $	??450??6   $	??800??6,   .??5	      ?  ? RowDefinitions 5
   
   ?? 5      $
??152* ?6   ?? 5      $??30 ?6   ?? 5      $??30 ?6   ?? 5      $	??34* ?6   ?? 5      $	??35* ?6   ?? 5      $	??49* ?6   ?? 5      $	??49* ?6   ?? 5      $	??55* ?6    ? ColumnDefinitions 5   
   ?? 5      $
??251* ?6   ?? 5      $
??116* ?6   ?? 5      $	??180 ?6   ?? 5      $
??254* ?6   .+?5   
   ?? -   5        $??tboxUserName??5       ?? TextWrapping$
 Wrap=?6&   $?? ??6:   $??2??6B   $??1??6R   G? 5   
   -   5        $??tboxPassword??5      $??2??6*   $??2??6;   G? 5   
   -   5         G?Name$ tBoxConfirmedPassword??5      $??2??65   $??3??6E   ?? 5    
   -   5         ??Name$ label??5       $??1??6   $??	Username:??6/   $??1??6C   ?? 5!   
   -   5        $ label1??5!      $??1??6    $??	Password:??60   $??2??6D   ?? 5"   
   -   5        $ label2??5"      $??1??6    $??Confirm Password:??60   $??3??6L   ?? 5$   
   -   5         ??Name$ buttonCreateAccount??5$      $??2??6.   $??Create Account??6>   $??5??6W    J? RenderTransformOrigin$ 0.5,0.5&?6d   6?   ?? 5&   
   -   5        $ messageLabel??5&      $??1??6&   $??Passwords dont match??66   $??5??6U   $??2??6b   $??0,49,0,0q?6v   $??2??6?   $y?Hidden=?6?   5+      5,      