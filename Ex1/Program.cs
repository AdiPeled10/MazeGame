using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using MazeGeneratorLib;
using SearchAlgorithmsLib;

namespace Ex1
{
    /// <summary>
    /// In this program we will compare the two searchers that we wrote for the BFS and
    /// DFS  algorithm.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main will run the function that compares both solvers.
        /// </summary>
        /// <param name="args">
        /// Command line arguments.
        /// </param>
        static void Main(string[] args)
        {
            CompareSolvers();
            //Console.ReadKey();
        }

        /// <summary>
        /// Compare the solvers we created by using the adapter desing pattern between
        /// the maze and searchable.
        /// </summary>
        static void CompareSolvers()
        {
            DFSMazeGenerator dfsMaze = new DFSMazeGenerator();
            Maze maze = dfsMaze.Generate(100, 100);
            ISearchable<Position> adapt = new MazeToSearchableAdapter(maze);
            BFSSearcher<Position> bfs = new BFSSearcher<Position>();
            DFSSearcher<Position> dfs = new DFSSearcher<Position>();
            //Console.WriteLine(maze);
            bfs.Search(adapt);
            Console.WriteLine("Best First Search evaluated {0} nodes.", bfs.GetNumberOfNodesEvaluated());
            dfs.Search(adapt);
            Console.WriteLine("DFS evaluated {0} nodes.", dfs.GetNumberOfNodesEvaluated());
        }
    }
}
