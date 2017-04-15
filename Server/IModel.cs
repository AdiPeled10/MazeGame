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

        bool Join(string name, IClient player);

        void Play(Direction move, IClient player);

        Player GetPlayer(IClient player);

        ISearchGame GetGameOf(IClient player);

        ISearchGame GetGameOf(Player player);

        void Close(string name);

        ISearchGame GetGameByName(string name);

        void RemoveClient(IClient client);
    }
}
