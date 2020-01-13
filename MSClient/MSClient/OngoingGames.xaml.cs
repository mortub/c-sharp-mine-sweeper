using MSClient.ServiceReference1;
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

namespace MSClient
{
    /// <summary>
    /// Interaction logic for OngoingGames.xaml
    /// </summary>
    public partial class OngoingGames : Window
    {
        public MSServiceClient Client;
        public OngoingGames()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lbGames.ItemsSource=Client.ShowOngoingGames();
        }

    }
}
