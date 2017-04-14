using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
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
            if (commands.ContainsKey(commandKey))
            {
                string[] args = arr.Skip(1).ToArray();
                ICommand command = commands[commandKey];
                request = () =>
                {
                    command.Execute(args, client); // TODO maybe use IView for the send
                };
            }
            else
            {
                request =() =>
                {
                    client.SendResponse("Command not found"); // TODO maybe use IView for the send
                };
            }
            queue.Add(request, client);
        }

        // not in the dictionary to prevent clients from using it
        public void DisconnectClient(IClient client)
        {
            // remove all the client requeses
            queue.Remove(client);
            // TODO command for removing the client from the games. doesn't has to be in the queue
            // let a thread handle disconnecting the client from the modle
            Task task = new Task(() =>
            {
                model.RemoveClient(client);
                client.Disconnect();
            });
            task.Start();
        }
    }
}
