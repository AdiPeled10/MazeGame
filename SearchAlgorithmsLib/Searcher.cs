using System.Collections.Generic;

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

        protected LinkedList<State<T>> BackTracePath(State<T> goalState)
        {
            State<T> currentState = goalState;
            LinkedList<State<T>> path = new LinkedList<State<T>>();
            //Loop until we get to the initial state.
            while (!ReferenceEquals(currentState, null))
            {
                path.AddFirst(currentState);
                currentState = currentState.GetCameFrom();
            }
            return path;
        }

        // ISearcher’s methods:
        public int GetNumberOfNodesEvaluated()
        {
            return evaluatedNodes;
        }

        // reuseable search
        public Solution<T> Search(ISearchable<T> searchable)
        {
            openList.Clear();
            evaluatedNodes = 0;
            return RealSearch(searchable);
        }

        protected abstract Solution<T> RealSearch(ISearchable<T> searchable);
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
