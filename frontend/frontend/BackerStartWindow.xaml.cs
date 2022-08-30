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
    /// Interaction logic for BackerStartWindow.xaml
    /// </summary>
    public partial class BackerStartWindow : Window
    {
        String name;
        String password;
        public BackerStartWindow(String name, string password)
        {
            this.name = name;
            this.password = password;
            InitializeComponent();
            textBlockName.Text = "Hello " + name;
            this.password = password;
        }

        private void buttonOrderSite_Click(object sender, RoutedEventArgs e)
        {
            IngredientsWindow ingredientsWindow = new IngredientsWindow(name, password);
            this.Close();
            ingredientsWindow.ShowDialog();
        }

        private void buttonZutaten_Click(object sender, RoutedEventArgs e)
        {
            BackerOrder backerOrder = new BackerOrder(name, password);
            this.Close();
            backerOrder.ShowDialog();
        }
    }
}
