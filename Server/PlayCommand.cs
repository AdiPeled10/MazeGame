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

        public string Execute(string[] args,IClient client)
        {
            string name = args[0];
            string direction = args[1];

            model.Play(Converter.StringToDirection(direction), new Player(client, 0));

            JObject playObj = new JObject();
            playObj["Name"] = name;
            playObj["Direction"] = direction;
            return playObj.ToString();
        }
    }
}
