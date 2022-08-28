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
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using frontend.Objects;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace frontend
{
    /// <summary>
    /// Interaction logic for CostumerWindow.xaml
    /// </summary>
    public partial class CostumerWindow : Window
    {
        List<String> ordersListPizzaNames;
        List<Bestellung> orderList;

        String userName;
        String password;
        int id;
        public CostumerWindow(String username, String password)
        {
            this.userName = username;
            this.password = password;

            InitializeComponent();
            nameBlock.Text = "Hello "+username;
            ordersListPizzaNames = new List<String>();
            fillPizzaList();
            getID();
        }
        private void getID() {

            var data = Task.Run(() => getIDFromAPI(userName, password));
            data.Wait();

            if (data.Result.Length > 3) //Result is not []
            {
                //ZutatenJson zutatenJson = JsonConvert.DeserializeObject<ZutatenJson>(data.Result);
                List<AccountsUser> accountList = JsonConvert.DeserializeObject<List<AccountsUser>>(data.Result);

                foreach (AccountsUser account in accountList)
                {
                    if (account.ac_username == userName)
                    {
                        this.id = account.ac_id;
                        break;
                    }
                }
            }

        }

        static async Task<string> getIDFromAPI(string u, string p)
        {
            var authData = Encoding.ASCII.GetBytes($"{u}:{p}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            var url = $"{EnviromentPizza.baseUrl}account";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            //HttpResponseMessage result = await client.PostAsync(url, postData);
            HttpResponseMessage result = await client.GetAsync(url);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void fillPizzaList() {
            //TODO get Data from the API

            List<Pizza> pizzaList = new List<Pizza>() ;

            var data = Task.Run(() => getPizzasFromAPI(userName, password));
            data.Wait();

            if (data.Result.Length > 3) //Result is not []
            {
                //ZutatenJson zutatenJson = JsonConvert.DeserializeObject<ZutatenJson>(data.Result);
                pizzaList = JsonConvert.DeserializeObject<List<Pizza>>(data.Result);


            }

            listBox.ItemsSource = convertPizzaToList(pizzaList);





        }
        static async Task<string> getPizzasFromAPI(string u, string p)
        {
            var authData = Encoding.ASCII.GetBytes($"{u}:{p}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            var url = $"{EnviromentPizza.baseUrl}pizza";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            //HttpResponseMessage result = await client.PostAsync(url, postData);
            HttpResponseMessage result = await client.GetAsync(url);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private List<String> convertPizzaToList(List<Pizza> pizzaList)
        {
            List<String> list = new List<String>();
            foreach (Pizza pizza in pizzaList) {
                list.Add(pizza.p_name);
            }
            return list;
        }
        
        static async Task<string> GetAllPizzas(string id = null)
        {


            var userName = "adminaccount";
            var passwd = "1234";
            var authData = Encoding.ASCII.GetBytes($"{userName}:{passwd}");
            var response = string.Empty;
            var url = EnviromentPizza.baseUrl + "pizza";



            HttpClient client = new HttpClient();


            HttpResponseMessage result = await client.GetAsync(url);
            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void updateOrdersList(String pizzaName) 
        {
            ordersListPizzaNames.Add(pizzaName);
            ordersListBox.ItemsSource = null;
            ordersListBox.ItemsSource = ordersListPizzaNames;
        }

        //This should update the OrdersList every 5 Sec or so. Get API all orders from this person, match with pizza names. 
        private async void updateOrdersAsync() {
            //TODO get Orders Data from API
            Bestellung bestellung = new Bestellung();
            bestellung.status = "Order given";

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            String selectedPizza = listBox.SelectedItem.ToString();

            updateOrdersList(selectedPizza);
        }


        private void orderPizza()
        {
            //TODO tatsächlich eine Order per API erstellen. Pizzanamen mit der Pizza ID matchen. 
        }

        private void orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ordersListBox.SelectedItem == null)
                return;
            String selectedPizza = ordersListBox.SelectedItem.ToString();
            statusTextBlock.Visibility = Visibility.Visible;

            switch (getStatus(selectedPizza))
            {   
                
                case "Order given":
                    orderStatusLabel.Visibility = Visibility.Visible;
                    orderStatusLabel.Content = "Order given";
                    orderStatusLabel.Background = Brushes.Gray;
                    break;

                case "Order accepted":
                    orderStatusLabel.Visibility = Visibility.Visible;
                    orderStatusLabel.Content = "Order accepted";
                    orderStatusLabel.Background = Brushes.Yellow;
                    break;

                case "Pizza made":
                    orderStatusLabel.Visibility = Visibility.Visible;
                    orderStatusLabel.Content = "Pizza made";
                    orderStatusLabel.Background = Brushes.YellowGreen;
                    break;

                case "On the Way":
                    orderStatusLabel.Visibility = Visibility.Visible;
                    orderStatusLabel.Content = "On the Way";
                    orderStatusLabel.Background = Brushes.YellowGreen;
                    break;

                case "Delivered":
                    orderStatusLabel.Visibility = Visibility.Visible;
                    orderStatusLabel.Content = "Delivered";
                    orderStatusLabel.Background = Brushes.Green;
                    break;


                default:
                    break;
            }
        }
        private string getStatus(String pizzaName)
        {
            return "Order given";
        }
    }
}
