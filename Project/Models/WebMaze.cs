using MazeLib;

namespace Project.Models
{
    /// <summary>
    /// This class purpose is to be a maze. It contain all the required data.
    /// </summary>
    public class WebMaze
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public Position Start { get; set; }
        public Position End { get; set; }
        public string Maze { get; set; }

        /// <summary>
        /// set this maze to be an exact copy of the given maze.
        /// </summary>
        /// <param name="original"> Maze class object </param>
        public void SetMaze(Maze original)
        {
            //Original = original;
            Rows = original.Rows;
            Cols = original.Cols;
            Name = original.Name;
            Start = original.InitialPos;
            End = original.GoalPos;
            //Allocate array.
            Maze = "";
            //Copy array.
            for (int i = 0; i < original.Rows; i++)
            {
                for (int j = 0; j < original.Cols; j++)
                {
                    if (original[i, j] == CellType.Free)
                    {
                        Maze += '0';
                    }
                    else
                    {
                        Maze += '1';
                    }
                }

            }
        }
    }
}