using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;
using SearchGames;
using ClientForServer;

namespace Model
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
