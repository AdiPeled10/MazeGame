using Models;
using ClientForServer;
using SearchGames; // only here until the client will approve TODO

namespace Controllers
{
    /// <summary>
    /// Implementation of the Generate command which will generate a new maze
    ///  and implements the ICommand interface.
    /// </summary>
    class GenerateMazeCommand : ICommand
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Constructor for the GenerateMazeCommand which gets the model.
        /// </summary>
        /// <param name="model">
        /// The model which we will use.
        /// </param>
        public GenerateMazeCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Execute the generate command by creating a new maze based on the given
        /// arguments.
        /// </summary>
        /// <param name="args">
        /// The arguments of the command.
        /// </param>
        /// <param name="client">
        /// The client that sent the generate request.
        /// </param>
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
            if (!model.Join(name, client))
            {
                if (!ReferenceEquals(null,model.GetGameOf(client)))
                {
                    client.SendResponse(@"Deleted game named: " + model.GetGameOf(client).Name
                   + " to generate game: " + name);
                    model.DeleteGame(model.GetGameOf(client));
                    model.Join(name, client);
                }
            }
            (model.GetGameByName(name) as MazeGame).MaxPlayersAllowed = 1; 
        }

    }
}
