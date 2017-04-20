using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{
    public class StatePool<T>
    {
        private Dictionary<T, State<T>> statePool;

        public StatePool()
        {
            statePool = new Dictionary<T, State<T>>(1024);
        }

        public State<T> GetState(T state)
        {
            try
            {
                return statePool[state];
            }
            catch (KeyNotFoundException)
            {
                State<T> st = new State<T>(state);
                statePool[state] = st;
                return st;
            }
        }

        public State<T> GetState(T state, double initializeCost)
        {
            try
            {
                return statePool[state];
            }
            catch (KeyNotFoundException)
            {
                State<T> st = new State<T>(state, initializeCost);
                statePool[state] = st;
                return st;
            }
        }

    }
}
