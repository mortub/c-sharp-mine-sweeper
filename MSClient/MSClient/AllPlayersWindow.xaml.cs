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
    /// Interaction logic for AllPlayersWindow.xaml
    /// </summary>
    public partial class AllPlayersWindow : Window
    {
        public MSServiceClient client;
        public AllPlayersWindow()
        {
            InitializeComponent();
        }
        public DataTable dt1 = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[][] clients= client.ShowAllPlayers();
            dt1 = new System.Data.DataTable();
            dt1.Columns.Add("Player Name");
            dt1.Columns.Add("Number Of Games");
            dt1.Columns.Add("Number Of Wins");
            dt1.Columns.Add("Number Of Loses");
            dt1.Columns.Add("Number Of Ties");
            dt1.Columns.Add("Percentage Of Wins");
            foreach(var c in clients)
            {
                dt1.Rows.Add(c[0],c[1],c[2],c[3],c[4],c[5]);
            }
            
            dgPlayers.ItemsSource = dt1.DefaultView;
        }
    }
}
