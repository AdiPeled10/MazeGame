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
            Player player = model.GetPlayer(client);
            ISearchGame game = model.GetGameOf(player);

            try
            {
                // make the move
                model.Play(Converter.StringToDirection(direction), client);

                //// create the notification message about the move
                //JObject playObj = new JObject
                //{
                //    ["Name"] = game.Name,
                //    ["Direction"] = direction
                //};
                //string move = playObj.ToString();

                //// notify the other players
                //IEnumerable<Player> players = game.GetPlayers();
                //foreach (Player p in players)
                //{
                //    if (!player.Equals(p))
                //    {
                //        p.NotifyAChangeInTheGame(move);
                //    }
                //}

                // check if the game has ended
                if (game.HasEnded())
                {
                    game.DecalreWinner("You Won!", "You Lost!");
                }
            }
            catch (NullReferenceException)
            {
                client.SendResponse("Player doesn't belog to a game.");
            }
        }
    }
}
