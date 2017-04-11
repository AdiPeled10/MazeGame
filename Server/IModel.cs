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
        ISearchGame GenerateNewGame(string name, int rows, int cols);

        Solution<Position> ComputeSolution(string name, int algorithm);

        void StartGame(string name, int rows, int cols,IClient client);

        List<string> GetJoinableGamesList();

        void Join(string name,IClient player);

        void Play(Direction move,IClient player);

        void Close(string name);

        ISearchGame GetGame(string name);


    }
}
