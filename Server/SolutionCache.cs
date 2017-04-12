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
    class SolutionCache<T>
    {
        private Dictionary<T, Solution<Position>> nameToSolution;

        //HashSet of all the mazes we already solved.
        private HashSet<T> solved;

        public SolutionCache()
        {
            nameToSolution = new Dictionary<T, Solution<Position>>(32);
            solved = new HashSet<T>();
        }

        public void AddSolution(T key, Solution<Position> solution)
        {
            nameToSolution[key] = solution;
            solved.Add(key);
        }

        public Solution<Position> GetSolution(T key)
        {
            return nameToSolution[key];
            //if (IsSolved(key))
            //{
            //    return nameToSolution[key];
            //}
            ////There is no solution saved.TODO-Fix exception handling.
            //return null;
        }

        public bool IsSolved(T name)
        {
            return solved.Contains(name);
        }
    }
}
