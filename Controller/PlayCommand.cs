using System;
using Models;
using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// Implementation of the Play command which implements the ICommand interface.
    /// </summary>
    public class PlayCommand : ICommand
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Constructor of the Play command with the model as it's input.
        /// </summary>
        /// <param name="model">
        /// The model which we are going to use it's functionality.
        /// </param>
        public PlayCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Execution of the play command based on the given arguments
        /// and the client which sent this request.
        /// </summary>
        /// <exception cref="Exception">
        /// In the case in which there is no game to start.
        /// </exception>
        /// <param name="args">
        /// The arguments of the command.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
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
