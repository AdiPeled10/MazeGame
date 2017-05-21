
namespace ViewModel
{
    /// <summary>
    /// This class will help us save locations of rectangles that were drawn on the canvas
    /// for the maze.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// X coordiante of the location.
        /// </summary>
        private double x;

        /// <summary>
        /// Y coordiante of the location.
        /// </summary>
        private double y;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x"> X coordiante of the location. </param>
        /// <param name="y"> Y coordiante of the location. </param>
        public Location(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// X property.
        /// </summary>
        /// <value>
        /// The new X coordiante of the location.
        /// </value>
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Y property.
        /// </summary>
        /// <value>
        /// The new Y coordiante of the location.
        /// </value>
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Returns a hash value.
        /// </summary>
        /// <returns> Hash value. </returns>
        public override int GetHashCode()
        {
            return base.ToString().GetHashCode();
        }

        /// <summary>
        /// Checks if this instance and the given instance are equals.
        /// </summary>
        /// <param name="obj"> An object to be compared with. </param>
        /// <returns> True or False (equals or differ). </returns>
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
