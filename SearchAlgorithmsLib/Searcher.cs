using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public abstract class Searcher<MyPriorityQueue, T> : ISearcher<T> where MyPriorityQueue : ICollection<State<T>>, new()
    {
        private MyPriorityQueue openList;
        private int evaluatedNodes;
   
        public Searcher()
        {
            openList = new MyPriorityQueue();
            evaluatedNodes = 0;
        }

        protected State<T> popOpenList()
        {
            evaluatedNodes++;
            State<T> st = openList.FirstOrDefault();
            openList.Remove(st);
            return st;
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

        public abstract Solution<T> search(ISearchable<T> searchable);
    }
}
