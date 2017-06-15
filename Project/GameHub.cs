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

        public void Send(string userId,string message)
        {
            //Send message to user.
            string id = Context.ConnectionId;
            Clients.User(userId).send(message);
        }

        public void StartGame(string name,int rows,int cols)
        {
            // Start the game.
            MazeController.StartGame(Context.ConnectionId, name, rows, cols);
        }

        public void JoinGame(string name)
        {
            GameInfo game = MazeController.JoinGame(Context.ConnectionId, name);
            //Send to first client and second client the maze.
            Clients.Client(game.FirstClient).getMaze(game.Maze);
            Clients.Client(game.SecondClient).getMaze(game.Maze);
        }

        public void Play(int num)
        {
            string opponent = MazeController.GetOpponent(Context.ConnectionId);
            //Play move in given direction.
            Clients.Client(opponent).play(num);
        }


    }
}