using System;
using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// This is an adapter for the stack data structure which let use use methods with the same
    /// name with a stack,priority queue and any other data structure. It supports methods to add an elemnent
    /// delete an element and more which will allow us to use ploymorphism in different searchers, we could use 
    /// Add or Remove method without knowing which data structure we are really using. We are going to use it
    /// in the implementation of the DFSSearcher which uses a stack during it's search.
    /// </summary>
    /// <typeparam name="T">
    /// This is the type of the elements that the Stack holds.
    /// </typeparam>
    public class StackAdapter<T> : Stack<T>, ICollectionDataStructure<T>
    {
        /// <summary>
        /// This method pops the element which is at the top of the stack,we will just use the pop method
        /// of the Stack class which our adapter inherits from.
        /// </summary>
        /// <returns>
        /// The element of type T (our template) which is at the top of the stack.
        /// </returns>
        public T PopFirst()
        {
            return base.Pop();
        }

        /// <summary>
        /// This method is added only for polymorphism so we will be able to define different data
        /// structures with the same propeties, in this case the stack doesn't need to support a remove
        /// method so we will just throw a NotSupportedException.
        /// </summary>
        /// <param name="element"> 
        /// The element that we plan to remove.
        /// </param>
        /// <returns>
        /// Generally a boolean,but in this case we won't need to return anything.
        /// </returns>
        public bool Remove(T element)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Add an element to the stack,just use the Push method of the Stack class which our adapter
        /// inherits from.
        /// </summary>
        /// <param name="element"> 
        /// The element we are going to add to the stack.
        /// </param>
        public void Add(T element)
        {
            base.Push(element);
        }
    }

    /// <summary>
    /// This class will use the Depth First Search to search through the Searchable to find the path from the
    /// initial state to the goal state, this algorithm uses a stack in it's implementation that's why we will
    /// use the StackAdaptor,during the search we will run through the states by starting at the initial state
    /// of the Searchable and going in depth in the searchable until we get to the goal state,it is not promised
    /// that we will get the shortest path from the initial state to the goal state by using this specific algorithm.
    /// </summary>
    /// <typeparam name="T"> 
    /// This is a template to define the type of the element of the states
    /// of the Searchable this Searcher will search through.
    /// </typeparam>
    public class DFSSearcher<T> : Searcher<T, StackAdapter<State<T>>, State<T>>
    {

        /// <summary>
        /// This method will run the Search through the Searchable by using the Depth First Search algorithm.
        /// We will start in the initial state of the searchable and start going in depth in the searchable from
        /// state to state by marking each state that we pass through until we get to the goal state, in the case which
        /// we get from one to state to a state which is marked, which means we have already been there before we will
        /// use the StackAdapter to go back to the previous state.
        /// </summary>
        /// <param name="searchable">
        /// The searchable that we are going to search through.
        /// </param>
        /// <returns>
        /// It returns the solution which is the list of states we will pass through from
        /// the initial state to the goal state.
        /// </returns>
        protected override Solution<T> RealSearch(ISearchable<T> searchable)
        {
            List<State<T>> succerssors;
            State<T> current = searchable.GetInitialState(), goal = searchable.GetGoalState();
            HashSet<State<T>> closed = new HashSet<State<T>>();
            AddToOpenList(current);
            closed.Add(current);
            current.SetCameFrom(null);
            while (OpenListSize != 0)
            {
                current = PopOpenList();
                if (!current.Equals(goal))
                {
                    // calling the delegated method, returns a list of states with n as a parent
                    succerssors = searchable.GetAllPossibleStates(current);
                }
                else
                {
                    //Back trace the path from the goal to the initial state.
                    return new Solution<T>(BackTracePath(current), GetNumberOfNodesEvaluated());
                }

                foreach (State<T> st in succerssors)
                {
                    if (!closed.Contains(st))
                    {
                        //Set the new parent with SetCameFrom method.
                        st.SetCameFrom(current);
                        closed.Add(st);
                        AddToOpenList(st);
                    }
                }
            }
            return new Solution<T>();
        }
    }
}
