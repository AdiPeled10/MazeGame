using Models;
using ClientForServer;
using SearchGames;

namespace Controllers
{
    /// <summary>
    /// Implementation of the Start command which implements the ICommand interface.
    /// </summary>
    public class StartGameCommand : ICommand
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Constructor of the Start command which is given the model as an input.
        /// </summary>
        /// <param name="model">
        /// The model which we are going to use.
        /// </param>
        public StartGameCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Execution of the Start command based on the given arguments
        /// and the client that sent it.
        /// </summary>
        /// <param name="args">
        /// The arguments of this command.
        /// </param>
        /// <param name="client">
        /// The client which sent this request.
        /// </param>
        public void Execute(string[] args, IClient client)
        {
            System.Console.WriteLine("Got start command.");
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
                    client.SendResponse(@"A game with that name already exists and 
                        cannot be replaced at this point.");
                else
                    client.SendResponse(@"You're already a part of a game.
                        Please close that game and try again.");
            }
        }
    }
}
