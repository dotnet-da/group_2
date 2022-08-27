using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace frontend
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String username;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
             
            char user = checkLogin();

            switch (user)
            {
                case 'b':
                    sucess.Content = "Login succesful";
                    sucess.Background = System.Windows.Media.Brushes.Green;
                    sucess.Visibility = Visibility.Visible;

                    await Task.Delay(2000);

                    BackerStartWindow backerStartWindow = new BackerStartWindow(username);
                    this.Close();
                    backerStartWindow.ShowDialog();
                    //TODO go to Bäcker Window
                    break;

                case 'u':
                    sucess.Content = "Login succesful";
                    sucess.Background = System.Windows.Media.Brushes.Green;
                    sucess.Visibility = Visibility.Visible;

                    await Task.Delay(2000);
                    CostumerWindow costumerWindow = new CostumerWindow(username);
                    this.Close();
                    costumerWindow.ShowDialog();

                    break;
                default:
                    sucess.Content = "Login not succesful";
                    sucess.Background = System.Windows.Media.Brushes.Red;
                    sucess.Visibility = Visibility.Visible;

                    await Task.Delay(2000);

                    sucess.Visibility = Visibility.Hidden;
                    break;
            }

            if (true) {
                



                sucess.Visibility = Visibility.Hidden;
            } else {
                

            }
        }

        private char checkLogin() {
            //TODO actually check the login and get the username + type back. 
            if (tboxUserName.Text == "backer" && tboxPassword.Password == "backer")
            {
                this.username = "Backer";
                return 'b';
            }
            else if (tboxUserName.Text == "user" && tboxPassword.Password == "user")
            {
                this.username = "Costumer";
                return 'u';
            }
            else 
            {  
                return 'e';
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            this.Close();
            createAccount.ShowDialog();
        }
    }
}
