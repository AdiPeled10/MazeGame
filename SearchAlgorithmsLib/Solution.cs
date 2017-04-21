using System.Collections.Generic;
using System.Collections;
using System.Text;
using MazeLib;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// This class represents the solution to the Searchable which is the path from the initial
    /// state of the searchable to the goal state. We expect the search to be Enumerable.
    /// </summary>
    /// <typeparam name="T">
    /// This template represents the type of the states of the Solution.
    /// </typeparam>
    public class Solution<T> : IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// This is the list of the states which represents the path from the initial state to
        /// the goal path.
        /// </summary>
        private List<T> theRoadToVictory;

        /// <summary>
        /// The number of nodes evaluated during the search.
        /// </summary>
        private int numberOfNodesEvaluated;

        /// <summary>
        /// Default Constructor, create an empty list and set the number of
        /// nodes evaluated to be zero.
        /// </summary>
        public Solution()
        {
            theRoadToVictory = new List<T>();
            numberOfNodesEvaluated = 0;
        }

        /// <summary>
        /// Constructor for the Solution class, we will get the list of states of the path from
        /// the initial state to the goal state and the number of nodes evaluated.
        /// </summary>
        /// <param name="thePathToVictory">
        /// A list of states from the initial state to the goal state.
        /// </param>
        /// <param name="evaluated">
        /// The number of nodes evaluated.
        /// </param>
        public Solution(ICollection<State<T>> thePathToVictory, int evaluated)
        {
            this.theRoadToVictory = new List<T>(thePathToVictory.Count);
            foreach (State<T> st in thePathToVictory)
            {
                this.theRoadToVictory.Add(st.GetState());
            }
            this.numberOfNodesEvaluated = evaluated;
        }

        /// <summary>
        /// Copy constructor for the solution class.
        /// </summary>
        /// <param name="sol">
        /// The solution we are going to replicate.
        /// </param>
        public Solution(Solution<T> sol)
        {
            theRoadToVictory = new List<T>(sol.theRoadToVictory);
            numberOfNodesEvaluated = sol.numberOfNodesEvaluated;
        }

        /// <summary>
        /// Property for the number of nodes evaluated until finding the solution.
        /// </summary>
        public int NodesEvaluated
        {
            get { return numberOfNodesEvaluated; }
        }

        /// <summary>
        /// Property to get the length of the Solution.
        /// </summary>
        public int Length
        {
            get { return theRoadToVictory.Count; }
        }

        /// <summary>
        ///  Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///  An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return theRoadToVictory.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator for the Solution itself.
        /// </summary>
        /// <returns>
        /// An enumerator for this class.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

}
