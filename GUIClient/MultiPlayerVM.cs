using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Threading;
using GUIClient;

namespace ViewModel
{
    //Use to create event to notify that other player was connected;
    public delegate void NotifyPlayerConnected();

    //Use to create event to notify that other player disconnected.
    public delegate void NotifyPlayerDisconnected();


    public class MultiPlayerVM : GameViewModel
    {

        /// <summary>
        /// List of games we can join.
        /// </summary>
        private ObservableCollection<string> joinableGames = new ObservableCollection<string>();
        
        /// <summary>
        /// Property for list of joinable games.
        /// </summary>
        public ObservableCollection<string> JoinableGames
        {
            get { return joinableGames; }
            set
            {
                joinableGames = value;
                NotifyPropertyChanged("JoinableGames");
            }
        }

        /// <summary>
        /// For thread which will ask server for list of games every 20 seconds.
        /// </summary>
        private bool stop = false;

        /// <summary>
        /// Event to notify that a player got conencted.
        /// </summary>
        public event NotifyPlayerConnected NotifyConnection;

        /// <summary>
        /// Event to notify that player got disconnected.
        /// </summary>
        public event NotifyPlayerDisconnected NotifyDisconnection;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public MultiPlayerVM()
        {
            //Create the model that we will use to communicate with server.
            model = new ClientModel();
            
            //Open thread that listens to list of joinable games.
            SendListThread();
        }

        /// <summary>
        /// Start new multiplayer game.
        /// </summary>
        /// <param name="name">Name of game.  </param>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of cols.</param>
        public override void GenerateMaze(string name,int rows,int cols)
        {
            //Use model.
            /*In case of start command we will get the maze from the server only when
             * another player will connect to the game,that's why we will run this function
             * in async thread to not cause the UI to freeze and we will wait to be notified
             * by the event in client model.(Our code is event driven).
             */
            Thread thread = new Thread(() =>
            {
                model.GenerateMaze("start", name, rows, cols);
                //Notify listeners that opponent got connected.
                NotifyConnection();
            });

            //Start thread.
            thread.Start();
        }

        private void SendListThread()
        {
            Thread thread = new Thread(() =>
            {
                while (!stop)
                {
                    //Update list of games.
                    GetListOfGames();
                    //Sleep 5 seconds TODO- Choose time to sleep later.
                    Thread.Sleep(5000);
                }
            });
        }

        /// <summary>
        /// Get list of available games and notify the view.
        /// We will use an observable collection to store the list of games
        /// it will update and notify the view at any update of the list.
        /// </summary>
        public void GetListOfGames()
        {
            //Ask list of games from model with list command.
            model.ListGames();
        }


        /// <summary>
        /// Join game of the given name.
        /// </summary>
        /// <param name="name">Name of game we will join.</param>
        public void JoinGame(string name)
        {

        }
    }
}
