using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class DFSSearcher<T> : StackSearcher<T>
    {

       
        public override Solution<T> Search(ISearchable<T> searchable)
        {

            State<T> initialState = searchable.getInitialState(), currentState;
            initialState.Property = true;
            // Add the initial state to the stack.
            stack.Push(initialState);
            while (stack.Count != 0) {
                currentState = stack.Pop();
                if (currentState == searchable.getGoalState())
                {
                    //Backtrace
                }
                if ((bool)currentState.Property == true)
                    continue;
                else if (currentState.Property == null)
                {
                    //Time of discovery in DFS traversal.
                    currentState.Property = true;
                    //Pass through all of his sons in the graph.
                    List<State<T>> sons = searchable.getAllPossibleStates(currentState);
                    foreach (State<T> s in sons)
                    {
                        stack.Push(s);
                    }
                }
                //Iterate until the stack will be empty.
                //Pop from the stack the last State we entered in.

            }
            return null;
        }

        
    }
}
