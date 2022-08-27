using frontend.Objects;
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
    /// Interaction logic for Ingredients.xaml
    /// </summary>
    public partial class IngredientsWindow : Window
    {
        String name;
        int runs = 0;
        List<Zutat> zutatenList;
        public IngredientsWindow(String name)
        {
            this.name = name;
            InitializeComponent();
            updateGrid();
        }

        private async void updateGrid() {
            dataGrid.ItemsSource = null;
            if(runs == 0)
            {
                zutatenList = getZutatenFromApi();
                runs++;
            }            

            dataGrid.ItemsSource = zutatenList;
        }

        private List<Zutat> getZutatenFromApi() { 
            //TODO Create API call to get all the Zutaten 
                return createMockData();
            
            
        }
        private List<Zutat> createMockData()
        {
            List<Zutat> zutatList = new List<Zutat>();
            Zutat zutat = new Zutat();
            zutat.zutatID = 0;
            zutat.zutatName = "Salt";
            zutat.amount = 300;

            Zutat zutat1 = new Zutat();
            zutat1.zutatID = 1;
            zutat1.zutatName = "Pepper";
            zutat1.amount = 200;

            Zutat zutat2 = new Zutat();
            zutat2.zutatID = 2;
            zutat2.zutatName = "Dough";
            zutat2.amount = 20;

            zutatList.Add(zutat);
            zutatList.Add(zutat1);
            zutatList.Add(zutat2);

            return zutatList;
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
            int updatedAmount = zutat.amount + int.Parse(textBoxAmount.Text);

            if (updateZutat(zutat, updatedAmount))
            {
                labelConfirm.Content = "Succesfully bought " + zutat.zutatName;
                labelConfirm.Visibility = Visibility.Visible;
                labelConfirm.Background = Brushes.ForestGreen;

                await Task.Delay(3000);

                labelConfirm.Visibility = Visibility.Hidden;
            }
            else {  }
            
        }

        private bool updateZutat(Zutat zutat, int amount) {
            //TODO call to API, update the Zutat
            mockUpdate(zutat, amount);
            return true;
        }

        private void mockUpdate(Zutat zutat, int amount)
        {
            foreach (Zutat tmpZutat in zutatenList)
            {
                if (zutat.zutatID == tmpZutat.zutatID)
                {
                    tmpZutat.amount = amount;
                    updateGrid();
                    return;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BackerStartWindow backerStartWindow = new BackerStartWindow(name);
            this.Close();
            backerStartWindow.ShowDialog();

        }
    }
}
