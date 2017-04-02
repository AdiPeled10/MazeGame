using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C5;

namespace SearchAlgorithmsLib
{
    public class PriorityQueue<TElement> : ConcurrentPriorityQueue.ConcurrentPriorityQueue<TElement, int>
    {
        public PriorityQueue()
        { }
    }

    // Best First Search
    public class BFSSearcher<U> : Searcher<PriorityQueue<State<U>>, U>
    {
        public override Solution<U> Search(ISearchable<U> searchable)
        {
            return null;
        }
    }
}
  
