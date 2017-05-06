using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUIClient
{
    /// <summary>
    /// This class will help us save locations of rectangles that were drawn on the canvas
    /// for the maze.
    /// </summary>
    public class Location
    {
        private double x;
        private double y;

        public Location(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public override int GetHashCode()
        {
            return base.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Location other = obj as Location;
            if (other != null)
            {
                return this.X == other.X && this.Y == other.Y;
            }
            return base.Equals(obj);
        }

    }
}
