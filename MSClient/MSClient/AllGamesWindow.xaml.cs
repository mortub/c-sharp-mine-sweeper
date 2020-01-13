using MSClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AllGamesWindow.xaml
    /// </summary>
    public partial class AllGamesWindow : Window
    {
        public MSServiceClient client;
        public AllGamesWindow()
        {
            InitializeComponent();
        }
        public DataTable dt1 = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dt1 = new System.Data.DataTable();
            dt1.Columns.Add("Date");
            dt1.Columns.Add("Winner");
            dt1.Columns.Add("Loser");
            dt1.Columns.Add("Tie");

            string[][] games = client.ShowAllGames();
            foreach (var c in games)
            {
                dt1.Rows.Add(c[0], c[1], c[2], c[3]);
            }

            dgGames.ItemsSource = dt1.DefaultView;

        }
    }
}
