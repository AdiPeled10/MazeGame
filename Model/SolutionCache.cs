using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Model
{
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
