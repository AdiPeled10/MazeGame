using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MazeLogic
    {
        private List<int> illegalLocations;

        private Dictionary<int, Location> map;

        private int playerSerialNumber;


        /// <summary>
        /// Save current location of the player.
        /// </summary>
        private Location playerLocation;

        public int PlayerSerialNumber
        {
            get { return playerSerialNumber; }
            set
            {
                playerSerialNumber = value;
            }
        }

        public Location PlayerLocation
        {
            get { return playerLocation; }
            set { playerLocation = value; }
        }

        public MazeLogic()
        {
            illegalLocations = new List<int>();
            map = new Dictionary<int, Location>();
        }

        public void AddToMap(int serial,Location loc)
        {
            map[serial] = loc;
        }

        public Location GetLocation(int serial)
        {
            return map[serial];
        }

        /// <summary>
        /// When drawing a black location we will remember it is illegal
        /// </summary>
        /// <param name="loc"></param>
        public void AddIllegal(int loc)
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
        public bool IsIllegal(int serialNumber)
        {
            return illegalLocations.Contains(serialNumber);
        }
    }
}
