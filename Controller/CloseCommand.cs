using System.Collections.Generic;
using Models;
using ClientForServer;
using SearchGames;

namespace Controllers
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

            // notify the othe players the game is closed
            foreach (Player p in players)
            {
                if (!player.Equals(p))
                {
                    // sending an empty JSON object
                    p.NotifyAChangeInTheGame("{}");
                }
            }

            // close the game
            model.Close(args[0]);
        }
    }
}
