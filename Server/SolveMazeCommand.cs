using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using MazeGeneratorLib;
using MazeLib;
using SearchAlgorithmsLib;

namespace Server
{
    class SolveMazeCommand : ICommand
    {
        private IModel model;

        public SolveMazeCommand(IModel model)
        {
            this.model = model;
        }
        public string Execute(string[] args, TcpClient client)
        {
            string name = args[0];
            int algorithm = int.Parse(args[1]);

            Solution<State<Position>> solution = model.computeSolution(name, algorithm);
            //Convert the solution to JSon format.
            return solution.ToJSON();
        }
    }
}
