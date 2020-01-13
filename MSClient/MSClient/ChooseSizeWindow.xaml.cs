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
    /// Interaction logic for ChooseSizeWindow.xaml
    /// </summary>
    public partial class ChooseSizeWindow : Window
    {
        public ChooseSizeWindow()
        {
            InitializeComponent();
        }
        public MSServiceClient Client;
        public ClientCallback Callback;
        public string Username;
        public string Rival;
        public int size;
        public int mines;

        private void bOK_Click(object sender, RoutedEventArgs e)
        {
            //if the client didn't choose, sets default numbers
            if (string.IsNullOrEmpty(tbSize.Text))
            {
                size = 16;      
            }
            else
            {
                bool res=Int32.TryParse(tbSize.Text, out size);
                if (res == false)
                {
                    var result = MessageBox.Show("please enter a number"
                , "Error Size", MessageBoxButton.OK);
                    return;
                }
                if(size< 4 || size> 16)
                {
                    var result = MessageBox.Show("please enter a number between 4 to 16"
                , "Error Size", MessageBoxButton.OK);
                    return;
                }
            }
            if (string.IsNullOrEmpty(tbMines.Text))
            {
                mines = 10;
            }
            else
            {
                bool res=Int32.TryParse(tbMines.Text, out mines);
                if (res == false)
                {
                    var result = MessageBox.Show("please enter a number"
                , "Error Mines", MessageBoxButton.OK);
                    return;
                }
                if (mines< 1 || mines> 50)
                {
                    var result = MessageBox.Show("please enter a number between 1 to 50"
               , "Error Mines", MessageBoxButton.OK);
                    return;
                }
            }
            if (mines > size)
            {
                var result = MessageBox.Show("Choose less mines"
               , "Error Mines", MessageBoxButton.OK);
                return;
            }
            Random minesPosition = new Random();
            Tuple<int, int>[] minesPositions = new Tuple<int, int>[mines];
            for(int i=0; i < mines; i++)
            {
                int row = minesPosition.Next(size);
                int col = minesPosition.Next(size);
                minesPositions[i] = Tuple.Create<int, int>(row, col);
            }           

            Client.OpenBoardGame(Username,Rival,size,mines, minesPositions);
            this.Close();
        }
        
    }
}
