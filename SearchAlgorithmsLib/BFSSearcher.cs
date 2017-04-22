using System.Collections.Generic;

namespace SearchAlgorithmsLib
{

    /// <summary>
    /// This is the class which we will use to search through different search areas by
    /// using the Best First Search algorithm, this algorithm uses a priority queue which holds
    /// the states with the state with minimal cost at the top of the queue. This is why this class 
    /// inherits from the PrioritySearcher abstract class which represents all searchers which use a 
    /// priority queue in their implementation.
    /// </summary>
    /// <typeparam name="T">
    /// This is the type which the State holds. 
    /// </typeparam>
    public class BFSSearcher<T> : PrioritySearcher<T>
    {

        /// <summary>
        ///  This method implements the Best First Search algorithm to search through the Searchable until
        ///  we get to the State which is our goal.
        /// </summary>
        /// <param name="searchable"> 
        /// This is the Searchable that we are going to search through.
        /// </param>
        /// <returns>
        /// It returns the Solution to the Searchable which is the path from the
        /// initial state to the goal state which the Best First Search algorithm found.
        /// </returns>
        protected override Solution<T> RealSearch(ISearchable<T> searchable)
        {
            KeyValuePair<double, State<T>> currentState;
            double pathPrice;
            List<State<T>> succerssors;
            State<T> currentStateValue = searchable.GetInitialState(), goal = searchable.GetGoalState();
            // a list of all already visited
            Dictionary<State<T>, double> closed = new Dictionary<State<T>, double>();
            AddToOpenList(new KeyValuePair<double, State<T>>(0, currentStateValue));
            closed.Add(currentStateValue, 0);
            currentStateValue.SetCameFrom(null);
            while (OpenListSize > 0)
            {
                currentState = PopOpenList(); // inherited from Searcher, removes the best state
                currentStateValue = currentState.Value;

                if (!currentStateValue.Equals(goal))
                {
                    // calling the delegated method, returns a list of states with n as a parent
                    succerssors = searchable.GetAllPossibleStates(currentStateValue);
                }
                else
                {
                    //Back trace the path from the goal to the initial state.
                    return new Solution<T>(BackTracePath(currentStateValue), GetNumberOfNodesEvaluated());
                }

                foreach (State<T> s in succerssors)
                {
                    pathPrice = currentState.Key + s.Cost;
                    if (!closed.ContainsKey(s))
                    {
                        s.SetCameFrom(currentStateValue);
                        AddToOpenList(new KeyValuePair<double, State<T>>(pathPrice, s));
                        closed.Add(s, pathPrice);
                    }
                    else if (pathPrice < closed[s])
                    {
                        UpdatePriority(new KeyValuePair<double, State<T>>(closed[currentStateValue], currentStateValue),
                            pathPrice);
                        closed[s] = pathPrice;
                        s.SetCameFrom(currentStateValue);
                    }
                }
            }
            //We wont get here because there will always be a path from initial to goal.
            return new Solution<T>();
        }
    }
}

