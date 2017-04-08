using System;
using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Ex1
{
    class MazeToSearchableAdapter : ISearchable<Position>
    {
        private Maze maze;
        internal struct Location
        {
            public int row, col;
        }

        public MazeToSearchableAdapter(Maze maze)
        {
            this.maze = maze;
        }

        public List<State<Position>> GetAllPossibleStates(State<Position> s)
        {
            List<State<Position>> neighbors = new List<State<Position>>();
            int i = s.GetState().Row, j = s.GetState().Col;
            Location[] locations = new Location[] {
                new Location { row = i - 1, col = j },
                new Location { row = i, col = j - 1 },
                new Location { row = i, col = j + 1 },
                new Location { row = i + 1, col = j }
            };
            foreach (Location loc in locations)
            {
                try
                {
                    if (maze[loc.row, loc.col] == CellType.Free)
                    {
                        neighbors.Add(new State<Position>(new Position(loc.row, loc.col), 1));
                    }
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
            return neighbors;
        }

        public State<Position> GetGoalState()
        {
            return new State<Position>(maze.GoalPos, 1);
        }

        public State<Position> GetInitialState()
        {
            return new State<Position>(maze.InitialPos, 1);
        }
    }
}
