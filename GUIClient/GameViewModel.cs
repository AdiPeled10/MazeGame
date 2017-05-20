using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUIClient;

namespace ViewModel
{
    /// <summary>
    /// Put in this class everything single and multiplayer viewmodels have in common.
    /// </summary>
    public abstract class GameViewModel : ViewModel
    {
        protected ClientModel model;

        /// <summary>
        /// Will be activated when client cant connect to server.
        /// </summary>
        public event Parameterless CantFindServer;

        /// <summary>
        /// String representation of the maze.
        /// </summary>
        protected string mazeString;

        protected Location startLocation;

        protected Location endLocation;

        protected int mazeRows;

        protected int mazeCols;

        protected string mazeName;

        public int MazeRows
        {
            get { return mazeRows; }
            set
            {
                mazeRows = value;
                NotifyPropertyChanged("MazeRows");
            }
        }

        public int MazeCols
        {
            get { return mazeCols; }
            set
            {
                mazeCols = value;
                NotifyPropertyChanged("MazeCols");
            }
        }

        public string MazeString
        {
            get { return mazeString; }
            set
            {
                if (mazeString != value)
                {
                    mazeString = value;
                    //use notify property changed.
                    NotifyPropertyChanged("MazeString");
                }
            }
        }

        public Location StartLocation
        {
            get { return startLocation; }
            set
            {
                if (startLocation != value)
                {
                    startLocation = value;
                    NotifyPropertyChanged("StartLocation");
                }
            }
        }

        public Location EndLocation
        {
            get { return endLocation; }
            set
            {
                if (endLocation != value)
                {
                    endLocation = value;
                    NotifyPropertyChanged("EndLocation");
                }
            }
        }

        public string MazeName
        {
            get { return mazeName; }
            set
            {
                mazeName = value;
                NotifyPropertyChanged("MazeName");
            }
        }

        public GameViewModel()
        {
            
            model = new ClientModel();
            //Add GotMaze to event to notify when maze was generarted.
            model.GeneratedMaze += GotMaze;
            //Add to event when client cant connect to server.
            model.MazeLoc += GotLocations;
            model.WhereIsServer += () => { CantFindServer(); };
            model.MazeRowsCols += RowsAndCols;
            model.NotifyName += GetName;
        }
        
        /// <summary>
        /// Connect to server.
        /// </summary>
        public void Connect()
        {
            model.Connect();
        }

        public abstract void GenerateMaze(string name, int rows, int cols);

        /// <summary>
        /// Update maze via event called from model.
        /// </summary>
        /// <param name="maze"></param>
        public void GotMaze(string maze)
        {
            MazeString = maze;
        }

        /// <summary>
        /// Get starting and ending locations of the maze.
        /// Will be activated through event in model.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void GotLocations(Location start, Location end)
        {
            StartLocation = start;
            EndLocation = end;
        }

        public void RowsAndCols(int rows, int cols)
        {
            MazeRows = rows;
            MazeCols = cols;
        }

        public void GetName(string name)
        {
            MazeName = name;
        }
    }
}
