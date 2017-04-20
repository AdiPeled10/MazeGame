using System.Collections.Generic;
using Models;
using ClientForServer;
using Newtonsoft.Json;

namespace Controllers
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
            client.SendResponse(JsonConvert.SerializeObject(games, Formatting.Indented));
        }
    }
}
