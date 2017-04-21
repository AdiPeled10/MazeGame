using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// This interface will have all the abilities that we expect a class which is Searchable to have.
    /// We want to be able to get the initial state of the Searchable and the goal state.
    /// In addition, we want the searchable to give us all the neighbour states of a specific state, like
    /// nodes which are neighbours in an undirected graph.
    /// </summary>
    /// <typeparam name="T"> This is the type of the States of the Searchable.</typeparam>
    public interface ISearchable<T>
    {
        /// <summary>
        /// Get the initial state of the Searchable, i.e. where we start from.
        /// </summary>
        /// <returns>
        /// Returns the initial State, the state is of the type of the template T.
        /// </returns>
        State<T> GetInitialState();

        /// <summary>
        /// Get the goal state of the searchable, i.e. where the search ends.
        /// </summary>
        /// <returns>
        /// Returns the goal state which is of the type of the template T.
        /// </returns>
        State<T> GetGoalState();

        /// <summary>
        /// By giving this method a specific state we will get all the states we can go to from this
        /// state in this searchable, like a group of neighbours in an undirected graph.
        /// </summary>
        /// <param name="s">
        /// The state which we want to know all of it's neighbours.
        /// </param>
        /// <returns>
        /// The list of states we can get to from the input state.
        /// </returns>
        List<State<T>> GetAllPossibleStates(State<T> s);
    }
}
