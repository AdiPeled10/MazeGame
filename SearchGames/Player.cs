using MazeLib;
using ClientForServer;
using System;

namespace SearchGames
{
    public delegate void MoveListener(string message);

    // format a direction to a message to notify the move listeners 
    public delegate string FormatNotificationToListeners(Direction direction);

    public class Player
    {
        protected int id;  // will make a unique id to each player. If will have on one place that creates Players it
                         // will be unique (as long as we'll have up to 4,000,000,000 players).
        private ICanbeNotified client;
        protected Position location;
        public event MoveListener NotifyMeWhenYouMove;
        protected FormatNotificationToListeners format;

        public Position Location
        {
            set { location = value; }
            get { return location; }
        }

        public Player(ICanbeNotified client, int id)
        {
            this.client = client;
            this.id = id;
            // initialize as empty
            NotifyMeWhenYouMove = new MoveListener((s) => { });
            format = (dir) => "";
        }

        public void ClearListeners()
        {
            Delegate[] list = NotifyMeWhenYouMove.GetInvocationList();
            foreach (Delegate d in list)
            {
                NotifyMeWhenYouMove -= (MoveListener)d;
            }
        }

        public void SetFormat(FormatNotificationToListeners format)
        {
            this.format = format;
        }

        /**
         * if a move request is invalid, we ignore it. A game is much funer if it doesn't bother you
         * everytime you eccidently push the wrong button move forward to a wall.
         */
        public void Move(Direction move, IsLegalPlayerLocation isLegal)
        {
            // TODO Check if it's like a GUI Up mean -1 Down +1.
            switch (move)
            {
                case Direction.Up:
                    {
                        location.Row -= 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                        }
                        else
                        {
                            location.Row += 1;
                        }
                        break;
                    }
                case Direction.Down:
                    {
                        location.Row += 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                        }
                        else
                        {
                            location.Row -= 1;
                        }
                        break;
                    }
                case Direction.Left:
                    {
                        location.Col -= 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                        }
                        else
                        {
                            location.Col += 1;
                        }
                        break;
                    }
                case Direction.Right:
                    {
                        location.Col += 1;
                        if (isLegal(location))
                        {
                            NotifyMeWhenYouMove(format(move));
                        }
                        else
                        {
                            location.Col -= 1;
                        }
                        break;
                    }
                default:
                    break;
            }
        }

        public override bool Equals(object obj)
        {
            return id == (obj as Player).id && ReferenceEquals(client, (obj as Player).client);
        }

        public override int GetHashCode()
        {
            return id;
        }

        public void NotifyAChangeInTheGame(string message)
        {
            client.Notify(message);
        }
    }
}

