using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace frontend
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private async void buttonCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if(passwordsMatch())
            {
                await Task.Delay(2000);
                messageLabel.Visibility = Visibility.Hidden;
                if (createAccount()) 
                {
                    CostumerWindow costumerWindow = new CostumerWindow(tboxUserName.Text);
                    this.Close();
                    costumerWindow.ShowDialog();
                    
                }
            }
            await Task.Delay(2000);
            messageLabel.Visibility = Visibility.Hidden;


        }

        private bool passwordsMatch() {
        
        if(tboxPassword.Password == String.Empty)
            {
                messageLabel.Content = "Password Field is empty";
                messageLabel.Background = Brushes.IndianRed;
                messageLabel.Visibility = Visibility.Visible;

                return false;
            }
            if (tBoxConfirmedPassword.Password == String.Empty)
            {
                messageLabel.Content = "Password Confirmation Field is empty";
                messageLabel.Background = Brushes.IndianRed;
                messageLabel.Visibility = Visibility.Visible;
                return false;
            }

            if (tboxPassword.Password != tBoxConfirmedPassword.Password)
            {
                messageLabel.Content = "Passwords do not match!";
                messageLabel.Background = Brushes.IndianRed;
                messageLabel.Visibility = Visibility.Visible;
                return false;
            }

            return true;
        }

        private bool createAccount() {
            //TODO implement this feature
            if(tboxUserName.Text == "Admin")
            {
                return true;
            }
            else
            {
                messageLabel.Content = "Acount Creation failed";
                messageLabel.Background = Brushes.IndianRed;
                messageLabel.Visibility = Visibility.Visible;

                return false;
            }
        }

    }
}
