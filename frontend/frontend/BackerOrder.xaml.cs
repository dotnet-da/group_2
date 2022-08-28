using frontend.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
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
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Windows.Threading;

namespace frontend
{
    /// <summary>
    /// Interaction logic for BackerOrder.xaml
    /// </summary>
    public partial class BackerOrder : Window
    {
        int runs = 0;
        String name;
        String password;
        List<Bestellung> gridData;
        BackerBestellung currentBestellungInOven;
        bool pizzaInTheOven = false;

        BackerBestellung currentBestellungSending;
        bool pizzaDriverDriving = false;
        public BackerOrder(String name, String password)
        {
            this.name = name;
            this.password = password;
            InitializeComponent();
            getOrdersAsync();

            // Create a timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += myEvent;
            timer.Start();
        }

        // Implement a call with the right signature for events going off
        private void myEvent(object sender, EventArgs e)
        {
            getOrdersAsync();
        }
        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            BackerBestellung bestellung = (BackerBestellung) dataGrid.SelectedItem;
            if (bestellung == null)
            {
                MessageBox.Show("You need to select an Order first");
                return;
            }
            updateBestellung(bestellung, "Order accepted");
        }

        private async void updateBestellung(BackerBestellung bestellung, String status) {
            //TODO update Bestellung in the API
            var authData = Encoding.ASCII.GetBytes($"{name}:{password}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            BackerBestellelungStatus backerBestellelungStatus = new BackerBestellelungStatus();
            backerBestellelungStatus.be_status = status;  


            var json = JsonConvert.SerializeObject(backerBestellelungStatus);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{EnviromentPizza.baseUrl}order/" + bestellung.be_id;


            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.PutAsync(url, postData);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
        }

        //This should get all the Orders from the API where the username matches
        private void getOrdersAsync() {
            //Testing
            var data = Task.Run(() => getOrdersFromAPI(name, password));
            data.Wait();

            if (data.Result.Length > 3) //Result is not []
            {
                //ZutatenJson zutatenJson = JsonConvert.DeserializeObject<ZutatenJson>(data.Result);
                List<BackerBestellung> accountList = JsonConvert.DeserializeObject<List<BackerBestellung>>(data.Result);
                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = accountList;
                

            }
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

        private void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            BackerStartWindow backerStartWindow = new BackerStartWindow(name, password);
            this.Close();
            backerStartWindow.ShowDialog();
        }

        private async void buttonMake_Click(object sender, RoutedEventArgs e)
        {
            if (pizzaInTheOven)
            {
                MessageBox.Show("There is already a Pizza in the Oven");
                return;
            }
            currentBestellungInOven = (BackerBestellung)dataGrid.SelectedItem;
            if(currentBestellungInOven == null)
            {
                MessageBox.Show("You need to select an Order first");
                return;
            }
            if(currentBestellungInOven.be_status == "Order accepted")
            {
                pizzaInTheOven = true;
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_MakePizza;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += pizzaMade;
                worker.RunWorkerAsync();
            }
            else {
                MessageBox.Show("You musst first Accept the Order");
                currentBestellungInOven = null;
            }

                          
         
        }    



        void worker_MakePizza(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(80);
            }
            
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressMake.Value = e.ProgressPercentage;
        }

        void worker_ProgressChangedDelivery(object sender, ProgressChangedEventArgs e)
        {
            progressSend.Value = e.ProgressPercentage;
        }

        private void pizzaMade(object sender, RunWorkerCompletedEventArgs e)
        {
            updateBestellung(currentBestellungInOven, "Pizza made");
            currentBestellungInOven = null;
            pizzaInTheOven = false;
        }

        private void pizzaDelivered(object sender, RunWorkerCompletedEventArgs e)
        {
            updateBestellung(currentBestellungSending, "Pizza delivered");
            currentBestellungSending = null;
            pizzaDriverDriving = false;
        }

        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            if (pizzaDriverDriving)
            {
                MessageBox.Show("The Pizza driver is already driving");
                return;
            }
            currentBestellungSending = (BackerBestellung)dataGrid.SelectedItem;
            if (currentBestellungSending == null)
            {
                MessageBox.Show("You need to select an Order first");
                return;
            }
            if (currentBestellungSending.be_status == "Pizza made")
            {
                pizzaDriverDriving = true;
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DeliverPizza;
                worker.ProgressChanged += worker_ProgressChangedDelivery;
                worker.RunWorkerCompleted += pizzaDelivered;
                worker.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("You musst first make the Pizza");
                currentBestellungSending = null;
            }

           

            

        }
        void worker_DeliverPizza(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(80);
            }

        }
    }
}
