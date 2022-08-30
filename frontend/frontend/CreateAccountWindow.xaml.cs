using frontend.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
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
using System.Xml.Linq;

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
                if (createAccount(tboxUserName.Text, tboxPassword.Password)) 
                {
                    messageLabel.Content = "Login succesful";
                    messageLabel.Background = System.Windows.Media.Brushes.Green;
                    messageLabel.Visibility = Visibility.Visible;
                    await Task.Delay(1000);


                    CostumerWindow costumerWindow = new CostumerWindow(tboxUserName.Text, tboxPassword.Password);
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

        private bool createAccount(String username, String password) {
            //TODO implement this feature
            FreshUser account = new FreshUser();
            account.ac_username = username;
            account.ac_password = password;
            account.ac_type = "client";

            var data = Task.Run(() => createAccountAPI(account));
            data.Wait();

            if (data.Result.Length > 3) //Result is not []
            {
                //ZutatenJson zutatenJson = JsonConvert.DeserializeObject<ZutatenJson>(data.Result);
                FreshUser freshUser = JsonConvert.DeserializeObject<FreshUser>(data.Result);
                if (freshUser != null)
                {
                    return true;
                }

            }
            return false;
        }

        private async Task<string> createAccountAPI(FreshUser createAccount)
        {
            //TODO update Bestellung in the API
            var authData = Encoding.ASCII.GetBytes($"backeraccount:1234"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;


            var json = JsonConvert.SerializeObject(createAccount);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");


            var url = $"{EnviromentPizza.baseUrl}account";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.PostAsync(url, postData);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
            return response;
        }

    }
}
