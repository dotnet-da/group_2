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

namespace frontend
{
    /// <summary>
    /// Interaction logic for CostumerWindow.xaml
    /// </summary>
    public partial class CostumerWindow : Window
    {
        List<String> ordersListPizzaNames;
        List<Bestellung> orderList;
        public CostumerWindow(String username)
        {
            InitializeComponent();
            nameBlock.Text = "Hello "+username;
            ordersListPizzaNames = new List<String>();
            fillPizzaList();

           
        }
        private void fillPizzaList() {
            //TODO get Data from the API
            //List<String> list = new List<String>();
            //list.Add("Salami");
            //list.Add("Mushrooms");
            //list.Add("Mozarella");
            //listBox.ItemsSource = list;

            List<Pizza> pizzaList = new List<Pizza>() ;
            Pizza pizza = new Pizza();
            pizza.pizzaName = "Pizza Salami";
            pizza.pizzaID = 0;

            Pizza pizza1 = new Pizza();
            pizza1.pizzaID=1;
            pizza1.pizzaName = "Schinken";

            pizzaList.Add(pizza);
            pizzaList.Add(pizza1);



            listBox.ItemsSource = convertPizzaToList(pizzaList);

            //This shit is not working 

            //var data = Task.Run(() => GetAllPizzas());
            //data.Wait();
            //Console.WriteLine(data.Result);
            //if (data.Result.Length > 3) //Result is not []
            //{




            //}
            //else
            //{
            //    MessageBox.Show("There is no PIzza!!!");
            //}



        }

        private List<String> convertPizzaToList(List<Pizza> pizzaList)
        {
            List<String> list = new List<String>();
            foreach (Pizza pizza in pizzaList) {
                list.Add(pizza.pizzaName);
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
