using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public abstract class StackSearcher<T> : ISearcher<T> 
    {
        protected Stack<State<T>> stack;
        protected int evaluatedNodes;

        public StackSearcher() {
            collection = new Stack<State<T>>();
        }

        public abstract Solution<T> Search(ISearchable<T> searchable);

        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }
    }
}
