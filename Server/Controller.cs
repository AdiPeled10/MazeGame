using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Controller : IController
    {
        private IView view;
        private IModel model;
        private Dictionary<string, ICommand> commands;

        public Controller(IModel model,IView view)
        {
            this.model = model;
            this.view = view;
            commands = new Dictionary<string, ICommand>();
            commands.Add("generate", new GenerateMazeCommand(model));
            commands.Add("solve", new SolveMazeCommand(model));
            commands.Add("start", new StartGameCommand(model));
            //Continue to add other commands to the dictionary after they are created.
        }

        public void ExecuteCommand(string commandLine,IClient client)
        {

        }
    }
}
