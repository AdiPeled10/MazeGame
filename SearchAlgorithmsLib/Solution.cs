using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class Solution<T>
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
            State<T> st = theRoadToVictory.FirstOrDefault();
            theRoadToVictory.Remove(st);
            return st;
        }
    }
}
