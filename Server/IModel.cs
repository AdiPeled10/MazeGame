using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using SearchAlgorithmsLib;
namespace Server
{
    public interface IModel
    {
        Maze GenerateMaze(string name, int rows, int cols);

        Solution<Position> ComputeSolution(string name, int algorithm);

        void StartGame(string name, int rows, int cols);

        List<string> GetJoinableGamesList();

        void Join(string name,Player player);

        void Play(Direction move,Player player);

        void Close(string name);


    }
}
