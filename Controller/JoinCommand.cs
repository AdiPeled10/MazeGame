using Newtonsoft.Json.Linq;
using Model;
using ClientForServer;
using SearchGames;

namespace Controller
{
    public class JoinCommand : ICommand
    {
        private IModel model;

        public JoinCommand(IModel model)
        {
            this.model = model;
        }

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
                        game.MakePlayersNotifyEachOtherAboutTheirMoves((move) => {
                            // create the notification message about the move
                            JObject playObj = new JObject
                            {
                                ["Name"] = game.Name,
                                ["Direction"] = move.ToString()
                            };
                            return playObj.ToString();
                        });
                        game.Start();
                    }
                }
            }
            catch //(NullReferenceException, KeyNotFoundException)
            {
                client.SendResponse("Falied to join. No such game or You already a part of an existing game.");
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
