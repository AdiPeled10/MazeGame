using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class StartGameCommand : ICommand
    {
        private IModel model;

        public StartGameCommand(IModel model)
        {
            this.model = model;
        }

        public string Execute(string[] args,IClient client)
        {
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);
            model.StartGame(name, rows, cols);

            return model.GetGame(name).ToJSON();
        }
    }
}
