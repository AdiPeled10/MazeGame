using GUIClient;

namespace ViewModel
{
    /// <summary>
    /// Put in this class everything single and multiplayer viewmodels have in common.
    /// </summary>
    public abstract class GameViewModel : ViewModel
    {
        /// <summary>
        /// The model of the ViewModel in the MVVM standard.
        /// </summary>
        protected ClientModel model;

        /// <summary>
        /// Will be activated when client cant connect to server.
        /// </summary>
        public event Parameterless CantFindServer;

        /// <summary>
        /// String representation of the maze.
        /// </summary>
        protected string mazeString;

        /// <summary>
        /// The current maze starting location.
        /// </summary>
        protected Location startLocation;

        /// <summary>
        /// The current maze end location.
        /// </summary>
        protected Location endLocation;

        /// <summary>
        /// The current maze number of rows.
        /// </summary>
        protected int mazeRows;

        /// <summary>
        /// The current maze number of cols.
        /// </summary>
        protected int mazeCols;

        /// <summary>
        /// The current maze name.
        /// </summary>
        protected string mazeName;

        /// <summary>
        /// MazeRows property.
        /// </summary>
        /// <value>
        /// The number of rows in the current maze.
        /// calls "NotifyPropertyChanged" when changed.
        /// </value>
        public int MazeRows
        {
            get { return mazeRows; }
            set
            {
                mazeRows = value;
                NotifyPropertyChanged("MazeRows");
            }
        }

        /// <summary>
        /// MazeCols property.
        /// </summary>
        /// <value>
        /// The number of cols in the current maze.
        /// calls "NotifyPropertyChanged" when changed.
        /// </value>
        public int MazeCols
        {
            get { return mazeCols; }
            set
            {
                mazeCols = value;
                NotifyPropertyChanged("MazeCols");
            }
        }

        /// <summary>
        /// MazeString property. It's value is a sequance of 0,1
        /// where 1 represents a wall and 0 reprenets a free pass.
        /// calls "NotifyPropertyChanged" when changed.
        /// </summary>
        /// <value>
        /// A sequance of 0,1 where 1 represents a wall and 0 reprenets a free pass.
        /// </value>
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

        /// <summary>
        /// StartLocation property.
        /// calls "NotifyPropertyChanged" when changed.
        /// </summary>
        /// <value>
        /// A new starting location.
        /// </value>
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

        /// <summary>
        /// EndLocation property.
        /// calls "NotifyPropertyChanged" when changed.
        /// </summary>
        /// <value>
        /// A new ending location.
        /// </value>
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

        /// <summary>
        /// MazeName property.
        /// calls "NotifyPropertyChanged" when changed.
        /// </summary>
        /// <value>
        /// New name for the maze.
        /// </value>
        public string MazeName
        {
            get { return mazeName; }
            set
            {
                mazeName = value;
                NotifyPropertyChanged("MazeName");
            }
        }

        /// <summary>
        /// Constructor.
        /// Creates the client model and register to some of its events.
        /// </summary>
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

        /// <summary>
        /// Generates a new maze.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
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

        /// <summary>
        /// Sets the maze measurements.
        /// </summary>
        /// <param name="rows"> Number of rows. </param>
        /// <param name="cols"> Number of cols. </param>
        public void RowsAndCols(int rows, int cols)
        {
            MazeRows = rows;
            MazeCols = cols;
        }

        /// <summary>
        /// Sets the game name.
        /// </summary>
        /// <param name="name"> The new name. </param>
        public void GetName(string name)
        {
            MazeName = name;
        }
    }
}
