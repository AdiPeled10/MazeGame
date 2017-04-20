using ClientForServer;

namespace Controllers
{
    public interface IController
    {
        void ExecuteCommand(string commandLine, IClient client);

        void DisconnectClient(IClient client);
    }
}
