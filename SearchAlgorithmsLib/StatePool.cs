using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlgorithmsLib
{

    /// <summary>
    /// The StatePool class will help us handle the creation of all the states during the search
    /// very efficiently and will help us recycle states which we used before by using the pool.
    /// We are going to get states only through the state pool, that way the pool can manipulate the
    /// use of the states and to use the avialable memory efficiently.
    /// </summary>
    /// <typeparam name="T">
    /// This template represents the real type of the states.
    /// </typeparam>
    public class StatePool<T>
    {
        /// <summary>
        /// This dictionary will match each state of type T( the key) to his State class representative.
        /// </summary>
        private Dictionary<T, State<T>> statePool;

        /// <summary>
        /// Default Constructor, creates an empty dictionary.
        /// </summary>
        public StatePool()
        {
            statePool = new Dictionary<T, State<T>>(1024);
        }

        /// <summary>
        /// Get a specific state, if it's already in the pool take it from
        /// the statePool member, otherwise create a new one which holds the state
        /// in the input and add it to the statePool.
        /// </summary>
        /// <exception cref="KeyNotFoundException">
        /// This exception will be thrown when the state in the input is not a key in our
        /// statePool dictionary, when we catch this exception we will create a new State.
        /// </exception>
        /// <param name="state">
        /// The state that we want to get his State class representative.
        /// </param>
        /// <returns>
        /// State which holds the input state.
        /// </returns>
        public State<T> GetState(T state)
        {
            try
            {
                //This is already in the pool.
                return statePool[state];
            }
            catch (KeyNotFoundException)
            {
                //It is not in the state, create a new one and add to the pool.
                State<T> st = new State<T>(state);
                statePool[state] = st;
                return st;
            }
        }

        /// <summary>
        /// Get the state but this time we will use a different constructor for
        /// the state than the previous method, we will use the cost as an input as well.
        /// </summary>
        /// <param name="state">
        /// The state that we want to get.
        /// </param>
        /// <param name="initializeCost">
        /// The state's price.
        /// </param>
        /// <returns>
        /// The state, a new state that we created or a state that
        /// we took from the pool.
        /// </returns>
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
