using System;
using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Ex1
{
    class MazeToSearchableAdapter : ISearchable<Position>
    {
        private Maze maze;
   

        public MazeToSearchableAdapter(Maze maze)
        {
            this.maze = maze;
        }

        public List<State<Position>> GetAllPossibleStates(State<Position> s)
        {
            List<State<Position>> neighbors = new List<State<Position>>();
            int i = s.GetState().Row, j = s.GetState().Col;
            Position[] locations = new Position[] {
                new Position(i - 1, j),
                new Position(i, j - 1),
                new Position(i, j + 1),
                new Position(i + 1, j)
            };
            foreach (Position loc in locations)
            {
                try
                {
                    if (maze[loc.Row, loc.Col] == CellType.Free)
                    {
                        neighbors.Add(new State<Position>(loc, 1));
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
