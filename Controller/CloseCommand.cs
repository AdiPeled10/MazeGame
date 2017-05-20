using Models;
using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// This is the Close command which tells the server to close a specific game.
    /// </summary>
    class CloseCommand : ICommand
    {
        /// <summary>
        /// The model which we will use to execute the command.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Constructor of the CloseCommand that gets the model.
        /// </summary>
        /// <param name="model">
        /// The model that we will use.
        /// </param>
        public CloseCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// The execution of the close command.
        /// </summary>
        /// <param name="args">
        /// The arguments for this command.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
        public void Execute(string[] args, IClient client = null)
        {
            // close the game
            model.Close(args[0], client);
            client.Disconnect();
        }
    }
}
