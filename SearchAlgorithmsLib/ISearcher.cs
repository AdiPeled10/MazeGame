
namespace SearchAlgorithmsLib
{
    /// <summary>
    /// This is an interface which holds all the set of abilities that we expect from a Searcher which is
    /// going to search through a Searchable. It holds the Search method which will get the Searchable that we
    /// plan to search through and returns the Solution to the Searchable. We also want
    /// the searchable to tell us the number of nodes in the Searchable that it evaluated during the search. 
    /// That way we can compare between different searchers.
    /// </summary>
    /// <typeparam name="T">
    /// This template represents the type of the States that the searcher will search though.
    /// </typeparam>
    public interface ISearcher<T>
    {
        /// <summary>
        /// Search through the Searchable, this is the most basic method which we will expect
        /// from every Searcher to have.
        /// </summary>
        /// <param name="searchable">
        /// The Searchable we are going to search through.
        /// </param>
        /// <returns>
        /// Returns the solution which is a list of the States we pass through from the initial state
        /// to the goal state.
        /// </returns>
        Solution<T> Search(ISearchable<T> searchable);

        /// <summary>
        /// Get the number of nodes that the Searcher evaluated during the search.
        /// </summary>
        /// <returns>
        /// The number of nodes evaluated as an integer.
        /// </returns>
        int GetNumberOfNodesEvaluated();
    }
}
