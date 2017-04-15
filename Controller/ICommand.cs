using ClientForServer;

namespace Controller
{
    interface ICommand
    {
        void Execute(string[] args, IClient client = null);
    }
}
