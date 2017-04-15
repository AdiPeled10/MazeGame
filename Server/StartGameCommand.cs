using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class StartGameCommand : ICommand
    {
        private IModel model;

        public StartGameCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client)
        {
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);
            ISearchGame game = model.StartGame(name, rows, cols, client);

            if (!ReferenceEquals(game, null))
            {
                // set condition to start the game
                game.SetStartWhenTrue((g) => (g.NumOfPlayer >= 2));

                // send the client the game when it starts
                game.TellMeWhenTheGameStarts += () => client.SendResponse(game.ToJSON());
            }

            client.SendResponse("A game with that name already exists and cannot be replaced at this point.");
        }
    }
}
