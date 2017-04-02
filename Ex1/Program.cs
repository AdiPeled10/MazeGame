using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using MazeGeneratorLib;

namespace Ex1
{
    class Program
    {
        static void Main(string[] args)
        {
            DFSMazeGenerator dfsMaze = new DFSMazeGenerator();
            Maze m = dfsMaze.Generate(10, 5);
            m = new Maze(5, 5);
            Console.WriteLine(m[1, 1]);
            //Console.ReadKey();
        }
    }
}
