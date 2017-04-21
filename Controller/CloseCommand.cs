using Models;
using ClientForServer;

namespace Controllers
{
    class CloseCommand : ICommand
    {
        private IModel model;

        public CloseCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client = null)
        {
            // close the game
            model.Close(args[0], client);
        }
    }
}
