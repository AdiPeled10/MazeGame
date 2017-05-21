using System.Collections.Generic;

namespace ViewModel
{
    /// <summary>
    /// This Class is a "ViewModel" and is like a pipe between the view and the
    /// model, but it also saves a chache of solutions.
    /// </summary>
    public class SinglePlayerVM : GameViewModel
    {
        /// <summary>
        /// Save solution per maze name, it is a dictionary for now in the case
        /// where there could be several games running at once.
        /// </summary>
        private Dictionary<string, string> nameToSolution;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SinglePlayerVM() : base()
        {
            nameToSolution = new Dictionary<string, string>(5);
            //Add GotMaze to event to notify when maze was generarted.
            model.GotSolution += AddSolution;
        }

        /// <summary>
        /// Calls model.GenerateMaze("generate", name, rows, cols).
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public override void GenerateMaze(string name, int rows, int cols)
        {
            model.GenerateMaze("generate", name, rows, cols);
        }

        /// <summary>
        /// Calls model.GetMazeSolution(maze).
        /// </summary>
        /// <param name="maze"></param>
        public void AskForSolution(string maze)
        {
            model.GetMazeSolution(maze);
        }

        /// <summary>
        /// Calls model.CloseClient(mazeName).
        /// </summary>
        public void CloseConnection()
        {
            model.CloseClient(mazeName);
        }

        /// <summary>
        /// Adds a solution to the dictionary.
        /// </summary>
        /// <param name="solution"></param>
        /// <param name="name"></param>
        public void AddSolution(string solution, string name)
        {
            nameToSolution[name] = solution;
        }

        /// <summary>
        /// Returns the solution in the chache that fits "name" or null if
        /// no such is saved in the chache.
        /// </summary>
        /// <param name="name"></param>
        /// <returns> a string that represents the solution. </returns>
        public string GetMazeSolution(string name)
        {
            try
            {
                return nameToSolution[name];
            } catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
