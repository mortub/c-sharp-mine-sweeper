using MSClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ClientCallback callback;
        public MSServiceClient client;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbUsername.Text)&& !string.IsNullOrEmpty(tbPassword.Text))
            {
               ConnectToServer();
            }
            else
            {
                var result = MessageBox.Show("username or password missing", "error",
                            MessageBoxButton.OK);
            }
        }

        public string username;
        private void ConnectToServer()
        {
            callback = new ClientCallback();
            try
            {
                client = new MSServiceClient(
                        new InstanceContext(callback));

                //checking if this user exists, if not, receives a message
                MSClient.ServiceReference1.Clients c = new ServiceReference1.Clients();
                c.UserName = tbUsername.Text.Trim();
                c.Password = tbPassword.Text.Trim();
                bool value = client.SearchUsernamePasswordInDB(c);
                if( value == true)
                {
                    username = tbUsername.Text.Trim();
                    client.ClientConnected(username);
                    MenuWindow mainWindow = new MenuWindow();
                    mainWindow.Client = client;
                    mainWindow.Callback = callback;
                    mainWindow.Username = username;
                    mainWindow.Title = username;
                    this.Close();
                    mainWindow.Show();
                }
                else
                {
                    var res = MessageBox.Show("username or password wrong. please register", "error",
                      MessageBoxButton.OK);
                }
                
            }
            catch (FaultException<UserExistsFault> ex)
            {
                MessageBox.Show(ex.Detail.Message);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            callback = new ClientCallback();
            client = new MSServiceClient(
                       new InstanceContext(callback));
            RegisterWindow wind = new RegisterWindow();
            wind.client = client;
            wind.callback = callback;
            wind.ShowDialog();
        }
    }
}
