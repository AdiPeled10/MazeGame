using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Models
{
    // TODO save the actual solutions in files(and load it from the file if needed) to prevent the heap/main memory
    // from growing very large.
    /// <summary>
    /// The SolutionCache is basically a cache that we will use to save solutions to different Search Games
    /// to prevent the case in which we compute the solution to the same SearchGame multiple times.
    /// </summary>
    /// <typeparam name="T">
    /// Template which represents the type of the keys in the dictionnary,makes code more generic.
    /// </typeparam>
    class SolutionCache<T>
    {
        /// <summary>
        /// A dictionary that matches between the template T and the solution.
        /// </summary>
        private Dictionary<T, Solution<Position>> nameToSolution;

        /// <summary>
        /// HashSet of all the search games we already solved.
        /// </summary>
        private HashSet<T> solved;

        /// <summary>
        /// Default constructor of the Solution cache.
        /// </summary>
        public SolutionCache()
        {
            nameToSolution = new Dictionary<T, Solution<Position>>(32);
            solved = new HashSet<T>();
        }

        /// <summary>
        /// Add a solution to the cache by adding it to our dictionary.
        /// </summary>
        /// <param name="key">
        /// The key that we will add to the dictionary.
        /// </param>
        /// <param name="solution">
        /// The solution which will be added as a value in the dictionary.
        /// </param>
        public void AddSolution(T key, Solution<Position> solution)
        {
            nameToSolution[key] = solution;
            solved.Add(key);
        }

        /// <summary>
        /// Get the solution of the given key.
        /// </summary>
        /// <param name="key">
        /// The key of the solution that we are looking for.
        /// </param>
        /// <returns>
        /// The solution that matches this key.
        /// </returns>
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

        /// <summary>
        /// Tells us if a given SearchGame was already solved.
        /// </summary>
        /// <param name="name">
        /// The representative of this game in the template type T.
        /// </param>
        /// <returns>
        /// True if it was solved, false otherwise.
        /// </returns>
        public bool IsSolved(T name)
        {
            return solved.Contains(name);
        }
    }
}
