using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MazeGeneratorLib;
using MazeLib;
using SearchAlgorithmsLib;
using Newtonsoft.Json.Linq;

namespace Server
{
    class SolveMazeCommand : ICommand
    {
        private IModel model;

        public SolveMazeCommand(IModel model)
        {
            this.model = model;
        }
        public string Execute(string[] args, IClient client)
        {
            string name = args[0];
            int algorithm = int.Parse(args[1]);

            Solution<Position> solution = model.ComputeSolution(name, algorithm);
            //Convert the solution to JSon format.
            JObject solveObj = new JObject();
            solveObj["Name"] = name;
            solveObj["Solution"] = Converter.ToJSON(solution);
            solveObj["NodesEvaluated"] = solution.NodesEvaluated;
            return solveObj.ToString();
        }
    }
}
