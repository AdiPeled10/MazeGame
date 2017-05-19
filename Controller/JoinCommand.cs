using Newtonsoft.Json.Linq;
using Models;
using ClientForServer;
using SearchGames;

namespace Controllers
{
    /// <summary>
    /// Implementation of the Join command which implements the ICommand interface.
    /// </summary>
    public class JoinCommand : ICommand
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Constructor of the join command which will get the model that we 
        /// will use in the command's execution.
        /// </summary>
        /// <param name="model">
        /// The model that we are going to use it's functionality.
        /// </param>
        public JoinCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Execution of the join command based on the given arguments and the client
        /// that sent this command.
        /// </summary>
        /// <param name="args">
        /// The arguments of the command.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
        public void Execute(string[] args, IClient client)
        {
            string name = args[0];
            try
            {
                if (model.Join(name, client))
                {
                    // get the game
                    ISearchGame game = model.GetGameByName(name);

                    // register a notifier to tell the client the game has started
                    game.TellMeWhenTheGameStarts += () => client.SendResponse(game.ToJSON());

                    // check if we can start the game
                    if (game.CanStart())
                    {
                        game.MakePlayersNotifyEachOtherAboutTheirMoves((move) =>
                        {
                            // create the notification message about the move
                            if (move.ToString().ToLower() == "exit")
                            {
                                //Exit message.
                                return "exit";
                            }
                            JObject playObj = new JObject
                            {
                                ["Name"] = game.Name,
                                ["Direction"] = move.ToString()
                            };
                            return playObj.ToString();
                        });
                        try
                        {
                            game.Start();
                        }
                        catch
                        {
                            client.SendResponse("Failed To join. Something is wrong with the game setting.");
                        }
                    }
                }
                else
                {
                    client.SendResponse(@"Falied to join. You already a part of an existing game.
                        \r\n Please close that game and try again.");
                }
            }
            catch //(System.NullReferenceException)
            {
                client.SendResponse("Falied to join. No such game.");
            }
        }
        //public void MakePlayersNotifyEachOtherAboutTheirMoves(IEnumerable<Player> players,
        //    FormatNotificationToListeners format)
        //{
        //    MoveListener notifyFunc;
        //    // delete old listeners and set format
        //    foreach (Player player in players)
        //    {
        //        player.SetFormat(format);
        //        //player.NotifyMeWhenYouMove = null;
        //    }

        //    // set each player new listeners
        //    foreach (Player player in players)
        //    {
        //        notifyFunc = (move) => player.NotifyAChangeInTheGame(move);
        //        foreach (Player p in players)
        //        {
        //            if (!player.Equals(p))
        //            {
        //                p.NotifyMeWhenYouMove += notifyFunc;
        //            }
        //        }
        //    }
        //}
    }
}
