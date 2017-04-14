using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class PlayCommand : ICommand
    {
        private IModel model;

        public PlayCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client)
        {
            string direction = args[0];

            string name = model.Play(Converter.StringToDirection(direction), client);

            JObject playObj = new JObject
            {
                ["Name"] = name,
                ["Direction"] = direction
            };
            client.SendResponse(playObj.ToString());
        }
    }
}
