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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public MSClient.ServiceReference1.Clients c;
        public ClientCallback callback;
        public MSServiceClient client;
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void register_button_click(object sender, RoutedEventArgs e)
        {
            if( !string.IsNullOrEmpty(tbUsername.Text)&& !string.IsNullOrEmpty(tbPassword.Text))
            {
                //searching for username in db. if exists, return false
                //else adds client to db and returns false
                c = new ServiceReference1.Clients();
                c.UserName = tbUsername.Text.Trim();
                c.Password = tbPassword.Text.Trim();
                bool value = client.EnterClientToDB(c);
                if (value == false)
                {
                    var res = MessageBox.Show("username already exists", "error",
                             MessageBoxButton.OK);
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                var result = MessageBox.Show("username or password missing", "error",
                            MessageBoxButton.OK);
            }
        }
    }
}
