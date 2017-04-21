using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// This is an abstract Searcher class which implements several functions which are common
    /// for each Searcher and doesn't rely on the algorithm that the searcher uses.
    /// </summary>
    /// <typeparam name="T">
    /// This template represents the type of the State.
    /// </typeparam>
    /// <typeparam name="DataStructure">
    /// This is the data structure that the Searcher is going to use during his search.
    /// </typeparam>
    /// <typeparam name="DataStructureElementsType">
    /// The type of the elements that the data structure is going to hold. 
    /// </typeparam>
    public abstract class Searcher<T, DataStructure, DataStructureElementsType> : ISearcher<T>
      where DataStructure : ICollectionDataStructure<DataStructureElementsType>, new()
    {
        /// <summary>
        /// This is the data structure that we are going to use for the searching algorithm.
        /// </summary>
        private DataStructure openList;

        /// <summary>
        /// Save the number of nodes that the searcher evaluated during the search
        /// as a private member.
        /// </summary>
        private int evaluatedNodes;

        /// <summary>
        /// Constructor of the Searcher class, in this method we will create the data
        /// structure and set the number of nodes evaluated to 0.
        /// </summary>
        public Searcher()
        {
            openList = new DataStructure();
            evaluatedNodes = 0;
        }

        /// <summary>
        /// Pop an element from the openList which is our data structure.
        /// </summary>
        /// <returns>
        /// Returns an elemet of the type of the elements of the DataStructure which is
        /// popped from the data structure.
        /// </returns>
        protected DataStructureElementsType PopOpenList()
        {
            ++evaluatedNodes;
            // Return the state.
            return openList.PopFirst();
        }

        /// <summary>
        /// Add an element to the data structure.
        /// </summary>
        /// <param name="value">
        /// The element that we are going to add to the data structure.
        /// </param>
        protected void AddToOpenList(DataStructureElementsType value)
        {
            openList.Add(value);
        }

        /// <summary>
        /// Replace two elements in the openList which is the Searcher's data structure.We will use it in
        /// different algorithms to update the openList ,(like Decrease Key in a Priority queue).
        /// </summary>
        /// <param name="value">
        /// The old value which is going to be replaced.
        /// </param>
        /// <param name="newValue">
        /// The new element which we are going to replace with the old one.
        /// </param>
        protected void ReplaceElementInOpenList(DataStructureElementsType value, DataStructureElementsType newValue)
        {
            openList.Remove(value);
            openList.Add(newValue);
        }

        /// <summary>
        /// This is a property which we are going to use to get the size of the open list. 
        /// </summary>
        protected int OpenListSize
        {
            get { return openList.Count; }
        }

        /// <summary>
        /// We will use this method to back trace the path we went through from the initial state
        /// to the goal state to help us build the Solution to the Searcher. We will use the State class
        ///  CameFro method to build this path. We know when to stop because the initial state's came from
        ///  is initialized to be null.
        /// </summary>
        /// <param name="goalState">
        /// This is the goal state where we ended our path.
        /// </param>
        /// <returns>
        /// Return a linked list of states which represents the path from the initial state to the goal
        /// state.
        /// </returns>
        protected LinkedList<State<T>> BackTracePath(State<T> goalState)
        {
            State<T> currentState = goalState;
            LinkedList<State<T>> path = new LinkedList<State<T>>();
            //Loop until we get to the initial state.
            while (!ReferenceEquals(currentState, null))
            {
                path.AddFirst(currentState);
                currentState = currentState.GetCameFrom();
            }
            return path;
        }

        /// <summary>
        /// Get the number of nodes that Searcher evaluated.
        /// </summary>
        /// <returns>
        /// The number of nodes evaluated.
        /// </returns>
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        /// <summary>
        /// The Search method which searches through the searchable, at first we clear the open list
        /// data structure and set the number of nodes evaluated to 0 and then start the RealSearch which
        /// is the algorithm that the Searcher is going to use to Search through the Searcher.
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>
        /// The Solution to the Searcher, a path from the initial state to the goal state.
        /// </returns>
        public Solution<T> Search(ISearchable<T> searchable)
        {
            openList.Clear();
            evaluatedNodes = 0;
            return RealSearch(searchable);
        }

        /// <summary>
        /// The RealSearch is an abstract method because the searching algorithm is going to
        /// be different for each Searcher and so does the implementation.
        /// </summary>
        /// <param name="searchable">
        /// This is the Searchable that we are going to search through.
        /// </param>
        /// <returns>
        /// Returns the solution to the searchable.
        /// </returns>
        protected abstract Solution<T> RealSearch(ISearchable<T> searchable);
    }

    /// <summary>
    /// This is an abstract class that represents each Searcher that is going to use
    /// the priority queue data structure during his searching algorithm, most of the time when
    /// using a priority queue we will use the Decrease Key operation so we added a method of 
    /// UpdatePriority that is basically the DecreaseKey operation.
    /// </summary>
    /// <typeparam name="T">
    /// This template represents the type of the state we are going to search through.
    /// </typeparam>
    public abstract class PrioritySearcher<T> :
        Searcher<T, MyPriorityQueue<double, State<T>>, KeyValuePair<double, State<T>>>
    {

        /// <summary>
        /// This method will update the priority of a KeyValuePair to a new priority.
        /// </summary>
        /// <param name="pair">
        /// The key and value pair that we are going to update.
        /// </param>
        /// <param name="newPriority">
        /// The new priority that we are going to update to.
        /// </param>
        protected void UpdatePriority(KeyValuePair<double, State<T>> pair, double newPriority)
        {
            ReplaceElementInOpenList(pair, new KeyValuePair<double, State<T>>(newPriority, pair.Value));
        }
    }
}
