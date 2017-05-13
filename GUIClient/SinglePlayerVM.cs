using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUIClient;

namespace ViewModel
{
    public class SinglePlayerVM : ViewModel
    {
        //Save instance of the model which is basically the client.
        private ClientModel model;

        /// <summary>
        /// String representation of the maze.
        /// </summary>
        private string mazeString;

        private Location startLocation;

        private Location endLocation;

        private int mazeRows;

        private int mazeCols;

        private string mazeName;

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

        public SinglePlayerVM()
        {
            model = new ClientModel();
            //Add GotMaze to event to notify when maze was generarted.
            model.GeneratedMaze += GotMaze;
            model.MazeLoc += GotLocations;
            model.MazeRowsCols += RowsAndCols;
            model.NotifyName += GetName;
        }

        public void GenerateMaze(string name,int rows,int cols)
        {
            //Generate maze through the model.
            model.GenerateMaze(name, rows, cols);
        }

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
        public void GotLocations(Location start,Location end)
        {
            StartLocation = start;
            EndLocation = end;
        }

        public void RowsAndCols(int rows,int cols)
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
