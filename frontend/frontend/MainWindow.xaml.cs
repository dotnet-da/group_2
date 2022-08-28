using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Newtonsoft.Json;


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
             
            char user = await checkLoginFromAPI();

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
                case 'a':
                    MessageBox.Show("Admin Login detected"); //\todo Admin-Modus (vielleicht öffnen von BackerStartWindow UND CostumerWindow?)
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

        private async Task<char> checkLoginFromAPI()
        {
            var result_from_api = await LoginHttp(tboxUserName.Text, tboxPassword.Password);
            if (result_from_api.Equals("backer"))
                return 'b';
            else if (result_from_api.Equals("customer"))
                return 'c';
            else if (result_from_api.Equals("admin"))
                return 'a';
            return 'e';
        }

        static async Task<string> LoginHttp(string u, string p)
        {
            var authData = Encoding.ASCII.GetBytes($"{u}:{p}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            Account objectUser = new Account(u, p);

            var json = JsonConvert.SerializeObject(objectUser);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{EnviromentPizza.baseUrl}login";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.PostAsync(url, postData);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;
            
            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            this.Close();
            createAccount.ShowDialog();
        }
    }
}
