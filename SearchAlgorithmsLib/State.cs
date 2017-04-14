using System;
using System.Collections.Generic;

namespace SearchAlgorithmsLib
{
    public class State<T> : IComparable
    {
        private const double epsilon = 1e-14; // Math.Pow(10, 2 - 2 * sizeof(double));
        private T state; // the state
        private double cost;
        public double Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        private State<T> cameFrom; // the state we came from to this state (setter)

        private static Dictionary<T, State<T>> StatePool = new Dictionary<T, State<T>>(32);

        public static State<T> GetState(T state)
        {
            try
            {
                return State<T>.StatePool[state];
            }
            catch (KeyNotFoundException)
            {
                State<T> st = new State<T>(state);
                State<T>.StatePool[state] = st;
                return st;
            }
        }

        public static State<T> GetState(T state, double initializeCost)
        {
            try
            {
                return State<T>.StatePool[state];
            }
            catch (KeyNotFoundException)
            {
                State<T> st = new State<T>(state, initializeCost);
                State<T>.StatePool[state] = st;
                return st;
            }
        }

        private State(T state) // CTOR
        {
            this.state = state;
            //this.cameFrom = this;
        }

        private State(T state, double cost) // CTOR
        {
            this.state = state;
            this.cost = cost;
            //this.cameFrom = this;
        }

        public override bool Equals(object obj) // we override Object's Equals method
        {
            return !ReferenceEquals(obj, null) && state.Equals((obj as State<T>).state);
            //State<T> other = obj as State<T>;
            //return (Cost - other.Cost) < epsilon && (other.Cost - Cost) < epsilon && state.Equals(other.state);
        }
        
        public int CompareTo(object obj)
        {
            if (!ReferenceEquals(obj, null))
            {
                State<T> other = obj as State<T>;
                double diff = Cost - other.Cost;
                return (diff > epsilon || -diff > epsilon) ? ((diff > epsilon) ? 1 : -1) : 0;
            }
            return -1;
            //int res = state.CompareTo(other.state);
            //if(res==0 && ((Cost - other.Cost) > epsilon || (other.Cost - Cost) > epsilon))
            //{
            //    res = ((Cost - other.Cost) > epsilon) ? 1 : -1;
            //}
            //return res;
        }

        public override int GetHashCode()
        {
            /*
             * TODO change it if we'll want to use the state as a dictionary key
             * and we'll want to know the difference between two States that have
             * the same "state" but different "cost" or "cameFrom".
             */
            return state.GetHashCode();
        }

        public void SetCameFrom(State<T> cameFrom)
        {
            this.cameFrom = cameFrom;
        }

        public State<T> GetCameFrom()
        {
            return this.cameFrom;
        }

        public T GetState()
        {
            return this.state;
        }

        public static bool operator ==(State<T> s1, State<T> s2)
        {
            // the "ReferenceEquals(s1, s2)" will evaluate only if s1 is null.
            // And it will be true only if s2 is also null.
            return !ReferenceEquals(s1, null) && s1.Equals(s2) || ReferenceEquals(s1, s2);
        }

        public static bool operator !=(State<T> s1, State<T> s2)
        {
            return !(s1 == s2);
        }
    }
}
