using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrentPriorityQueue;

namespace SearchAlgorithmsLib
{
    public abstract class Searcher<MyPriorityQueue, T> : ISearcher<T>
        where MyPriorityQueue : IPriorityQueue<State<T>,int>, new()
    {
        private MyPriorityQueue openList;
        private int evaluatedNodes;
   
        public Searcher()
        {
            openList = new MyPriorityQueue();
            evaluatedNodes = 0;
        }

        protected State<T> PopOpenList()
        {
            evaluatedNodes++;
            return openList.Dequeue();
        }

        // a property of openList
        public int OpenListSize
        { // it is a read-only property :)
            get { return openList.Count; }
        }

        // ISearcher’s methods:
        public int getNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        public abstract Solution<T> Search(ISearchable<T> searchable);
    }
}
