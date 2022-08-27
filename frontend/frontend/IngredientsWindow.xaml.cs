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
        public IngredientsWindow(String name)
        {
            this.name = name;
            InitializeComponent();
            updateGrid();
        }

        private async void updateGrid() {
            dataGrid.ItemsSource = null;
            List <String> dataList = new List<string>();
            dataList.Add("Salami");
            dataList.Add("Dough");

            List<Zutat> zutatenList = getZutatenFromApi();

            dataGrid.ItemsSource = zutatenList;
        }

        private List<Zutat> getZutatenFromApi() { 
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
            Zutat zutat = (Zutat) dataGrid.SelectedItem;
            int updatedAmount = zutat.amount + int.Parse(textBoxAmount.Text);

            if (updateZutat(updatedAmount, zutat.zutatID))
            {
                labelConfirm.Content = "Succesfully bought " + zutat.zutatName;
                labelConfirm.Visibility = Visibility.Visible;
                labelConfirm.Background = Brushes.ForestGreen;

                await Task.Delay(3000);

                labelConfirm.Visibility = Visibility.Hidden;
            }
            else {  }
            
        }

        private bool updateZutat(int updatedAmount, int id) {
            //TODO call to API, update the Zutat

            return true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BackerStartWindow backerStartWindow = new BackerStartWindow(name);
            this.Close();
            backerStartWindow.ShowDialog();

        }
    }
}
