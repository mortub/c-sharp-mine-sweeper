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
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }
        public MSServiceClient Client;
        public ClientCallback Callback;
        public string Username;
        
        private void one_player_button_click(object sender, RoutedEventArgs e)
        {
            //opens a game for one player
            OnePlayerChooseWindow wind = new OnePlayerChooseWindow();
            wind.Username = Username;
            wind.ShowDialog();
        }

        private void ongoing_games_button_click(object sender, RoutedEventArgs e)
        {
            //shows the games that are happening now
            OngoingGames wind = new OngoingGames();
            wind.Client = Client;
            wind.ShowDialog();
        }

        private void all_games_button_click(object sender, RoutedEventArgs e)
        {
            //shows all games in data base
            AllGamesWindow wind = new AllGamesWindow();
            wind.client = Client;
            wind.ShowDialog();
        }
        private void all_players_button_click(object sender, RoutedEventArgs e)
        {
            //shows all clients in data base
            AllPlayersWindow wind = new AllPlayersWindow();
            wind.client = Client;
            wind.ShowDialog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Callback.updateClients += UpdateClients;
            Callback.putRequest += PutRequest;
            Callback.requestDenied += RequestDenied;
            Callback.openGame += OpenGame;
        }
        private void UpdateClients(List<string> clients)
        {
            lbUsers.ItemsSource = clients;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Client.ClientDisconnected(Username);
            Environment.Exit(Environment.ExitCode);
        }
        private void bsendRequest_Click(object sender, RoutedEventArgs e)
        {
            //if the username chose himself for the game, send a message and return
            if (lbUsers.SelectedItem.ToString() == Username)
            {
                var result = MessageBox.Show("You can't send a game request to yourself"
             , "Game Request", MessageBoxButton.OK);
                return;
            }
            Client.SendRequest(Username, lbUsers.SelectedItem.ToString());
        }
     
        private void PutRequest(string fromClient)
        {
            var result = MessageBox.Show(fromClient + " invites you to play, do you accept?"
              , "Game Request", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {  

                ChooseSizeWindow choose = new ChooseSizeWindow();
                choose.Client = Client;
                choose.Callback = Callback;
                choose.Username = Username;
                choose.Rival = fromClient;
                choose.ShowDialog();
                


            }
            if(result == MessageBoxResult.No)
            {
                Client.SendRequestDenied(Username, fromClient);
            }
        }

       
        private void RequestDenied(string fromClient)
        {
            var res = MessageBox.Show(fromClient + " does not want to play"
                , "Game Request denied", MessageBoxButton.OK);
        }

        private void OpenGame(string fromClient,int size,int mines, Tuple<int, int>[] minesPositions,bool flag)
        {
           
            BoardWindow wind = new BoardWindow(mines,size, minesPositions,
                Username, fromClient, Client, Callback,flag);
           
            wind.ShowDialog();
        }




    }
}
