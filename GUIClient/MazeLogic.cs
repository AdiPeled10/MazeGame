using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MazeLogic
    {
        private List<Location> illegalLocations;

        /// <summary>
        /// Save current location of the player.
        /// </summary>
        private Location playerLocation;

        public Location PlayerLocation
        {
            get { return playerLocation; }
            set { playerLocation = value; }
        }

        public MazeLogic()
        {
            illegalLocations = new List<Location>();
        }

        /// <summary>
        /// When drawing a black location we will remember it is illegal
        /// </summary>
        /// <param name="loc"></param>
        public void AddIllegal(Location loc)
        {
            illegalLocations.Add(loc);
        }

        /// <summary>
        /// Check if a location is illegal.
        /// </summary>
        /// <param name="loc">
        /// The location we will check.
        /// </param>
        /// <returns>
        /// True if illegal false otherwise.
        /// </returns>
        public bool IsIllegal(Location loc, double xLimit, double yLimit)
        {
            if (loc.X >= 0 && loc.X <= xLimit
                && loc.Y >= 0 && loc.Y <= yLimit)
                return illegalLocations.Contains(loc);
            return true;
        }
    }
}
