using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public State(T state) // CTOR
        {
            this.state = state;
            //this.cameFrom = this;
        }

        public override bool Equals(object obj) // we override Object's Equals method
        {
            return state.Equals((obj as State<T>).state);
            //State<T> other = obj as State<T>;
            //return (Cost - other.Cost) < epsilon && (other.Cost - Cost) < epsilon && state.Equals(other.state);
        }
        

        public int CompareTo(object obj)
        {
            State<T> other = obj as State<T>;
            double diff = Cost - other.Cost;
            return (diff > epsilon || -diff > epsilon) ? ((diff > epsilon) ? 1 : -1) : 0;
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
            return s1.Equals(s2);
        }

        public static bool operator !=(State<T> s1, State<T> s2)
        {
            return !(s1 == s2);
        }
    }
}
