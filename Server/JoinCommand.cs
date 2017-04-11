using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server
{
    public class JoinCommand : ICommand
    {
        private IModel model;

        public JoinCommand(IModel model)
        {
            this.model = model;
        }

        public string Execute(string[] args,IClient client)
        {

            // TODO - It's very similar to the case of StartGameCommand maybe create abstract class for both.
            string name = args[0];
            model.Join(name, new Player(client, 0));
            return model.GetGame(name).ToJSON();
        }
    }
}
