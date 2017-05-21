using System.Collections.Generic;

namespace ViewModel
{
    /// <summary>
    /// This class is responsible on the representing locations as ints.
    /// It also converts ints to locations(if possible) and saves the player location.
    /// </summary>
    public class MazeLogic
    {
        /// <summary>
        /// List of illegal locations on the maze.
        /// </summary>
        private List<int> illegalLocations;

        /// <summary>
        /// Maps ints called "SerialNumbers" to location.
        /// </summary>
        private Dictionary<int, Location> map;

        /// <summary>
        /// The player int that fits his location.
        /// </summary>
        private int playerSerialNumber;

        /// <summary>
        /// Save current location of the player.
        /// </summary>
        private Location playerLocation;

        /// <summary>
        /// PlayerSerialNumber property.
        /// Return or set the playerSerialNumber member.
        /// </summary>
        public int PlayerSerialNumber
        {
            get { return playerSerialNumber; }
            set
            {
                playerSerialNumber = value;
            }
        }

        /// <summary>
        /// PlayerLocation property.
        /// Return or set the playerLocation member.
        /// </summary>
        public Location PlayerLocation
        {
            get { return playerLocation; }
            set {
                playerLocation = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// Creates illegalLocations and map.
        /// </summary>
        public MazeLogic()
        {
            illegalLocations = new List<int>();
            map = new Dictionary<int, Location>(64);
        }

        /// <summary>
        /// Adds location and it's corresponding int to "map".
        /// </summary>
        /// <param name="serial"> The location SerialNumber". </param>
        /// <param name="loc"> a location on the maze. </param>
        public void AddToMap(int serial, Location loc)
        {
            map[serial] = loc;
        }

        /// <summary>
        /// Returns the location that fits the int "serial"
        /// </summary>
        /// <param name="serial"> an int. </param>
        /// <returns> Location as in the summery. </returns>
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
