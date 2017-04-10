using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MazeLib;

namespace Server
{
    public class Player
    {
        //TODO- Maybe use IPlayer interface later.
        private TcpClient client;
        private Position location;

        public TcpClient Client
        {
            get { return client; }
        }
        public Player(TcpClient client)
        {
            this.client = client;
        }

        public Position Location
        {
            set { location = value; }
            get { return location; }
        }

        public void Move(Direction move)
        {
            //TODO- Check if it's like a GUI Up mean -1 Down +1.
            try
            {
                switch (move)
                {
                    case Direction.Up:
                        {
                            location = new Position(location.Row - 1, location.Col);
                            break;
                        }
                    case Direction.Down:
                        {
                            location = new Position(location.Row + 1, location.Col);
                            break;
                        }
                    case Direction.Left:
                        {
                            location = new Position(location.Row, location.Col - 1);
                            break;
                        }
                    case Direction.Right:
                        {
                            location = new Position(location.Row, location.Col + 1);
                            break;
                        }
                    default:
                        break;

                }
            } catch (IndexOutOfRangeException exp)
            {
                //TODO- Exception handling.
            }
        }

        public override bool Equals(object obj)
        {
            //Compare by string of TcpClient's hashcode.
            //We compare only by TcpClient cause we get only TcpClient in the command
            return (obj.ToString().GetHashCode() == this.client.ToString().GetHashCode());
        }
    }
}

