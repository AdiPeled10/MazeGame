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

        ISearchGame StartGame(string name, int rows, int cols, IClient client);

        List<string> GetJoinableGamesList();

        void Join(string name, IClient player);

        // returns the name of the game the move took place in
        string Play(Direction move, IClient player);

        void Close(string name);

        ISearchGame GetGameByName(string name);

        void RemoveClient(IClient client);
    }
}
