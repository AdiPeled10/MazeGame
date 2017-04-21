using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// Interface for all of the commands that we will implement, it holds all the
    /// abilities that we expect a command class to have.
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// Execute our command based on the given arguments and the client
        /// which sent this command.
        /// </summary>
        /// <param name="args">
        /// The command's arguments.
        /// </param>
        /// <param name="client">
        /// The client which sent this request.
        /// </param>
        void Execute(string[] args, IClient client = null);
    }
}
