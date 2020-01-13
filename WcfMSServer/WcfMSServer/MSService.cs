using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace WcfMSServer
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
     ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class MSService : IMSService
    {
            SortedDictionary<string, IMSServiceCallback> callbacks
                = new SortedDictionary<string, IMSServiceCallback>();

        List<Games> games = new List<Games>();
        static int num_of_games = 2;

        public void ClientConnected(string username)
            {
                if (callbacks.ContainsKey(username))
                {
                    throw new FaultException<UserExistsFault>(
                        new UserExistsFault
                        {
                            Message = username + " already exists"
                        });
                }
                try
                {
                    IMSServiceCallback callback =
                        OperationContext.Current.GetCallbackChannel
                        <IMSServiceCallback>();
                    callbacks.Add(username, callback);
                    UpdateUsersList();
                }
                catch (Exception ex)
                {
                    throw new FaultException(ex.Message);
                }
            }

            private void UpdateUsersList()
            {
                Thread updateListThread = new Thread(() =>
                {
                    foreach (var callback in callbacks.Values)
                    {
                        callback.UpdateClientsList(callbacks.Keys);
                    }
                });
                updateListThread.Start();
            }

            public void ClientDisconnected(string username)
            {
                callbacks.Remove(username);
                UpdateUsersList();
            }

        public bool SearchUsernamePasswordInDB(Clients client)
        {
            using (MineSweeperDB1Entities ctx = new MineSweeperDB1Entities())
            {
                var cli = from c in ctx.Clients
                          where c.UserName == client.UserName
                          && c.Password == client.Password
                          select c.UserName;
                //if the client exists in the database
                if (cli.ToList().Count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool EnterClientToDB(Clients client)
        {
            bool value;
            using (MineSweeperDB1Entities ctx = new MineSweeperDB1Entities())
            {
                var cli = from c in ctx.Clients
                          where c.UserName == client.UserName
                          select c.UserName;

                if (cli.ToList().Count == 0)
                {
                    //enter username+password to the db as a new client
                    ctx.Clients.Add(client);
                    ctx.SaveChanges();
                    value = true;
                }
                //if the username already exists, put a message
                else
                {
                    value = false;
                }
            }
            return value;
        }
        public void SendRequest(string fromClient, string toClient)
        {
            Thread putRequestThread = new Thread(() =>
            {
                callbacks[toClient].PutRequest(fromClient);
            });
            putRequestThread.Start();
        }

        public string[][] ShowAllPlayers()
        {
            string[][] clients = null;
            using (MineSweeperDB1Entities ctx = new MineSweeperDB1Entities())
            {
                clients = new string[ctx.Clients.Count()][];
                for(int i=0; i < ctx.Clients.Count(); i++)
                {
                    Clients c = ctx.Clients.ToArray()[i];
                    //the games c won
                    var gamesCWon = (from g in ctx.Games
                                     where g.Winner == c.UserName
                                     select g).ToArray();
                    //the games c lost
                    var gamesCLost = (from g in ctx.Games
                                      where g.Loser == c.UserName
                                      select g).ToArray();
                    //the games where there was a tie and c participated
                    var gamesTie = from g in ctx.Games
                                   where g.tie == true &&
                                   (g.Loser == c.UserName 
                                   || g.Winner == c.UserName)
                                   select g;

                   double gamesPlayed = gamesCWon.Count() + gamesCLost.Count();
                    double percent;
                    if (gamesPlayed == 0)
                        percent = 0;
                    else
                        percent = (gamesCWon.Count() / gamesPlayed) * 100;
                    clients[i] = new string[6];
                    for(int j = 0; j < 6; j++)
                    {
                        if(j == 0)
                            clients[i][j] = ctx.Clients.ToArray()[i].UserName;
                        if (j == 1)
                            clients[i][j] = gamesPlayed.ToString();
                        if (j == 2)
                            clients[i][j] = gamesCWon.Count().ToString();
                        if (j == 3)
                            clients[i][j] = gamesCLost.Count().ToString();
                        if (j == 4)
                            clients[i][j] = gamesTie.Count().ToString();
                        if (j == 5)
                            clients[i][j] = percent.ToString();


                    }
                }
                    
            }
            return clients;
        }

        public string[][] ShowAllGames()
        {
            string[][] GList = null;
            using (MineSweeperDB1Entities ctx = new MineSweeperDB1Entities())
            {
                GList = new string[ctx.Games.Count()][];
                           
                 for (int i=0; i< ctx.Games.Count(); i++)
                 {
                    GList[i] = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        string isTie = ctx.Games.ToArray()[i].tie ? "yes" : "no";
                        if(j== 0)
                            GList[i][j] = ctx.Games.ToArray()[i].EndingDate.ToString();
                        if(j==1)
                            GList[i][j] = ctx.Games.ToArray()[i].Winner;
                        if(j==2)
                            GList[i][j] = ctx.Games.ToArray()[i].Loser;
                        if(j==3)
                            GList[i][j] = isTie;
                    }
                    
                 }
                 
            }
            return GList;
        }

        public void SendRequestDenied(string fromClient, string toClient)
        {
            Thread putRequestDeniedThread = new Thread(() =>
            {
                callbacks[toClient].RequestDenied(fromClient);
            });
            putRequestDeniedThread.Start();
        }
       public void OpenBoardGame(string fromClient, string toClient,int size,int mines, Tuple<int, int>[] minesPositions)
        {
            Thread OpenBoardRivalThread = new Thread(() =>
            {
                callbacks[toClient].OpenGame(fromClient,size,mines, minesPositions,true);
                callbacks[fromClient].OpenGame(toClient, size, mines, minesPositions, false);
            });
            OpenBoardRivalThread.Start();
        }
        public void CallButtonClick(int row, int col,int tag, string fromClient,string toClient)
        {
            Thread CallButtonClickThread = new Thread(() =>
            {
                callbacks[toClient].Button_Click_Reaction(row,col,tag, fromClient,toClient);
                callbacks[fromClient].Button_Click_Reaction(row, col,tag, fromClient, toClient);
            });
            CallButtonClickThread.Start();
        }

        public void CallRightButtonClick(int row, int col, int tag, string fromClient, string toClient)
        {
            Thread CallRightButtonClickThread = new Thread(() =>
            {
                callbacks[toClient].Right_Button_Click_Reaction(row,col,tag,fromClient);
                callbacks[fromClient].Right_Button_Click_Reaction(row, col, tag, fromClient);
            });
            CallRightButtonClickThread.Start();
        }

        public void CallYouWin(string toClient)
        {
            Thread YouWinThread = new Thread(() =>
            {
                callbacks[toClient].YouWin();
            });
            YouWinThread.Start();
        }
        public void GameConnect(Games game)
        {
            games.Add(game);
        }

        public void GameDisconnect(Games game)
        {
            games.Remove(game);
        }
        public List<string> ShowOngoingGames()
        {
            List<string> gamesToString = new List<string>();
           
            foreach (var g in games)
            {
                if (g == null)
                {
                    gamesToString.Add(" ");
                    return gamesToString;
                }
                string sgame = "player 1: " + g.Winner + " player 2: " + g.Loser;
                gamesToString.Add(sgame);
            }
            return gamesToString;
        }
       
        public void EnterGameToDB(Games game)
        {
            using (MineSweeperDB1Entities ctx = new MineSweeperDB1Entities())
            {
                var query = (from g in ctx.Games
                             select g.Id).Max();

                game.Id = query+1;
                ctx.Games.Add(game);
                num_of_games = game.Id;
                ctx.SaveChanges();
            }
            GameDisconnect(game);
        }

        public void CallUserExit(string Username, string Rival)
        {
            Thread UserExitThread = new Thread(() =>
            {
               callbacks[Rival].UserExit(Username, Rival);
            });
            UserExitThread.Start();
            
        }

    }
}
