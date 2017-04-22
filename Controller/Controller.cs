using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using ClientForServer;
using SearchGames;

namespace Controllers
{
    /// <summary>
    /// This is the controller class which we will use in the implementation of the MVC 
    /// architectural pattern, it will implement all the methods in the IController which is
    /// the interface for all of the controllers in our application.
    /// </summary>
    public class Controller : IController
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// A queue of request from clients.
        /// </summary>
        private IRequestsQueue queue;

        /// <summary>
        /// A dictionary that matches between a given string to the correct command.
        /// </summary>
        private Dictionary<string, ICommand> commands;

        /// <summary>
        /// Constructor of the controller, it gets the model of the controller and the queue 
        /// of requests.
        /// </summary>
        /// <param name="model">
        /// This is the model which we will use.
        /// </param>
        /// <param name="executionQueue">
        /// The queue of client's requests.
        /// </param>
        public Controller(IModel model, IRequestsQueue executionQueue)//, IView view)
        {
            this.model = model;
            this.queue = executionQueue;
            //this.view = view;
            //Continue to add other commands to the dictionary after they are created.
            commands = new Dictionary<string, ICommand>(7)
            {
                { "generate", new GenerateMazeCommand(model) },
                { "solve", new SolveMazeCommand(model) },
                { "start", new StartGameCommand(model) },
                { "list", new ListCommand(model) },
                { "join", new JoinCommand(model) },
                { "play", new PlayCommand(model) },
                { "close", new CloseCommand(model) }
            };
        }

        /// <summary>
        /// Execute the given command in the command line from the given client.
        /// </summary>
        /// <param name="commandLine">
        /// The command that was given as a string.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
        public void ExecuteCommand(string commandLine, IClient client)
        {
            Action request;
            string[] arr = commandLine.Split(' ');
            string commandKey = arr[0];
            // Ignore "faults requests" the player probably pressed/send it by accident.
            if (commands.ContainsKey(commandKey))
            {
                string[] args = arr.Skip(1).ToArray();
                ICommand command = commands[commandKey];
                request = () =>
                {
                    command.Execute(args, client);
                };
                queue.Add(request, client);
            }
         
            else 
            {
                request = () =>
                {
                   client.SendResponse("This command is invalid.");
               };
               queue.Add(request, client);
            }
        }

        /// <summary>
        /// Disconnect the given client from the server.
        /// Not in the dictionary to prevent clients from using it
        /// </summary>
        /// <param name="client">
        /// The client that will be disconnected.
        /// </param>
        public void DisconnectClient(IClient client)
        {
            // get the game
            ISearchGame game = model.GetGameOf(client);

            // remove all the client requeses
            queue.Remove(client);

            ///*
            // * TODO for now this method doesn't notify anyone because we don't need/know how to handle
            // * it right now (it creates a problem of "when the client will see the message" if the client
            // * is in "read input" state before the message got to him). In the future we probably move this
            // * logic to the client application to easy the work of the server.
            // * maybe we don't need to worry about the other the clients player, because he will just
            // * freeze in the game.
            // */
            //// declare technical winning.
            //game.DecalreWinner("Connection lost with one of the player. Technical victory!",
            //    "Connection lost with one of your teammate. Technical Lost!");

            /**
             * Removing the client from the server(game and player). This task doesn't has to be
             * in the task queue(when it will be execute won't matter), so it's only a Task that
             * let a thread handle disconnecting the client from the model.
             */
            Task task = new Task(() =>
            {
                model.RemoveClient(client);
                client.Disconnect();
            });
            task.Start();
        }
    }
}
