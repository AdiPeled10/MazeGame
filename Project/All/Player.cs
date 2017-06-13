using MazeLib;
using ClientForServer;
using System;

namespace All
{
    /// <summary>
    /// This delegate represents all the functions that gets a message
    /// as a string and tells the players to move.
    /// </summary>
    /// <param name="message">
    /// The movement represented in a string.
    /// </param>
    public delegate void MoveListener(string message);

    /// <summary>
    /// Format a direction to a message to notify the move listeners .
    /// </summary>
    /// <param name="direction">
    /// The direction which we will use.
    /// </param>
    /// <returns>
    /// A string of the notification.
    /// </returns>
    public delegate string FormatNotificationToListeners(Direction direction);

    /// <summary>
    /// The player class will be a layer that we will add to the client that the server
    /// will communicate with,we have in this class all the abilities and function that
    /// we expect from a player in a SearchGame.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// Will make a unique id to each player. If will have on one place that creates Players it
        /// Will be unique (as long as we'll have up to 4,000,000,000 players).
        /// </summary>
        protected int id;

        /// <summary>
        /// Holds the client which this player class represents.
        /// </summary>
        private ICanbeNotified client;

        /// <summary>
        /// The position of this player in the SearchGame.
        /// </summary>
        protected Position location;

        /// <summary>
        /// An event that notifies the players in a case of a movement in the SearchGame.
        /// </summary>
        public event MoveListener NotifyMeWhenYouMove;

        /// <summary>
        /// A delegate that creates the notification format with given direction.
        /// </summary>
        protected FormatNotificationToListeners format;

        /// <summary>
        /// Property for the location of the player in the game.
        /// </summary>
        /// <return>
        /// The location of the player in the game.
        /// </return>
        public Position Location
        {
            set { location = value; }
            get { return location; }
        }

        /// <summary>
        /// Constructor for the player class that gets the client and it's id.
        /// </summary>
        /// <param name="client">
        /// The client as a ICanBeNotified type.
        /// </param>
        /// <param name="id">
        /// The id of the player.
        /// </param>
        public Player(ICanbeNotified client, int id)
        {
            this.client = client;
            this.id = id;
            // Initialize as empty
            NotifyMeWhenYouMove = new MoveListener((s) => { });
            format = (dir) => "";
        }

        /// <summary>
        /// Clear all the listeners to this player movement.
        /// </summary>
        public void ClearListeners()
        {
            Delegate[] list = NotifyMeWhenYouMove.GetInvocationList();
            foreach (Delegate d in list)
            {
                NotifyMeWhenYouMove -= (MoveListener)d;
            }
        }

        /// <summary>
        /// Set the format of the notification to the listeners as the
        /// function which we will get in the input as a delegate.
        /// </summary>
        /// <param name="format">
        /// A delegate of the notification format function as we defined above.
        /// </param>
        public void SetFormat(FormatNotificationToListeners format)
        {
            this.format = format;
        }

        /**
         * 
         */
         ///<summary>
         /// If a move request is invalid, we ignore it. A game is much funer if it doesn't bother you 
         /// everytime you eccidently push the wrong button move forward to a wall.
         /// </summary>
        public bool Move(Direction move, IsLegalPlayerLocation isLegal,string isExit)
        {
            if (isExit == "exit")
            {
                //Console.WriteLine("SENDING EXIT THROUGH PLAYER");
                NotifyMeWhenYouMove("exit");
            }
            bool returnValue = false;
            switch (move)
            {
                case Direction.Up:
                    {
                        location.Row -= 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                            returnValue = true;
                        }
                        else
                        {
                            location.Row += 1;
                            returnValue = false;
                        }
                        break;
                    }
                case Direction.Down:
                    {
                        location.Row += 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                            returnValue = true;
                        }
                        else
                        {
                            location.Row -= 1;
                            returnValue = false;
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        location.Col -= 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                            returnValue = true;
                        }
                        else
                        {
                            location.Col += 1;
                            returnValue = false;
                        }
                        break;
                    }
                case Direction.Right:
                    {
                        location.Col += 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                            returnValue = true;
                        }
                        else
                        {
                            location.Col -= 1;
                            returnValue = false;
                        }
                        break;
                    }
                default:
                    break;
            }
            return returnValue;
        }
        /// <summary>
        /// Overload the Equals method of object for this player class, we will compare
        /// players by their references and by the clients that they represent.
        /// </summary>
        /// <param name="obj">
        /// The object we will compare to.
        /// </param>
        /// <returns>
        /// True if they are equal,false otherwise.
        /// </returns>
        public override bool Equals(object obj)
        {
            return id == (obj as Player).id && ReferenceEquals(client, (obj as Player).client);
        }

        /// <summary>
        /// Override the GetHashCode method of object.
        /// </summary>
        /// <returns>
        /// The id as the hashcode.
        /// </returns>
        public override int GetHashCode()
        {
            return id;
        }

        /// <summary>
        /// Notify a change in the game by using the abilities of the ICanBeNotified.
        /// </summary>
        /// <param name="message">
        /// The message of the notification.
        /// </param>
        public void NotifyAChangeInTheGame(string message)
        {
            client.Notify(message);
        }
    }
}

