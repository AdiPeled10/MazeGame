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
    class Program
    {
        static void Main(string[] args)
        {
            CompareSolvers();
            //Console.ReadKey();
        }

        static void CompareSolvers()
        {
            DFSMazeGenerator dfsMaze = new DFSMazeGenerator();
            Maze maze = dfsMaze.Generate(1500, 1500);
            ISearchable<Position> adapt = new MazeToSearchableAdapter(maze);
            BFSSearcher<Position> bfs = new BFSSearcher<Position>();
            DFSSearcher<Position> dfs = new DFSSearcher<Position>();
            Console.WriteLine(maze);
            bfs.Search(adapt);
            Console.WriteLine("Best First Search evaluated {0} nodes.", bfs.getNumberOfNodesEvaluated());
            dfs.Search(adapt);
            Console.WriteLine("DFS evaluated {0} nodes.", dfs.getNumberOfNodesEvaluated());
        }
    }
}
