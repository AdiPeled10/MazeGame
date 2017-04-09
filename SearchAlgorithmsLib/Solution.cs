using System.Collections.Generic;
using System.Collections;

namespace SearchAlgorithmsLib
{
    public class Solution<T> : IEnumerable<State<T>>, IEnumerable
    {
        private List<State<T>> theRoadToVictory;

        public Solution()
        {
            theRoadToVictory = new List<State<T>>();
        }

        public Solution(List<State<T>> theRoadToVictory)
        {
            this.theRoadToVictory = new List<State<T>>(theRoadToVictory);
        }

        public void AddState(State<T> newLastState)
        {
            theRoadToVictory.Add(newLastState);
        }

        public State<T> GetNextState()
        {
            State<T> st = theRoadToVictory[0];
            theRoadToVictory.Remove(st);
            return st;
        }

        public T GetNextStep()
        {
            State<T> st = theRoadToVictory[0];
            theRoadToVictory.Remove(st);
            return st.GetState();
        }

        //
        // Summary:
        //     Returns an enumerator that iterates through the collection.
        //
        // Returns:
        //     An enumerator that can be used to iterate through the collection.
        public IEnumerator<State<T>> GetEnumerator()
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