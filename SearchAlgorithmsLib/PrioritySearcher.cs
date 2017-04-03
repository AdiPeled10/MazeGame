using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrentPriorityQueue;

namespace SearchAlgorithmsLib
{
    public abstract class PrioritySearcher<T> : ISearcher<T>
      //  where MyPriorityQueue : IPriorityQueue<State<T>,int>, new()
    {
        protected MyPriorityQueue<State<T>,double> openList;
        protected int evaluatedNodes;
   
        public PrioritySearcher()
        {
            openList = new MyPriorityQueue<State<T>, double>();
            evaluatedNodes = 0;
        }

        protected State<T> PopOpenList()
        {
            evaluatedNodes++;
            KeyValuePair<State<T>,double> pair = openList.Dequeue();
            // Return the state.
            return pair.Key;
        }

        // a property of openList
        public int OpenListSize
        { // it is a read-only property :)
            get { return openList.Count; }
        }

        // ISearcher’s methods:
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        public abstract Solution<T> Search(ISearchable<T> searchable);
    }
}
