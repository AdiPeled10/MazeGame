using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using ClientForServer;
using SearchGames;

namespace Controller
{
    public class Controller : IController
    {
        //private IView view;
        private IModel model;
        private IRequestsQueue queue;
        private Dictionary<string, ICommand> commands;

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
            //else
            //{
            //    request =() =>
            //    {
            //        client.SendResponse("Command not found"); // TODO maybe use IView for the send
            //    };
            //}
            //queue.Add(request, client);
        }

        // not in the dictionary to prevent clients from using it
        public void DisconnectClient(IClient client)
        {
            // get the game
            ISearchGame game = model.GetGameOf(client);

            // remove all the client requeses
            queue.Remove(client);

            // declare technical winning.
            game.DecalreWinner("Connection lost with one of the player. Technical victory!",
                "Connection lost with one of your teammate. Technical Lost!");

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
