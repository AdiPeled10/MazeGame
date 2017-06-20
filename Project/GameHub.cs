using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Project.Controllers;
using Project.Models;

namespace Project
{
    public class GameHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string userId, string message)
        {
            //Send message to user.
            string id = Context.ConnectionId;
            Clients.User(userId).send(message);
        }

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

        public void Play(int num)
        {
            string opponent = MazeController.GetOpponent(Context.ConnectionId);
            //Play move in given direction.
            Clients.Client(opponent).play(num);
        }

        public void GameEnded()
        {
            MazeController.GameEnded(Context.ConnectionId);
        }
    }
}