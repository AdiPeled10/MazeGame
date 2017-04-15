using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MazeLib;

namespace Server
{
    public delegate void MoveListener(string message);

    // format a direction to a message to notify the move listeners 
    public delegate string FormatNotificationToListeners(Direction direction);

    public class Player
    {
        private int id;  // will make a unique id to each player. If will have on one place that creates Players it
                         // will be unique (as long as we'll have up to 4,000,000,000 players).
        private IClient client;
        private Position location;
        public event MoveListener NotifyMeWhenYouMove;
        private FormatNotificationToListeners format;

        public IClient Client
        {
            get { return client; }
        }

        public Position Location
        {
            set { location = value; }
            get { return location; }
        }

        public Player(IClient client, int id)
        {
            this.client = client;
            this.id = id;
        }

        public void ClearListeners()
        {
            NotifyMeWhenYouMove = new MoveListener((s) => { });
            NotifyMeWhenYouMove = null;
            MoveListener[] list = (MoveListener[])NotifyMeWhenYouMove.GetInvocationList();
            foreach (MoveListener d in list)
            {
                NotifyMeWhenYouMove -= d;
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
        public void Move(Direction move, int width, int height)
        {
            NotifyMeWhenYouMove(format(move));
            // TODO Check if it's like a GUI Up mean -1 Down +1.
            switch (move)
            {
                case Direction.Up:
                    {
                        if (location.Row - 1 >= 0)
                            location.Row -= 1;
                        break;
                    }
                case Direction.Down:
                    {
                        if (location.Row + 1 < height)
                            location.Row += 1;
                        break;
                    }
                case Direction.Left:
                    {
                        if (location.Col - 1 >= 0)
                            location.Col -= 1;
                        break;
                    }
                case Direction.Right:
                    {
                        if (location.Col + 1 < width)
                            location.Col += 1;
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
            client.SendResponse(message);
        }
    }
}

