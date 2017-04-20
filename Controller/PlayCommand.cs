using System;
using Models;
using ClientForServer;

namespace Controllers
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
            //Player player = model.GetPlayer(client);
            //ISearchGame game = model.GetGameOf(player);

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

                /*
                 * TODO for now this method doesn't notify anyone because we don't need/know how to handle
                 * it right now (it creates a problem of "when the client will see the message" if the client
                 * is in "read input" state before the message got to him). In the future we probably move this
                 * logic to the client application to easy the work of the server.
                 */
                // check if the game has ended
                //if (game.HasEnded())
                //{
                //    game.DecalreWinner("You Won!", "You Lost!");
                //}
            }
            catch (NullReferenceException)
            {
                client.SendResponse("Player doesn't belog to a game.");
            }
        }
    }
}
