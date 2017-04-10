using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using SearchAlgorithmsLib;

namespace Server
{
    public class SolutionCache
    {
        private Dictionary<string,Solution<Position>> nameToSolution;

        //HashSet of all the mazes we already solved.
        private HashSet<string> solved;

        public SolutionCache()
        {
            nameToSolution = new Dictionary<string, Solution<Position>>();
            solved = new HashSet<string>();
        }

        public void AddSolution(string name,Solution<Position> solution)
        {
            nameToSolution[name] = solution;
            solved.Add(name);
        }

        public Solution<Position> GetSolution(string name)
        {
            if (IsSolved(name))
            {
                return nameToSolution[name];
            }
            //There is no solution saved.TODO-Fix exception handling.
            return null;
        }

        public bool IsSolved(string name)
        {
            return solved.Contains(name);
        }
    }
}
