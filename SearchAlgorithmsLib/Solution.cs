using System.Collections.Generic;
using System.Collections;
using System.Text;
using MazeLib;

namespace SearchAlgorithmsLib
{
    // Immutable
    public class Solution<T> : IEnumerable<T>, IEnumerable
    {
        private List<T> theRoadToVictory;
        private int numberOfNodesEvaluated;

        public Solution()
        {
            theRoadToVictory = new List<T>();
            numberOfNodesEvaluated = 0;
        }

        public Solution(ICollection<State<T>> thePathToVictory, int evaluated)
        {
            this.theRoadToVictory = new List<T>(thePathToVictory.Count);
            foreach (State<T> st in thePathToVictory)
            {
                this.theRoadToVictory.Add(st.GetState());
            }
            this.numberOfNodesEvaluated = evaluated;
        }

        public Solution(Solution<T> sol)
        {
            theRoadToVictory = new List<T>(sol.theRoadToVictory);
            numberOfNodesEvaluated = sol.numberOfNodesEvaluated;
        }

        public int NodesEvaluated
        {
            get { return numberOfNodesEvaluated; }
        }

        //public void AddState(State<T> newLastState)
        //{
        //    theRoadToVictory.Add(newLastState);
        //}

        ///**
        // * Removes the returned state from the solution. Which means that if
        // * you want to go through the solution but skip the first state, you can
        // * call this right before the "foreach" loop.
        // */
        //public State<T> GetNextState()
        //{
        //    State<T> st = theRoadToVictory[0];
        //    theRoadToVictory.Remove(st);
        //    return st;
        //}

        ///**
        // * Removes the returned state from the solution. Which means that if
        // * you want to go through the solution but skip the first state, you can
        // * call this right before the "foreach" loop.
        // */
        //public T GetNextValue()
        //{
        //    State<T> st = theRoadToVictory[0];
        //    theRoadToVictory.Remove(st);
        //    return st.GetState();
        //}
        
        //public T this[int index]
        //{
        //    get { return theRoadToVictory[index]; }
        //}

        public int Length
        {
            get { return theRoadToVictory.Count; }
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        public IEnumerator<T> GetEnumerator()
        {
            return theRoadToVictory.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
  
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class Solution<T> : IEnumerable<T>
    {
        private List<T> theRoadToVictory;

        public Solution()
        {
            theRoadToVictory = new List<T>();
        }

        public Solution(List<T> theRoadToVictory)
        {
            this.theRoadToVictory = new List<T>(theRoadToVictory);
        }

        public Solution(List<State<T>> theRoadToVictory)
        {
            this.theRoadToVictory = new List<T>(theRoadToVictory.Count);
            foreach (State<T> s in theRoadToVictory)
            {
                this.theRoadToVictory.Add(s.GetState());
            }
            this.theRoadToVictory.Reverse();
        }

        public void AddState(State<T> newLastState)
        {
            theRoadToVictory.Add(newLastState.GetState());
        }

        public T GetNextStep()
        {
            State<T> st = theRoadToVictory.FirstOrDefault();
            theRoadToVictory.Remove(st);
            return st.GetState();
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        IEnumerator<T> GetEnumerator()
        {
            List<T> l = new List<T>(theRoadToVictory.Count);
            foreach (State<T> s in theRoadToVictory)
            {
                l.
            }
        }
    }
}
*/