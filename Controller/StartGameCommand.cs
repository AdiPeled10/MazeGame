﻿using Models;
using ClientForServer;
using SearchGames;

namespace Controllers
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
            else
            {
                if (!ReferenceEquals(model.GetGameByName(name), null))
                    client.SendResponse("A game with that name already exists and cannot be replaced at this point.");
                else
                    client.SendResponse("You're already a part of a game. Please close that game and try again.");
            }
        }
    }
}
