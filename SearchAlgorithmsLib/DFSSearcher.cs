using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class StackAdapter<T> : Stack<T>, ICollectionDataStructure<T>
    {
        public T PopFirst()
        {
            return base.Pop();
        }

        public bool Remove(T element)
        {
            throw new NotSupportedException();
        }

        public void Add(T element)
        {
            base.Push(element);
        }
    }

    public class DFSSearcher<T> : Searcher<T, StackAdapter<State<T>>, State<T>>
    {
        public override Solution<T> Search(ISearchable<T> searchable)
        {
            List<State<T>> succerssors;
            State<T> current, goal = searchable.getGoalState();
            HashSet<State<T>> closed = new HashSet<State<T>>();
            AddToOpenList(searchable.getInitialState());
            while (OpenListSize != 0)
            {
                current = PopOpenList();
                if (!current.Equals(goal))
                {
                    // calling the delegated method, returns a list of states with n as a parent
                    succerssors = searchable.getAllPossibleStates(current);
                }
                else
                {
                    //Back trace the path from the goal to the initial state.
                    return new Solution<T>(BackTracePath(current));
                }

                foreach (State<T> st in succerssors)
                {
                    if (!closed.Contains(st))
                        AddToOpenList(st);
                }
            }
            return new Solution<T>();
        }
    }
}
