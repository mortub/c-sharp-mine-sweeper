using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfMSServer
{


    [ServiceContract(CallbackContract = typeof(IMSServiceCallback))]
    public interface IMSService
    {
        [OperationContract]
        [FaultContract(typeof(UserExistsFault))]
        //connects client
        void ClientConnected(string username);
        [OperationContract]
        //disconnects client
        void ClientDisconnected(string username);
        [OperationContract]
        //searches username+password in db
        bool SearchUsernamePasswordInDB(Clients client);
        [OperationContract]
        //enters username to db
        bool EnterClientToDB(Clients client);
        [OperationContract]
        //sends a game request to toClient
        void SendRequest(string fromClient, string toClient);
        [OperationContract]
        //return DataTable of all players in db
        string[][] ShowAllPlayers();
        [OperationContract]
        //returns DataTable of all the games in db
        string[][] ShowAllGames();
        [OperationContract]
        //sends game decline to toClient
        void SendRequestDenied(string fromClient, string toClient);
        [OperationContract]
        //opens the game
        void OpenBoardGame(string fromClient, string toClient, int size, int mines, Tuple<int, int>[] minesPositions);
        [OperationContract]
        //activates button click in both clients
        void CallButtonClick(int row, int col,int tag, string fromClient, string toClient);
        [OperationContract]
        //activates right button click in both clients
        void CallRightButtonClick(int row, int col, int tag, string fromClient, string toClient);
        [OperationContract]
        //sends you win message
        void CallYouWin(string toClient);
        [OperationContract]
        //adds game to the list of ongoing games
        void GameConnect(Games game);
        [OperationContract]
        //deletes game from the list of ongoing games
        void GameDisconnect(Games game);
        [OperationContract]
        //enters game to database
        void EnterGameToDB(Games game);
        [OperationContract]
        //returns the list of ongoing games
        List<string> ShowOngoingGames();
        [OperationContract]
        //when user presses exit button, server activates that function
       void CallUserExit(string Username, string Rival);
    }
   
    public interface IMSServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        //sends a message to client
        void PutMessage(string message ,string fromClient);
        [OperationContract(IsOneWay = true)]
        //updates client list-when a clients is added/ deleted
        void UpdateClientsList(IEnumerable<string> clients);
        [OperationContract(IsOneWay = true)]
        //puts a game request
        void PutRequest(string fromClient);
        //sends denied request from username
        [OperationContract(IsOneWay = true)]
        //if the game request was declined, sends a message
        void RequestDenied(string username);
        [OperationContract(IsOneWay = true)]
        //opens a board window
        void OpenGame(string fromClient, int size, int mines,Tuple<int,int>[] minesPositions, bool flag);
        [OperationContract(IsOneWay = true)]
        //chang of board due to button click
        void Button_Click_Reaction(int row, int col,int tag, string fromClient, string other);
        [OperationContract(IsOneWay = true)]
        //change of board due to right button click
        void Right_Button_Click_Reaction(int row, int col, int tag, string fromClient);
        [OperationContract(IsOneWay = true)]
        //sends you win message
        void YouWin();
        [OperationContract(IsOneWay = true)]
        //when user presses exit button, server activates that function 
        void UserExit(string activator, string other);
    }
}
