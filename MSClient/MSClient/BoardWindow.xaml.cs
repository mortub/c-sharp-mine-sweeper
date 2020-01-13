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
    /// Interaction logic for BoardWindow.xaml
    /// </summary>
    public partial class BoardWindow : Window
    {
        public MSServiceClient Client;
        public ClientCallback Callback;
        public string Username;
        public string Rival;
        public int size;
        public int mines;
        public bool flag;
        public bool bombFlag = false;
        public MinesGrid Mines { get; private set; }
        private bool gameStarted;
        private Color[] mineText;
        public MSClient.ServiceReference1.Games game { get; set; } = null;

        public BoardWindow(int mines, int size, Tuple<int, int>[] minesPositions,
            string username, string rival, MSServiceClient client, ClientCallback callback, bool flag)
        {

            InitializeComponent();
            this.size = size;
            this.mines = mines;
            this.Username = username;
            this.Rival = rival;
            Title = username;
            this.Client = client;
            this.Callback = callback;
            this.flag = flag;

            for (int i = 0; i < size * size; ++i)
            {
                Button button = new Button()
                {
                    Tag = i,
                };

                button.Click += new RoutedEventHandler(Button_Click);
                button.MouseRightButtonDown += new MouseButtonEventHandler(Right_Button_Click);
                this.ButtonsGrid.Children.Add(button);
                this.ButtonsGrid.Columns = size;
                this.ButtonsGrid.Rows = size;
            }
            gameStarted = false;
            mineText = new Color[] {Colors.White /*first doesn't matter*/ ,
                                    Colors.Blue, Colors.DarkGreen, Colors.Red, Colors.DarkBlue,
                                    Colors.DarkViolet, Colors.DarkCyan, Colors.Brown, Colors.Black
                        };
            GameSetup(minesPositions);

            game = new MSClient.ServiceReference1.Games()
            {
                Winner = Rival,
                Loser = Username
            };
            if( flag == true)
                Client.GameConnect(game);

        }

        private void GameSetup(Tuple<int, int>[] minesPositions)
        {
            Mines = new MinesGrid(size, size, mines);
            foreach (Button btn in ButtonsGrid.Children)
            {
                btn.Content = ""; // clears flag or bomb image (if any)
                btn.IsEnabled = true; // button gets clickable
            }

            // Attaches Button Click, invoked by a plate
            Mines.ClickPlate += OnClickPlate;
            Mines.Run(minesPositions);
            gameStarted = true;
        }
      
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Callback.button_Click_Reaction += Button_Click_Reaction;
            Callback.right_button_Click_Reaction += Right_Button_Click_Reaction;
            Callback.youWin += YouWin;
            Callback.userExit += UserExit;
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender; // gets clicked button reference
            int row = ParseButtonRow(btn);
            int col = ParseButtonColumn(btn);
            //calls Button_Click_Reaction in both clients
            Client.CallButtonClick(row, col,(int)btn.Tag, Username, Rival);
        }
        private void Button_Click_Reaction(int row, int col,int tag, string activator/*to know who lost*/, string other)
        {
            RoutedEventArgs e = new RoutedEventArgs();
            Button btn= find_button_by_tag(tag);
            if (btn == null) throw new MinesweeperException("Invalid Button to MinesGrid reference on multiple reveal"); // the plate refers to an invalid button

            if (!Mines.IsInGrid(row, col)) throw new MinesweeperException("Invalid Button to MinesGrid reference on reveal"); // the button points to an invalid plate

            if (Mines.IsFlagged(row, col)) return; // flagged plate cannot be revealed

            btn.IsEnabled = false; // disables the button
            if (Mines.IsBomb(row, col)) //a bomb was revealed !!! 
            {
                // attaches bomb image to the button
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Image bombImage = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"..\..\bomb.png", UriKind.Relative);
                bi.EndInit();
                bombImage.Source = bi;
                bombImage.Width = bi.PixelWidth;
                bombImage.Height = bi.PixelHeight;
                sp.Children.Add(bombImage);
                btn.Content = sp;
                Mines.Stop();

                // finishes the game and opens all plates
                if (gameStarted)
                {
                    gameStarted = false;
                   /* foreach (Button butn in ButtonsGrid.Children)
                    {
                        if (butn.IsEnabled) this.Button_Click(butn, e); // calls all other unrevealed buttons
                        System.Threading.Thread.Sleep(5000);
                    }*/

                }
                if (flag==true)
                {
                    game.Winner = other;
                    game.Loser = activator;
                    game.EndingDate = DateTime.Now;
                    game.tie = false;               
                    Client.EnterGameToDB(game);
                }
                bombFlag = true;
                if (Username == activator)
                    YouLose();
                if (Username == other)
                    YouWin();


                this.Close();
            }
            else // an empty space was revealed
            {
                int count = Mines.RevealPlate(row, col); // opens the plate and checks for surrounding bombs
                if (count > 0) // put a corresponding label on the current button
                {
                    btn.Foreground = new SolidColorBrush(mineText[count]);
                    btn.FontWeight = FontWeights.Bold;
                    btn.Content = count.ToString();
                }
            }
        }
        
        private Button find_button_by_tag(object tag)
        {
            foreach(Button b in ButtonsGrid.Children)
            {
                if (b.Tag.ToString() == tag.ToString())
                    return b;
            }
            return null;           
        }
        //being called on windo closing
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Client.CallUserExit(Username, Rival);
        }
        private void UserExit(string activator, string other)
        {
            //if a bomb wasn't revealed 
            if (flag == true && bombFlag ==false)
            {
                game.Winner = other;
                game.Loser = activator;
                game.EndingDate = DateTime.Now;
                game.tie = false;
                Client.EnterGameToDB(game);
                //Client.CallYouWin(other);
            }

        }
        public void Right_Button_Click(object sender, MouseButtonEventArgs e)
        {
            Button btn = (Button)sender; // gets clicked button reference
            int row = ParseButtonRow(btn);
            int col = ParseButtonColumn(btn);
            Client.CallRightButtonClick(row, col, (int)btn.Tag, Username, Rival);
        }

        private void Right_Button_Click_Reaction(int row,int col, int tag, string activator/*to know who lost*/)
        {
            Button btn = find_button_by_tag(tag);
            if (btn == null) throw new MinesweeperException("Invalid Button to MinesGrid reference on multiple reveal");

            if (!Mines.IsInGrid(row, col)) throw new MinesweeperException("Invalid Button to MinesGrid reference on flag"); // the button points to an invalid plate

            if (Mines.IsFlagged(row, col)) // the button has flag image child
            {
                btn.Content = ""; // clears flag image
            }
            else
            {
                // attaches flag image to the button
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Image flagImage = new Image();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"..\..\flag.png", UriKind.Relative);
                bi.EndInit();
                flagImage.Source = bi;
                flagImage.Width = bi.PixelWidth;
                flagImage.Height = bi.PixelHeight;
                sp.Children.Add(flagImage);
                btn.Content = sp;
            }

            Mines.FlagMine(row, col);
        }

        private int ParseButtonRow(Button btn)
        {
            int row = 0;
            Int32.TryParse(btn.Tag.ToString(), out int num);
            if (num < size)
                row = 0;
            else if (num >= size && num < size * 2)
                row = 1;
            else if (num >= size * 2 && num < size * 3)
                row = 2;
            else if (num >= size * 3 && num < size * 4)
                row = 3;
            else if (num >= size * 4 && num < size * 5)
                row = 4;
            else if (num >= size * 5 && num < size * 6)
                row = 5;
            else if (num >= size * 6 && num < size * 7)
                row = 6;
            else if (num >= size * 7 && num < size * 8)
                row = 7;
            else if (num >= size * 8 && num < size * 9)
                row = 8;
            else if (num >= size * 9 && num < size * 10)
                row = 9;
            else if (num >= size * 10 && num < size * 11)
                row = 10;
            else if (num >= size * 11 && num < size * 12)
                row = 11;
            else if (num >= size * 12 && num < size * 13)
                row = 12;
            else if (num >= size * 13 && num < size * 14)
                row = 13;
            else if (num >= size * 14 && num < size * 15)
                row = 14;
            else if (num >= size * 15 && num < size * 16)
                row = 15;

            return row;
        }

        private int ParseButtonColumn(Button btn)
        {
            int col = 0;
            Int32.TryParse(btn.Tag.ToString(), out int num);
            if (num % size == 0)
                col = 0;
            else if (num % size == 1)
                col = 1;
            else if (num % size == 2)
                col = 2;
            else if (num % size == 3)
                col = 3;
            else if (num % size == 4)
                col = 4;
            else if (num % size == 5)
                col = 5;
            else if (num % size == 6)
                col = 6;
            else if (num % size == 7)
                col = 7;
            else if (num % size == 8)
                col = 8;
            else if (num % size == 9)
                col = 9;
            else if (num % size == 10)
                col = 10;
            else if (num % size == 11)
                col = 11;
            else if (num % size == 12)
                col = 12;
            else if (num % size == 13)
                col = 13;
            else if (num % size == 14)
                col = 14;
            else if (num % size == 15)
                col = 15;

            return col;
        }

        private void OnClickPlate(object sender, PlateEventArgs e)
        {
            // Opens requested plate through simulating Button Click
            string btnName = "Button";
            if (e.PlateRow < 10)
            {
                btnName += String.Format("0{0}", e.PlateRow);
            }
            else
            {
                btnName += String.Format("{0:D2}", e.PlateRow);
            }
            if (e.PlateColumn < 10)
            {
                btnName += String.Format("0{0}", e.PlateColumn);
            }
            else
            {
                btnName += String.Format("{0:D2}", e.PlateColumn);
            }


            Button senderButton = (ButtonsGrid.FindName(btnName) as Button);
            if (senderButton == null) throw new MinesweeperException("Invalid Button to MinesGrid reference on multiple reveal"); // the plate refers to an invalid button

            // calls respecive "Button Click" event handler 
            this.Button_Click(senderButton, new RoutedEventArgs());
        }
        private void OnCounterChanged(object sender, EventArgs e)
        {
            // Updates MineIndicator field in the UI
            //MinesIndicator.Text = (this.nrMines - Mines.FlaggedMines).ToString();
        }

        private void OnTimeChanged(object sender, EventArgs e)
        {
            // Updates MineIndicator field in the UI
            //TimeIndicator.Text = Mines.TimeElapsed.ToString();
        }

        private void YouWin()
        {
            var res = MessageBox.Show("YOU WIN!"
                , "YOU WIN", MessageBoxButton.OK);
            this.Close();
        }

        private void YouLose()
        {
            var res = MessageBox.Show("YOU LOSE!"
               , "YOU LOSE", MessageBoxButton.OK);
            this.Close();
        }

        private void Tie()
        {
            var res = MessageBox.Show("TIE!"
               , "TIE", MessageBoxButton.OK);
            this.Close();
        }

       
    }
}
