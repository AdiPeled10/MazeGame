using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{

    // Best First Search
    public class BFSSearcher<T> : Searcher<T>
    {
        public override Solution<T> Search(ISearchable<T> searchable)
        {
            //This will be our initial state that we start the search from.
            State<T> state = searchable.getInitialState();
            //Set the cost of the initial state to be 0.
            state.Cost = 0;
            //Set parent of the initial state to be null.
            state.SetCameFrom(null);
            openList.Enqueue(state, state.Cost);
            List<State<T>> closedList = new List<State<T>>();
            State<T> currentState;
            KeyValuePair<State<T>, double> currentPair;
            while (!openList.IsEmpty)
            {
                currentPair = openList.Dequeue();
                currentState = currentPair.Key;

                if (currentState.Equals(searchable.getGoalState()))
                {
                    //Back trace the path from the goal to the initial state.
                    return new Solution<T>(BackTracePath(currentState));
                }
                else
                {
                    List<State<T>> possibleStates = searchable.getAllPossibleStates(currentState);
                    foreach (State<T> myState in possibleStates)
                    {
                        if (!closedList.Contains(myState) && !openList.Contains(new KeyValuePair<State<T>,
                            double>(myState, myState.Cost)))
                        {
                            //Update that we came from the currentState.
                            myState.SetCameFrom(currentState);
                            openList.Enqueue(myState, myState.Cost);
                        }
                        else
                        {
                            //Get pair of current state in the priority queue.
                            currentPair = openList.GetPairFromValue(myState.Cost);
                            if (currentPair.Value > myState.Cost)
                            {
                                //In this case the path we found through myState is better.
                                if (!openList.Contains(new KeyValuePair<State<T>, double>(myState, myState.Cost)))
                                {
                                    //The priority queue doesn't contain this state,add it to the queue.
                                    openList.Enqueue(myState, myState.Cost);
                                }
                                else
                                {
                                    //Update it's value in the queue,like Decrease-Key operation in heap.
                                    openList.Remove(new KeyValuePair<State<T>, double>(myState, myState.Cost));
                                    openList.Enqueue(myState, myState.Cost);
                                }
                            }
                        }
                    }
                }
            }
            //We wont get here because there will always be a path from initial to goal.
            return null;
        }

        public List<State<T>> BackTracePath(State<T> goalState) {
            State<T> currentState = goalState;
            List<State<T>> path = new List<State<T>>();
            //Loop until we get to the initial state.
            while (currentState.GetCameFrom() != null)
            {
                path.Insert(0, currentState);
                currentState = currentState.GetCameFrom();
            }
            return path;
        }
    }
}
  
