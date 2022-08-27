using frontend.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace frontend
{
    /// <summary>
    /// Interaction logic for BackerOrder.xaml
    /// </summary>
    public partial class BackerOrder : Window
    {
        int runs = 0;
        String name;
        List<Bestellung> gridData;
        Bestellung currentBestellungInOven;
        bool pizzaInTheOven = false;

        Bestellung currentBestellungSending;
        bool pizzaDriverDriving = false;
        public BackerOrder(String name)
        {
            this.name = name;
            InitializeComponent();
            getOrdersAsync();
        }

        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            Bestellung bestellung = (Bestellung) dataGrid.SelectedItem;
            if (bestellung == null)
            {
                MessageBox.Show("You need to select an Order first");
                return;
            }
            updateBestellung(bestellung, "Order accepted");
        }

        private void updateBestellung(Bestellung bestellung, String status) {
            //TODO update Bestellung in the API
            mockUpdate(bestellung, status);

        }
        private void mockUpdate(Bestellung bestellung, String status)
        {
            foreach(Bestellung bestellung1 in gridData) {
                if(bestellung.be_id == bestellung1.be_id)
                {
                    bestellung1.status = status;
                    getOrdersAsync();
                    return;
                }
            }
        }

        //This should get all the Orders from the API where the username matches
        private async void getOrdersAsync() {
            //Testing
            if (runs == 0)
            {
                runs++;
                gridData = mockData();
            }
            
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = gridData;
        }

        private List<Bestellung> mockData()
        {
            
            List<Bestellung> bestellungs = new List<Bestellung>();

            Bestellung bestellung = new Bestellung();
            bestellung.status = "Order given";
            bestellung.ac_id = 1;
            bestellung.p_id = 1;
            bestellung.be_id = 1;
            bestellungs.Add(bestellung);

            Bestellung bestellung1 = new Bestellung();
            bestellung1.status = "Order given";
            bestellung1.ac_id = 2;
            bestellung1.p_id = 2;
            bestellung1.be_id = 2;
            bestellungs.Add(bestellung1);

            return bestellungs;

        }
        private void buttonHome_Click(object sender, RoutedEventArgs e)
        {
            BackerStartWindow backerStartWindow = new BackerStartWindow(name);
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
            currentBestellungInOven = (Bestellung)dataGrid.SelectedItem;
            if(currentBestellungInOven == null)
            {
                MessageBox.Show("You need to select an Order first");
                return;
            }
            if(currentBestellungInOven.status == "Order accepted")
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
            currentBestellungSending = (Bestellung)dataGrid.SelectedItem;
            if (currentBestellungSending == null)
            {
                MessageBox.Show("You need to select an Order first");
                return;
            }
            if (currentBestellungSending.status == "Pizza made")
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
