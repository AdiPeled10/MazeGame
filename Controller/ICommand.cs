using ClientForServer;

namespace Controllers
{
    interface ICommand
    {
        void Execute(string[] args, IClient client = null);
    }
}
