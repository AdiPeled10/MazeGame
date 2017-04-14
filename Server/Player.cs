using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MazeLib;

namespace Server
{
    public class Player// : IPlayer
    {
        // TODO Maybe use IPlayer interface later.
        private int id;  // will make a unique id to each player. If will have on one place that creates Players it
                         // will be unique (as long as we'll have up to 4,000,000,000 players).
        private IClient client;
        private Position location;

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

        /**
         * if a move request is invalid, we ignore it. A game is much funer if it doesn't bother you
         * everytime you eccidently push the wrong button move forward to a wall.
         */
        public void Move(Direction move, int width, int height)
        {
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
                        if (location.Col + 1 < height)
                            location.Col += 1;
                        break;
                    }
                default:
                    break;

            }
        }

        public override bool Equals(object obj)
        {
            ////Compare by string of TcpClient's hashcode.
            ////We compare only by TcpClient cause we get only TcpClient in the command
            //return (obj.ToString().GetHashCode() == this.client.ToString().GetHashCode());

            // TODO THIS IS NOT SAFE. It assume every player has a different id and that same clients are stored at the same reference.
            return id == (obj as Player).id && ReferenceEquals(client, (obj as Player).client);
        }

        public override int GetHashCode()
        {
            return id;
        }

        // TODO maybe not notify directly
        public void NotifyWonOrLost(string message)
        {
            client.SendResponse(message);
        }
    }
}

