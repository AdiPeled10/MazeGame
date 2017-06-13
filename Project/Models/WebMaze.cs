using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using MazeLib;
using All;
using MazeGeneratorLib;
using SearchAlgorithmsLib;

namespace Project.Models
{
    public class WebMaze
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Cols { get; set; }
        public Position Start { get; set; }
        public Position End { get; set; }
        public string Maze { get; set; }
        //public Maze Original { get; set; }
        //public char[,] Maze { get; set; }

        
        public void SolveMaze()
        {
            
        }

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