using Model;
using ClientForServer;

namespace Controller
{
    class GenerateMazeCommand : ICommand
    {
        private IModel model;

        public GenerateMazeCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client)
        {
            string name = args[0];
            int rows = int.Parse(args[1]);
            int cols = int.Parse(args[2]);

            //Return JSon format of this maze.
            client.SendResponse(model.GenerateNewGame(name, rows, cols).ToJSON());
        }

    }
}
