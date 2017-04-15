﻿using MazeLib;
using SearchAlgorithmsLib;
using Newtonsoft.Json.Linq;
using Model;
using ClientForServer;

namespace Controller
{
    class SolveMazeCommand : ICommand
    {
        private IModel model;

        public SolveMazeCommand(IModel model)
        {
            this.model = model;
        }

        public void Execute(string[] args, IClient client)
        {
            string name = args[0];
            int algorithm = int.Parse(args[1]);

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
