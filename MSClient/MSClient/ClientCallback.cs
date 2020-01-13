using MSClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MSClient
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallback: IMSServiceCallback
    {
            public delegate void PutMessageDelegate(string m, string fc);
            public event PutMessageDelegate putMessage;
            public void PutMessage(string message, string fromClient)
            {
                putMessage(message, fromClient);
            }

            public delegate void UpdateClientsDelegate(List<string> clients);
            public event UpdateClientsDelegate updateClients;
            public void UpdateClientsList(List<string> clients)
            {
                updateClients(clients);
            }

          public void UpdateClientsList(string[] clients)
          {
            updateClients(clients.ToList());
          }

        public delegate void PutRequestDelegate(string fromClient);
        public event PutRequestDelegate putRequest;
        public void PutRequest(string fromClient)
        {
            putRequest(fromClient);
        }

        public delegate void RequestDeniedDelegate(string fromClient);
        public event RequestDeniedDelegate requestDenied;
        public void RequestDenied(string fromClient)
        {
            requestDenied(fromClient);
        }

        public delegate void OpenGameDelegate(string fromClient, int size, int mines, Tuple<int, int>[] minesPositions, bool flag);
        public event OpenGameDelegate openGame;
        public void OpenGame(string fromClient,int size,int mines, Tuple<int, int>[] minesPositions, bool flag)
        {
            openGame(fromClient, size,mines,minesPositions,flag);
        }

        public delegate void Button_Click_ReactionDelegate(int row, int col,int tag, string fromClient,string other);
        public event Button_Click_ReactionDelegate button_Click_Reaction;
        public void Button_Click_Reaction(int row, int col,int tag, string fromClient, string other)
        {
            button_Click_Reaction(row,col,tag,fromClient,other);
        }

        public delegate void Right_Button_Click_ReactionDelegate(int row, int col, int tag, string fromClient);
        public event Right_Button_Click_ReactionDelegate right_button_Click_Reaction;
        public void Right_Button_Click_Reaction(int row, int col, int tag, string fromClient)
        {
            right_button_Click_Reaction(row, col, tag, fromClient);
        }

        public delegate void YouWinDelegate();
        public event YouWinDelegate youWin;
        public void YouWin()
        {
            youWin();
        }

        public delegate void UserExitDelegate(string activator, string other);
        public event UserExitDelegate userExit;
        public void UserExit(string activator, string other)
        {
            userExit(activator, other);
        }

        
    }
}
