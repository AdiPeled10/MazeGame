﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace ViewModel
{
    /// <summary>
    /// Use to create event to notify that other player was connected;
    /// </summary>
    public delegate void NotifyPlayerConnected();

    /// <summary>
    /// Use to create event to notify that other player disconnected.
    /// </summary>
    public delegate void Parameterless();

    /// <summary>
    /// Notify when the opponent moves.
    /// </summary>
    /// <param name="direction"> the direction he moved in. </param>
    public delegate void OpponentMoved(string direction);

    /// <summary>
    /// This class connects between the model and the view.
    /// It is almost just a pipe.
    /// </summary>
    public class MultiPlayerVM : GameViewModel
    {
        /// <summary>
        /// List of games we can join.
        /// </summary>
        private ObservableCollection<string> joinableGames = new ObservableCollection<string>();

        /// <summary>
        /// Event to when the opponent has moved.
        /// </summary>
        public event OpponentMoved Movement;

        /// <summary>
        /// The direction the opponent moved.
        /// </summary>
        private string directionVM;

        /// <summary>
        /// DirectionVM property.
        /// Get or set "directionVM".
        /// If set, also calls "Movement".
        /// </summary>
        public string DirectionVM
        {
            get { return directionVM; }
            set
            {
                directionVM = value;
                Movement(value);
            }
        }

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
        private bool shouldStop = false;

        /// <summary>
        /// Event to notify that a player got conencted.
        /// </summary>
        public event NotifyPlayerConnected NotifyConnection;

        /// <summary>
        /// Event to notify that player got disconnected.
        /// </summary>
        public event Parameterless NotifyDisconnection;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public MultiPlayerVM() : base()
        {
            //Create the model that we will use to communicate with server.
            model.NotifyList += UpdateList;
            model.NotifyDirection += UpdateOpponentDirection;
            model.Disconnection += NoOpponent;
            //Open thread that listens to list of joinable games.
            //SendListThread();
        }

        /// <summary>
        /// Calls model.ListGames.
        /// </summary>
        public void ListCommand()
        {
            model.ListGames();
        }

        /// <summary>
        /// Do "exit" move that disconnect us from the server.
        /// </summary>
        public void Disconnected()
        {
            //Send exit message.
            model.PlayMove("exit");
        }

        /// <summary>
        /// Activates when the opponent got disconnected.
        /// </summary>
        public void NoOpponent()
        {
            NotifyDisconnection();
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
            shouldStop = true; 
           
            ThreadStart starter = ()=> { model.StartGame(name, rows, cols); };
            //Add callback
            starter += () => { NotifyConnection(); };
            Thread thread = new Thread(starter) { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            //Start thread.
            thread.Start();
        }

        /// <summary>
        /// Send list commands to the server.
        /// </summary>
        private void SendListThread()
        {
            Thread thread = new Thread(() =>
            {
                while (!shouldStop)
                {
                    //Update list of games.
                    GetListOfGames();
                    //Sleep 5 seconds TODO- Choose time to sleep later.
                    Thread.Sleep(5000);
                }
            });
            thread.Start();
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
            //Stop asking for list.
            shouldStop = true;
            model.JoinGame(name);
            //Connection immediatly cause we join.
            NotifyConnection();
        }

        /// <summary>
        /// Sets "JoinableGames" to the Input.
        /// JoinableGames is the list od joinable multi player games.
        /// </summary>
        /// <param name="games"> List of game names. </param>
        public void UpdateList(List<string> games)
        {
            //Update games
            JoinableGames = new ObservableCollection<string>(games);
        }

        /// <summary>
        /// Sets "DirectionVM".
        /// </summary>
        /// <param name="direction"> A string that represents a direction. </param>
        public void UpdateOpponentDirection(string direction)
        {
            DirectionVM = direction;
        }

        /// <summary>
        /// Send play to server in given direction.
        /// </summary>
        /// <param name="direction"></param>
        public void PlayInDirection(string direction)
        {
            model.PlayMove(direction);
        }

        /// <summary>
        /// Run thread that will listen to directions of opponent.
        /// </summary>
        public void ListenToOpponent()
        {
            model.GetOpponentMoves();
        }

        /// <summary>
        /// Close the client and stop all the threads.
        /// </summary>
        public void GameOver()
        {
            model.Stop = true;
            model.CloseClient(mazeName);
        }
    }
}
