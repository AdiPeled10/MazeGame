using System;
using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Ex1
{
    class MazeToSearchableAdapter : ISearchable<Position>
    {
        private Maze maze;
        private StatePool<Position> statePool;

        public MazeToSearchableAdapter(Maze maze)
        {
            this.maze = maze;
            this.statePool = new StatePool<Position>();
        }

        public List<State<Position>> GetAllPossibleStates(State<Position> s)
        {
            List<State<Position>> neighbors = new List<State<Position>>();
            int i = s.GetState().Row, j = s.GetState().Col;
            /**
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
                        //neighbors.Add(d.ContainsKey(loc) ? (d[loc]) : (d[loc] = new State<Position>(loc, 1)));
                        //neighbors.Add(new State<Position>(loc, 1));
                        neighbors.Add(State<Position>.GetState(loc));
                    }
                }
                catch (IndexOutOfRangeException)
                {
                }
            }
            */

            // The neighbor on the left
            try
            {
                if (maze[i, j - 1] == CellType.Free)
                {
                    //neighbors.Add(d.ContainsKey(loc) ? (d[loc]) : (d[loc] = new State<Position>(loc, 1)));
                    //neighbors.Add(new State<Position>(loc, 1));
                    neighbors.Add(statePool.GetState(new Position(i, j - 1), 1));
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            // The neighbor on the right
            try
            {
                if (maze[i, j + 1] == CellType.Free)
                {
                    //neighbors.Add(d.ContainsKey(loc) ? (d[loc]) : (d[loc] = new State<Position>(loc, 1)));
                    //neighbors.Add(new State<Position>(loc, 1));
                    neighbors.Add(statePool.GetState(new Position(i, j + 1), 1));
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            // The neighbor above
            try
            {
                if (maze[i - 1, j] == CellType.Free)
                {
                    //neighbors.Add(d.ContainsKey(loc) ? (d[loc]) : (d[loc] = new State<Position>(loc, 1)));
                    //neighbors.Add(new State<Position>(loc, 1));
                    neighbors.Add(statePool.GetState(new Position(i - 1, j), 1));
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            // The neighbor below
            try
            {
                if (maze[i + 1, j] == CellType.Free)
                {
                    //neighbors.Add(d.ContainsKey(loc) ? (d[loc]) : (d[loc] = new State<Position>(loc, 1)));
                    //neighbors.Add(new State<Position>(loc, 1));
                    neighbors.Add(statePool.GetState(new Position(i + 1, j), 1));
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
            return neighbors;
        }

        public State<Position> GetGoalState()
        {
            return statePool.GetState(maze.GoalPos, 1);
        }

        public State<Position> GetInitialState()
        {
            return statePool.GetState(maze.InitialPos, 1);
        }
    }
}