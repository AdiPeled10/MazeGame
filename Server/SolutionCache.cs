using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using SearchAlgorithmsLib;

namespace Server
{
    // TODO search using the game because it's possiblr to have 2 games with the same maze but different names
    // TODO dictionary keys as "maze.ToString()" or "maze.GetHashValue()"
    // TODO save the actual solutions in files(and load it from the file if needed) to prevent the heap/main memory
    // from growing very large.
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
