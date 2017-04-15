using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Server
{
    class CloseCommand : ICommand
    {
        private IModel model;

        public CloseCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client = null)
        {
            // get a list of the players
            Player player = model.GetPlayer(client);
            IEnumerable<Player> players = model.GetGameByName(args[0]).GetPlayers();

            // prepare closing message
            JObject playObj = new JObject();
            string message = playObj.ToString();

            // notify the othe players the game is closed
            foreach (Player p in players)
            {
                if (!player.Equals(p))
                {
                    p.NotifyAChangeInTheGame(message);
                }
            }

            // close the game
            model.Close(args[0]);
        }
    }
}
