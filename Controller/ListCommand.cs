using System.Collections.Generic;
using Model;
using ClientForServer;
using Newtonsoft.Json;

namespace Controller
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
