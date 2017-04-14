using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Server
{
    public class ListCommand : ICommand
    {
        private IModel model;

        public ListCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args,IClient client)
        {
            List<string> games = model.GetJoinableGamesList();
            client.SendResponse(JsonConvert.SerializeObject(games));
        }
    }
}
