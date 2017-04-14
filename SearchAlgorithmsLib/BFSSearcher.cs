using System.Collections.Generic;

namespace SearchAlgorithmsLib
{

    // Best First Search
    public class BFSSearcher<T> : PrioritySearcher<T>
    {
        public override Solution<T> Search(ISearchable<T> searchable)
        {
            KeyValuePair<double, State<T>> currentState;
            double pathPrice;
            List<State<T>> succerssors;
            State<T> currentStateValue = searchable.GetInitialState(), goal = searchable.GetGoalState();
            Dictionary<State<T>, double> closed = new Dictionary<State<T>, double>();  // a list of all already visited
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
                    else if(pathPrice < closed[s])
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

/*
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
}*/
