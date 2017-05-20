using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUIClient;

namespace ViewModel
{
    public class SinglePlayerVM : GameViewModel
    {
        /// <summary>
        /// Save solution per maze name, it is a dictionary for now in the case
        /// where there could be several games running at once.
        /// </summary>
        private Dictionary<string, string> nameToSolution;

        public SinglePlayerVM() : base()
        {
            nameToSolution = new Dictionary<string, string>(5);
            //Add GotMaze to event to notify when maze was generarted.
            model.GotSolution += AddSolution;
        }

        public override void GenerateMaze(string name, int rows, int cols)
        {
            model.GenerateMaze("generate", name, rows, cols);
        }

        public void AskForSolution(string maze)
        {
            //TODO- Fix the way we get the algorithm.
            model.GetMazeSolution(maze);
        }

        public void CloseConnection()
        {
            model.CloseClient(mazeName);
        }

        public void AddSolution(string solution,string name)
        {
            nameToSolution[name] = solution;
        }

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
