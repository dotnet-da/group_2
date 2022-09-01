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
using System.Timers;
using System.Windows.Threading;

namespace frontend
{
    /// <summary>
    /// Interaction logic for Ingredients.xaml
    /// </summary>
    public partial class IngredientsWindow : Window
    {
        String name;
        String password;

        int runs = 0;
        List<Zutat> zutatenList;
        public IngredientsWindow(String name, String password)
        {
            this.name = name;
            this.password = password;
            InitializeComponent();

            // Create a timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += myEvent;
            timer.Start();



            updateGrid();
        }
        // Implement a call with the right signature for events going off
        private void myEvent(object sender, EventArgs e) { 
            updateGrid();
        }


        private void updateGrid() {
            
            Zutat record = (Zutat)dataGrid.SelectedItem;
            var selectedItemIndex = dataGrid.SelectedIndex;
            dataGrid.ItemsSource = null;
            //zutatenList = getZutatenFromApi();
            //zutatenList = 
            var data = Task.Run(() => getIngredients(name, password));
            data.Wait();

            if (data.Result.Length > 3) //Result is not []
            {
                //ZutatenJson zutatenJson = JsonConvert.DeserializeObject<ZutatenJson>(data.Result);
                zutatenList = JsonConvert.DeserializeObject<List<Zutat>>(data.Result);

                int test = 0;
                dataGrid.ItemsSource = zutatenList;//writes the data to DataGrid
            }
            if (record != null)
            {
                int indexToBeSelected = -1;

                for (int i = 0; i < dataGrid.Items.Count; i++)
                {
                    Zutat compareTo = (Zutat)dataGrid.Items[i];
                    if (((Zutat)dataGrid.Items[i]).zu_id == record.zu_id)
                    {
                        indexToBeSelected = i;
                        break;
                    }
                }
                dataGrid.SelectedIndex = indexToBeSelected;
            }
            //dataGrid.ItemsSource = zutatenList;
        }

        private 

        static async Task<string> getIngredients(string u, string p)
        {
            var authData = Encoding.ASCII.GetBytes($"{u}:{p}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            var url = $"{EnviromentPizza.baseUrl}ingredient";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            //HttpResponseMessage result = await client.PostAsync(url, postData);
            HttpResponseMessage result = await client.GetAsync(url);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
            return response;
        }


        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if(textBoxAmount.Text == "")
            {
                MessageBox.Show("You need to give a value");
                return;
            }
            Zutat zutat = (Zutat) dataGrid.SelectedItem;
            if(zutat == null) {
                MessageBox.Show("You need to select an ingredient first");
                return; }
            int updatedAmount = zutat.zu_amount + int.Parse(textBoxAmount.Text);

            bool test = await updateZutat(zutat, updatedAmount);
            

            if (test)
            {
                labelConfirm.Content = "Succesfully bought " + zutat.zu_name;
                labelConfirm.Visibility = Visibility.Visible;
                labelConfirm.Background = Brushes.ForestGreen;

                await Task.Delay(3000);

                labelConfirm.Visibility = Visibility.Hidden;
            }
            else {  }
            
        }

        private async Task<bool> updateZutat(Zutat zutat, int amount) {
            //TODO call to API, update the Zutat

            var authData = Encoding.ASCII.GetBytes($"{name}:{password}"); //\todo Backend: BasicAuth für alle User bei Login nötig
            var response = string.Empty;

            zutat.zu_amount = amount;

            var json = JsonConvert.SerializeObject(zutat);
            var postData = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{EnviromentPizza.baseUrl}ingredient/"+zutat.zu_id;


            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authData));
            HttpResponseMessage result = await client.PutAsync(url, postData);
            //HttpResponseMessage result = client.PostAsync(url, postData).Result;

            response = await result.Content.ReadAsStringAsync();
            return true;
        }

        private void mockUpdate(Zutat zutat, int amount)
        {
            foreach (Zutat tmpZutat in zutatenList)
            {
                if (zutat.zu_id == tmpZutat.zu_id)
                {
                    tmpZutat.zu_amount = amount;
                    updateGrid();
                    return;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BackerStartWindow backerStartWindow = new BackerStartWindow(name, password);
            this.Close();
            backerStartWindow.ShowDialog();

        }
    }
}
