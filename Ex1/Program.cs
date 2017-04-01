using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace Ex1
{
    class Program
    {
        static void Main(string[] args)
        {
            Maze m = new Maze(5, 5);
            MazeGeneratorLib.DFSMazeGenerator dfsMaze = new MazeGeneratorLib.DFSMazeGenerator();
            Console.WriteLine("Hello World!!");
            Console.ReadKey();
        }
    }
}
