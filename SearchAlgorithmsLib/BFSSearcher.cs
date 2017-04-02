using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C5;

namespace SearchAlgorithmsLib
{
    // Best First Search
    public class BFSSearcher<U> : Searcher<Queue<State<U>>, U>
    {
        public override Solution<U> Search(ISearchable<U> searchable)
        {
            return null;
        }
    }
}
  
