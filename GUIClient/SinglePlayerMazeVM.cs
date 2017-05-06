using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace ViewModel
{
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

        public Maze Maze
        {
            get { return maze; }
            set
            {
                maze = value;
                NotifyPropertyChanged("Maze");
            }
        }

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
