using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// Interface which holds all the functions that we expect a Controller to have
    /// in the MVC implementation for our application.
    /// </summary>
    public interface IController
    {
        /// <summary>
        /// Execute the given commmand in the command line from the given
        /// client.
        /// </summary>
        /// <param name="commandLine">
        /// The command in the format of a string.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
        void ExecuteCommand(string commandLine, IClient client);

        /// <summary>
        /// Disconnect the given client from our application.
        /// </summary>
        /// <param name="client">
        /// The client that we wish to disconnect.
        /// </param>
        void DisconnectClient(IClient client);
    }
}
