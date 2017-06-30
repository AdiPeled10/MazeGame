using Microsoft.AspNet.SignalR;
using Project.Controllers;
using Project.Models;

namespace Project
{
    /// <summary>
    /// This class is a Hub for SignalR usage at the client.
    /// </summary>
    public class GameHub : Hub
    {
        /// <summary>
        /// sends hello to all the clients.
        /// </summary>
        public void Hello()
        {
            Clients.All.hello();
        }

        /// <summary>
        /// sends message to the client with userId as its id.
        /// </summary>
        /// <param name="userId"> string </param>
        /// <param name="message"> string </param>
        public void Send(string userId, string message)
        {
            //Send message to user.
            string id = Context.ConnectionId;
            Clients.User(userId).send(message);
        }

        /// <summary>
        /// Checks if username is in the database and if it does, the function calss
        /// MazeController.StartGame(Context.ConnectionId, name, rows, cols, username).
        /// </summary>
        /// <param name="name"> string - the generated maze name </param>
        /// <param name="rows"> int - the number of rows in the generated maze </param>
        /// <param name="cols"> int - the number of cols in the generated maze </param>
        /// <param name="username"> string - The requesting client username(he has to be signed in) </param>
        public void StartGame(string name, int rows, int cols, string username)
        {
            // check that the username exists
            UsersController db = new UsersController();
            if (db.UserExists(username))
            {
                // Start the game.
                MazeController.StartGame(Context.ConnectionId, name, rows, cols, username);
            }
            db.TearDown();
        }

        /// <summary>
        /// The function checks if username is in the database and if it does,
        /// the function joins the client to the multiplayer game named name.
        /// Then, this function sends the two game players the game.
        /// </summary>
        /// <param name="name"> The existing multiplayer game </param>
        /// <param name="username"> string - The requesting client username(he has to be signed in) </param>
        public void JoinGame(string name, string username)
        {
            // check that the username exists
            UsersController db = new UsersController();
            if (db.UserExists(username))
            {
                GameInfo game = MazeController.JoinGame(Context.ConnectionId, name, username);
                //Send to first client and second client the maze.
                Clients.Client(game.FirstClient).getMaze(game.Maze);
                Clients.Client(game.SecondClient).getMaze(game.Maze);
            }
            db.TearDown();
        }

        /// <summary>
        /// sends the opponent of the calling client that the calling client
        /// moved in the direction of num.
        /// </summary>
        /// <param name="num"> int - a numer that represent a move in the game </param>
        public void Play(int num)
        {
            string opponent = MazeController.GetOpponent(Context.ConnectionId);
            //Play move in given direction.
            Clients.Client(opponent).play(num);
        }

        /// <summary>
        /// calls MazeController.GameEnded(Context.ConnectionId).
        /// </summary>
        public void GameEnded()
        {
            MazeController.GameEnded(Context.ConnectionId);
        }
    }
}