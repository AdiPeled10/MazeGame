using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrentPriorityQueue;

namespace SearchAlgorithmsLib
{
    public abstract class Searcher<T, DataStructure, DataStructureElementsType> : ISearcher<T>
      where DataStructure : ICollectionDataStructure<DataStructureElementsType>, new()
    {
        private DataStructure openList;
        private int evaluatedNodes;

        public Searcher()
        {
            openList = new DataStructure();
            evaluatedNodes = 0;
        }

        protected DataStructureElementsType PopOpenList()
        {
            ++evaluatedNodes;
            // Return the state.
            return openList.PopFirst();
        }

        protected void AddToOpenList(DataStructureElementsType value)
        {
            openList.Add(value);
        }

        protected void ReplaceElementInOpenList(DataStructureElementsType value, DataStructureElementsType newValue)
        {
            openList.Remove(value);
            openList.Add(newValue);
        }

        // a property of openList
        protected int OpenListSize
        { // it is a read-only property :)
            get { return openList.Count; }
        }

        protected List<State<T>> BackTracePath(State<T> goalState)
        {
            State<T> currentState = goalState;
            List<State<T>> path = new List<State<T>>();
            //Loop until we get to the initial state.
            while (currentState != null)
            {
                path.Add(currentState);
                currentState = currentState.GetCameFrom();
            }
            return path;
        }

        // ISearcher’s methods:
        public int getNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        public abstract Solution<T> Search(ISearchable<T> searchable);
    }

    public abstract class PrioritySearcher<T> : 
        Searcher<T, MyPriorityQueue<double, State<T>>, KeyValuePair<double, State<T>>>
    {
        protected void UpdatePriority(KeyValuePair<double, State<T>> pair, double newPriority)
        {
            ReplaceElementInOpenList(pair, new KeyValuePair<double, State<T>>(newPriority, pair.Value));
        }
    }
}
