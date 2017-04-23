using System;
using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;

namespace Ex1
{
    /// <summary>
    /// Implementation of the adapter design pattern to turn a Maze to
    /// a searchable.
    /// </summary>
    class MazeToSearchableAdapter : ISearchable<Position>
    {
        /// <summary>
        /// The maze we will use for this adapter.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// The state pool that we will use during the search.
        /// </summary>
        private StatePool<Position> statePool;

        /// <summary>
        /// Constructor for the Maze to Searchable adapter that gets the Maze.
        /// </summary>
        /// <param name="maze">
        /// The maze that we will use for this adapter.
        /// </param>
        public MazeToSearchableAdapter(Maze maze)
        {
            this.maze = maze;
            this.statePool = new StatePool<Position>();
        }

        /// <summary>
        /// Get the neighbours of a specific state in the maze.
        /// </summary>
        /// <exception cref="Exception">
        /// There could be an exception if we check for a position which is invalid.
        /// </exception>
        /// <param name="s">
        /// The maze that we will return it's neighbours.
        /// </param>
        /// <returns>
        /// A list of states as neighbours.
        /// </returns>
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

        /// <summary>
        /// Get the goal state of the maze.
        /// </summary>
        /// <returns>
        /// The goal state.
        /// </returns>
        public State<Position> GetGoalState()
        {
            return statePool.GetState(maze.GoalPos, 1);
        }

        /// <summary>
        /// Get the initial state of the maze.
        /// </summary>
        /// <returns>
        /// The initial state.
        /// </returns>
        public State<Position> GetInitialState()
        {
            return statePool.GetState(maze.InitialPos, 1);
        }
    }
}
