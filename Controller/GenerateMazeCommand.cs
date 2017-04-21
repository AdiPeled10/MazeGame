using Models;
using ClientForServer;
using SearchGames; // only here until the client will approve TODO

namespace Controllers
{
    class GenerateMazeCommand : ICommand
    {
        private IModel model;

        public GenerateMazeCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client)
        {
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);

            // Return JSON format of this maze.
            client.SendResponse(model.GenerateNewGame(name, rows, cols).ToJSON());

            // TODO - also effected rhe code of model.GenerateNewGame.
            // No need to know the name of the single player game, but while the
            // client application is stupid will know it. Afterward we can replace it with the coe above
            model.Join(name, client);
            (model.GetGameByName(name) as MazeGame).MaxPlayersAllowed = 1; // only here while the client isn't build.
        }

    }
}
