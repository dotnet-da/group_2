   M S B A M L   `   `   `  ????C  ?frontend, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:frontendfrontend  5   	       frontend.BackerOrder   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:frontend     5   	     ? Title$  BackerOrder??5   	   $	??450??6   $	??800??6*   .??5	      ?  ? RowDefinitions 5
   
   ?? 5      $
??138* ?6   ?? 5      $	??39* ?6   ?? 5      $	??40* ?6    ? ColumnDefinitions 5   
   ?? 5      $	??13* ?6   ?? 5      $	??14* ?6   ?? 5      $	??13* ?6   .+?5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid??5      $??3??6P   ?? 5   
   -   5         ??Name$ buttonConfirm??5      $??Confirm Order??6(   $??1??6@   6M   ?? 5   
   -   5        $ 
buttonMake??5      $??1??6%   $??
Make Pizza??65   $??1??6J   6W   ?? 5   
   -   5        $ 
buttonSend??5      $??2??6%   $??Deliver Pizza??65   $??1??6M   6Z   ?? 5   
   -   5        $ 
buttonHome??5      $
??Home??6%   $??2??64   6A   ? 5   
   -   5        $??progressMake??5      $??Center=?6*   $??19??6G   $??2??6S   $??Center=?6`   $	??216??6{   $??1??6?    ? Minimum$ 0Q?6?    ? Maximum$	 100Q?6?   ? 5   
   -   5        $??progressSend??5      $??Center=?6*   $??19??6G   $??2??6S   $??Center=?6`   $	??216??6{   $??2??6?   $ 0Q?6?   $	 100Q?6?   5      5       