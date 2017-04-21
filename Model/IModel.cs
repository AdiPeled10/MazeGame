using System.Collections.Generic;
using MazeLib;
using SearchAlgorithmsLib;
using SearchGames;
using ClientForServer;

namespace Models
{
    /// <summary>
    /// Enum for the Searching Algorithms, 0 for BFS,1 for DFS.
    /// </summary>
    public enum Algorithm
    {
        BFS = 0,
        DFS = 1
    };

    /// <summary>
    /// The IModel interface is the set of abilities we expect from every model which we will use
    /// in the MVC architectural pattern in our application.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Generate a new game based on given name, number of rows and number of columns.
        /// </summary>
        /// <param name="name">
        /// The name of game.
        /// </param>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="cols">
        /// The number of columns.
        /// </param>
        /// <returns>
        /// The SearchGame that was created.
        /// </returns>
        ISearchGame GenerateNewGame(string name, int rows, int cols);

        /// <summary>
        /// Compute the solution to the game with the given name by using the given algorithm.
        /// </summary>
        /// <param name="name">
        /// The name of the game that we will solve.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm that we will use to solve the SearchGame.
        /// </param>
        /// <returns></returns>
        Solution<Position> ComputeSolution(string name, Algorithm algorithm);

        /// <summary>
        /// Start the SearchGame with the given name, number of rows and number of
        /// columns and give the name of the Client that sent the Start command.
        /// </summary>
        /// <param name="name">
        /// The name of the game.
        /// </param>
        /// <param name="rows">
        /// Number of rows in a game.
        /// </param>
        /// <param name="cols">
        /// Number of columns in a game.
        /// </param>
        /// <param name="client">
        /// The client that sent the Start command.
        /// </param>
        /// <returns></returns>
        ISearchGame StartGame(string name, int rows, int cols, IClient client);

        /// <summary>
        /// Get the list of all the games that we can join.
        /// </summary>
        /// <returns>
        /// A list of strings with the names of games that we can join.
        /// </returns>
        List<string> GetJoinableGamesList();

        /// <summary>
        /// Let a player join a game with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the game the client wants to join.
        /// </param>
        /// <param name="player">
        /// The client that sent the join request.
        /// </param>
        /// <returns></returns>
        bool Join(string name, IClient player);

        /// <summary>
        /// Move a player in a given direction in the SearchGame.
        /// </summary>
        /// <param name="move">
        /// The move which the player wants to move.
        /// </param>
        /// <param name="player">
        /// The player which sent the Play command.
        /// </param>
        void Play(Direction move, IClient player);

        /// <summary>
        /// Get the player of this specific IClient.
        /// </summary>
        /// <param name="player">
        /// The client which we want his player class.
        /// </param>
        /// <returns>
        /// The player object which holds this IClient.
        /// </returns>
        Player GetPlayer(IClient player);

        /// <summary>
        /// Get the game of the given IClient.
        /// </summary>
        /// <param name="player">
        /// The IClient that we are looking for his game.
        /// </param>
        /// <returns>
        /// The game of the given client.
        /// </returns>
        ISearchGame GetGameOf(IClient player);
        
        /// <summary>
        /// Get the game of a player.
        /// </summary>
        /// <param name="player">
        /// The player that we are looking for his game.
        /// </param>
        /// <returns>
        /// The game of the given player.
        /// </returns>
        ISearchGame GetGameOf(Player player);

        /// <summary>
        /// Close the given game.
        /// </summary>
        /// <param name="name">
        /// The name of the game we wish to close.
        /// </param>
        /// <param name="player">
        /// The player that sent the Close command.
        /// </param>
        void Close(string name, IClient player);

        /// <summary>
        /// Get the search game by it's name.
        /// </summary>
        /// <param name="name">
        /// The name of the game.
        /// </param>
        /// <returns>
        /// The SearchGame that matches this name.
        /// </returns>
        ISearchGame GetGameByName(string name);

        /// <summary>
        /// Remove a client from all of his games.
        /// </summary>
        /// <param name="client">
        /// The client that we wish to remove.
        /// </param>
        void RemoveClient(IClient client);
    }
}
