using MazeLib;

namespace ViewModel
{
    /// <summary>
    /// Single player ViewModel.
    /// </summary>
    public class SinglePlayerMazeVM : ViewModel
    {
        /// <summary>
        /// The maze the game will be running on.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// Position of the player in the maze.
        /// </summary>
        private Position playerPos;

        /// <summary>
        /// Maze property.
        /// Get or set the "maze" member.
        /// If set, also call "NotifyPropertyChanged".
        /// </summary>
        public Maze Maze
        {
            get { return maze; }
            set
            {
                maze = value;
                NotifyPropertyChanged("Maze");
            }
        }

        /// <summary>
        /// PlayerPos property.
        /// Get or set the "playerPos" member.
        /// If set, also call "NotifyPropertyChanged".
        /// </summary>
        public Position PlayerPos
        {
            get { return playerPos; }
            set
            {
                playerPos = value;
                NotifyPropertyChanged("PlayerPos");
            }
        }
    }
}
