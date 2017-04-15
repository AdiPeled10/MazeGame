using ClientForServer;

namespace Controller
{
    public interface IController
    {
        void ExecuteCommand(string commandLine, IClient client);

        void DisconnectClient(IClient client);
    }
}
