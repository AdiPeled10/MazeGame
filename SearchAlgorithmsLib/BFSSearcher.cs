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
            State<T> state = searchable.getInitialState();
            state.Cost = 0;
            openList.Enqueue(state, state.Cost);
            List<State<T>> closedList = new List<State<T>>();
            State<T> currentState;
            KeyValuePair<State<T>, double> currentPair;
            while (!openList.IsEmpty)
            {
                currentPair = openList.Dequeue();
                currentState = currentPair.Key;

                if (currentState.Equals(searchable.getGoalState())) {
                    //Backtrace path to n through recorded parents.
                }
                List<State<T>> possibleStates = searchable.getAllPossibleStates(currentState);
                foreach (State<T> myState in possibleStates) {
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
                            if (openList.Contains(new KeyValuePair<State<T>, double>(myState, myState.Cost)))
                            {
                                openList.Enqueue(myState, myState.Cost);
                            }
                            else
                            {
                                openList.Remove(new KeyValuePair<State<T>, double>(myState, myState.Cost));
                                openList.Enqueue(myState, myState.Cost);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
  
