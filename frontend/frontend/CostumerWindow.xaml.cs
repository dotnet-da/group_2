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
using System.Windows.Markup;
using System.Windows.Threading;

namespace frontend
{
    /// <summary>
    /// Interaction logic for CostumerWindow.xaml
    /// </summary>
    public partial class CostumerWindow : Window
    {
        List<String> ordersListPizzaNames;
        List<Bestellung> orderList;
        List<Pizza> pizzaList;

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
            pizzaList = new List<Pizza>();
            orderList = new List<Bestellung>();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += myEvent;
            timer.Start();

            fillPizzaList();
            getID();
            updateOrdersAsync();
        }
        // Implement a call with the right signature for events going off
        private void myEvent(object sender, EventArgs e)
        {
            updateOrdersAsync();
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
        
        //This should update the OrdersList every 5 Sec or so. Get API all orders from this person, match with pizza names. 
        private async void updateOrdersAsync() {
            //TODO get Orders Data from API

            ordersListPizzaNames.Clear();
            orderList.Clear();

            var data = Task.Run(() => getOrdersFromAPI(userName, password));
            data.Wait();

            if (data.Result.Length > 3) //Result is not []
            {
                //ZutatenJson zutatenJson = JsonConvert.DeserializeObject<ZutatenJson>(data.Result);
                List<Bestellung> accountList = JsonConvert.DeserializeObject<List<Bestellung>>(data.Result);
                foreach (Bestellung tmpBestellung in accountList)
                {
                    if(tmpBestellung.ac_id == id)
                    {
                        orderList.Add(tmpBestellung);
                        ordersListPizzaNames.Add("Order ID: " + tmpBestellung.be_id + " | Pizza: " + tmpBestellung.p_name);
                    }
                        
                }
                
            }
            ordersListBox.ItemsSource = null;
            ordersListBox.ItemsSource = ordersListPizzaNames;

        }

        static async Task<string> getOrdersFromAPI(string u, string p)
        {
            var authData = Encoding.ASCII.GetBytes($"{u}:{p}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            var url = $"{EnviromentPizza.baseUrl}order";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            //HttpResponseMessage result = await client.PostAsync(url, postData);
            HttpResponseMessage result = await client.GetAsync(url);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
            return response;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            String selectedPizza = listBox.SelectedItem.ToString();
            int pizzaID = -1; 
            foreach(Pizza pizza in pizzaList) {
                if (selectedPizza == pizza.p_name)
                {
                    pizzaID = pizza.p_id;
                    break;
                }
            }
            orderPizza(pizzaID);
            updateOrdersAsync();
        }


        private async void orderPizza(int pizzaID)
        {
            var authData = Encoding.ASCII.GetBytes($"{userName}:{password}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            UserBestellung bestellung = new UserBestellung();
            bestellung.p_id = pizzaID;
            bestellung.ac_id = id;

            var url = $"{EnviromentPizza.baseUrl}order";
            var json = JsonConvert.SerializeObject(bestellung);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");


            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.PostAsync(url, postData);
           // HttpResponseMessage result = await client.SendAsync(url, );
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
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

                case "Pizza delivered":
                    orderStatusLabel.Visibility = Visibility.Visible;
                    orderStatusLabel.Content = "Pizza delivered";
                    orderStatusLabel.Background = Brushes.Green;
                    break;


                default:
                    break;
            }
        }
        private string getStatus(String pizzaName)
        {
            int end = pizzaName.IndexOf('|');
            int test =int.Parse( pizzaName.Substring(10, end - 11));

            foreach (Bestellung tmpBestellung in orderList) {
                if (tmpBestellung.be_id == test)
                {
                    return tmpBestellung.be_status;
                }
            }
            return "something went Wrong";
        }
    }
}
