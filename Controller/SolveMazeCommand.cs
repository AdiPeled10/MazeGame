using MazeLib;
using SearchAlgorithmsLib;
using Newtonsoft.Json.Linq;
using Models;
using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// Implementation of the Solve command which implements the ICommand interface.
    /// </summary>
    class SolveMazeCommand : ICommand
    {
        /// <summary>
        /// The model which we will use.
        /// </summary>
        private IModel model;

        /// <summary>
        /// Construcotor of the solve command with the model as it's input.
        /// </summary>
        /// <param name="model">
        /// The model that we are going to use.
        /// </param>
        public SolveMazeCommand(IModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Execution of the solve command based on the arguments
        /// and the client that sent the command.
        /// </summary>
        /// <param name="args">
        /// The arguments of the command.
        /// </param>
        /// <param name="client">
        /// The client that sent this command.
        /// </param>
        public void Execute(string[] args, IClient client)
        {
            string name = args[0];
            Algorithm algorithm = (Algorithm)int.Parse(args[1]);

            Solution<Position> solution = model.ComputeSolution(name, algorithm);
            //Convert the solution to JSON format.
            JObject solveObj = new JObject
            {
                ["Name"] = name,
                ["Solution"] = Converter.ToJSON(solution),
                ["NodesEvaluated"] = solution.NodesEvaluated
            };
            client.SendResponse(solveObj.ToString());
        }
    }
}
