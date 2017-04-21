using System;
using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    /// <summary>
    /// We will use the State class to add another layer to each node in the graph which 
    /// represents the Searchable, if our Searchable searches through different Positions somehow, we
    /// won't need to know that because we are going to hide this information by using the State class to add
    /// another layer of encapsulation to our code.
    /// </summary>
    /// <typeparam name="T">
    /// Template that represents the type of the state.
    /// </typeparam>
    public class State<T>// : IComparable
    {
        /// <summary>
        /// This is the real state.
        /// </summary>
        private T state;

        /// <summary>
        /// The cost of this state(used in different searching algorithms).
        /// </summary>
        private double cost;

        /// <summary>
        /// A property for the cost of the state.
        /// </summary>
        /// <returns>
        /// The cost of this state.
        /// </returns>
        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        /// <summary>
        /// This property represents the state which was previous to this state
        /// in the search.
        /// </summary>
        private State<T> cameFrom;

        /// <summary>
        /// Copy constructor for the state class, it is internal so we will have access to it
        /// only in this assembly, we will create the State only in the StatePool.
        /// </summary>
        /// <param name="state">
        /// The state that we are going to replicate.
        /// </param>
        internal State(T state)
        {
            this.state = state;
            this.cameFrom = null;
        }

        /// <summary>
        /// Constructor for the State class, we will get the real state and 
        /// the cost of this state.
        /// </summary>
        /// <param name="state">
        /// The real state that we are going to encapsulate.
        /// </param>
        /// <param name="cost">
        /// The cost of this state.
        /// </param>
        internal State(T state, double cost)
        {
            this.state = state;
            this.cost = cost;
            this.cameFrom = null;
        }

        /// <summary>
        /// We will override the Equals method from object to compare states by first
        /// checking if the reference which is given isn't null and then compare the states.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) // we override Object's Equals method
        {
            return !ReferenceEquals(obj, null) && state.Equals((obj as State<T>).state);

        }

        //We may use this in the future so we left this code here for now.
        //public int CompareTo(object obj)
        //{
        //    if (!ReferenceEquals(obj, null))
        //    {
        //        State<T> other = obj as State<T>;
        //        double diff = Cost - other.Cost;
        //        return (diff > epsilon || -diff > epsilon) ? ((diff > epsilon) ? 1 : -1) : 0;
        //    }
        //    return -1;
        //    //int res = state.CompareTo(other.state);
        //    //if(res==0 && ((Cost - other.Cost) > epsilon || (other.Cost - Cost) > epsilon))
        //    //{
        //    //    res = ((Cost - other.Cost) > epsilon) ? 1 : -1;
        //    //}
        //    //return res;
        //}

        /// <summary>
        /// Override the GetHashCode from object.
        /// </summary>
        /// <returns>
        /// The hashcode of the state property.
        /// </returns>
        public override int GetHashCode()
        {
            /*
             * TODO change it if we'll want to use the state as a dictionary key
             * and we'll want to know the difference between two States that have
             * the same "state" but different "cost" or "cameFrom".
             */
            return state.GetHashCode();
        }

        /// <summary>
        /// Set the state that we came from to this state.
        /// </summary>
        /// <param name="cameFrom">
        /// The previous state in the search.
        /// </param>
        public void SetCameFrom(State<T> cameFrom)
        {
            this.cameFrom = cameFrom;
        }

        /// <summary>
        /// Get the previous state to this one.
        /// </summary>
        /// <returns>
        /// The state that we came from to this state.
        /// </returns>
        public State<T> GetCameFrom()
        {
            return this.cameFrom;
        }

        /// <summary>
        /// Get the real state.
        /// </summary>
        /// <returns>
        /// The state member of the type T of our template.
        /// </returns>
        public T GetState()
        {
            return this.state;
        }

        /// <summary>
        /// Operator overloading for the == operator to compare two states.
        /// </summary>
        /// <param name="s1">
        /// The first state we are going to compare.
        /// </param>
        /// <param name="s2">
        /// The second state that we are going to compare.
        /// </param>
        /// <returns>
        /// A boolean, true if they are equal and false otherwise.
        /// </returns>
        public static bool operator ==(State<T> s1, State<T> s2)
        {
            // the "ReferenceEquals(s1, s2)" will evaluate only if s1 is null.
            // And it will be true only if s2 is also null.
            return !ReferenceEquals(s1, null) && s1.Equals(s2) || ReferenceEquals(s1, s2);
        }

        /// <summary>
        /// Operator overloading for != ,check if two states are different.
        /// </summary>
        /// <param name="s1">
        /// The first state that we are going to compare.
        /// </param>
        /// <param name="s2">
        /// The second state that we are going to compare.
        /// </param>
        /// <returns>
        /// True if they are unequal and false otherwise.
        /// </returns>
        public static bool operator !=(State<T> s1, State<T> s2)
        {
            return !(s1 == s2);
        }
    }
}
