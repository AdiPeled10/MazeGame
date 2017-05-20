using System.Collections.Generic;
using SearchGames;
using ClientForServer;

namespace Models
{
    /*
      * This class will help us connect between a player and his game.
      * It has medium-high coupling with class Model. The only purpose of this class is to avoid handling
      * all the connections between objects in the Model. The Model maybe uses many of Connector methods but
      * it makes the Model simplier.
      */
    /// <summary>
    ///  This class will help us connect between a player and his game. 
    ///  It has medium-high coupling with class Model. The only purpose of this class is to avoid handling
    ///  all the connections between objects in the Model.The Model maybe uses many of Connector methods but
    ///  it makes the Model simplier.
      /// </summary>
    internal class Connector
    {
        /// <summary>
        /// A dictionary that matches between a name of a game to the game itself.
        /// </summary>
        private Dictionary<string, ISearchGame> nameToGame;

        /// <summary>
        /// A dictionary that matches between a Player to his SearchGame.
        /// </summary>
        private Dictionary<Player, ISearchGame> playerToGame;

        /// <summary>
        /// A dictionary that matches between an IClient to it's player.
        /// </summary>
        private Dictionary<IClient, Player> clientToPlayer;

        /// <summary>
        /// Counter for the id of the players which will be used as a hashcode later.
        /// </summary>
        private int idCounter;

        /// <summary>
        /// Default Constructor of the connector.
        /// </summary>
        public Connector()
        {
            nameToGame = new Dictionary<string, ISearchGame>(20);
            playerToGame = new Dictionary<Player, ISearchGame>(20);
            clientToPlayer = new Dictionary<IClient, Player>(20);
            idCounter = 0;
        }

        /// <summary>
        /// Return all the games in our dictionary.
        /// </summary>
        public IEnumerable<ISearchGame> Games
        {
            get
            {
                return nameToGame.Values;
            }
        }

        /// <summary>
        /// Add an entry that matches a client to his game in our data.
        /// </summary>
        /// <param name="client">
        /// The client which will be used as a key.
        /// </param>
        /// <param name="game">
        /// The game which will be used as a value.
        /// </param>
        public void AddClientToGame(IClient client, ISearchGame game)
        {
            AddClientToGame(GetPlayer(client), game);
        }

        /// <summary>
        /// Add an entry that matches a client to a game's name.
        /// </summary>
        /// <param name="client">
        /// The client used as a key.
        /// </param>
        /// <param name="name">
        /// The name of the game used as a value.
        /// </param>
        public void AddClientToGame(IClient client, string name)
        {
            AddClientToGame(GetPlayer(client), nameToGame[name]);
        }

        /// <summary>
        /// Add a player to game entry.
        /// </summary>
        /// <param name="player">
        /// The player which will be used as a key.
        /// </param>
        /// <param name="game">
        /// The SearchGame which will be used as a value.
        /// </param>
        public void AddClientToGame(Player player, ISearchGame game)
        {
            playerToGame[player] = game;
        }

        /// <summary>
        /// Add a matching between a player and the name of it's game.
        /// </summary>
        /// <param name="player">
        /// The player which will be used as a key.
        /// </param>
        /// <param name="name">
        /// The name of the game this player is in.
        /// </param>
        public void AddClientToGame(Player player, string name)
        {
            playerToGame[player] = nameToGame[name];
        }

        /// <summary>
        /// Add an entry to the dictionary from IClient to Player.
        /// </summary>
        /// <param name="client">
        /// The client which will be used as a key.
        /// </param>
        /// <param name="player">
        /// The player which will be used as a value.
        /// </param>
        public void AddClientAsPlayer(IClient client, Player player) 
        {
            clientToPlayer[client] = player;
        }

        /// <summary>
        /// Add a new SearchGame to the Connector's data.
        /// </summary>
        /// <param name="game">
        /// The game that will be added.
        /// </param>
        public void AddGame(ISearchGame game)
        {
            nameToGame[game.Name] = game;
        }

        /// <summary>
        /// Tells us if there is a game with the given name.
        /// </summary>
        /// <param name="name">
        /// The name that we will check.
        /// </param>
        /// <returns>
        /// True if there is a game with this name, false otherwise.
        /// </returns>
        public bool ContainsGame(string name)
        {
            return nameToGame.ContainsKey(name);
        }

        /// <summary>
        /// Get the player of the given IClient.
        /// </summary>
        /// <param name="client">
        /// The IClient that we want it's player.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        /// If there isn't an entry for the given IClient in the Connector.
        /// </exception>
        /// <returns>
        /// The player of the given IClient.
        /// </returns>
        public Player GetPlayer(IClient client)
        {
            try
            {
                return clientToPlayer[client];
            }
            catch (KeyNotFoundException)
            {
                Player p = clientToPlayer[client] = new Player(client, idCounter);
                ++idCounter;
                return p;
            }
        }

        /// <summary>
        /// Get the game of the given IClient.
        /// </summary>
        /// <param name="player">
        /// The IClient that we search for it's game.
        /// </param>
        /// <returns>
        /// The game of this IClient.
        /// </returns>
        public ISearchGame GetGame(IClient player)
        {
            return GetGame(clientToPlayer[player]);
        }

        /// <summary>
        /// Get the game of the given Player.
        /// </summary>
        /// <param name="player">
        /// The player that we search for it's game.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        /// If the player doesn't have an entry in the Connector.
        /// </exception>
        /// <returns>
        /// The game of the given player.
        /// </returns>
        public ISearchGame GetGame(Player player)
        {
            try
            {
                return playerToGame[player];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get the SearchGame with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the SearchGame that we are looking for.
        /// </param>
        /// <returns>
        /// The SearchGame with the given name.
        /// </returns>
        public ISearchGame GetGame(string name)
        {
            return nameToGame[name];
        }

        /// <summary>
        /// Delete the given SearchGame from the Connector's data.
        /// </summary>
        /// <param name="game">
        /// The game that we wish to remove.
        /// </param>
        public void DeleteGame(ISearchGame game)
        {
            DeleteGame(game?.Name);
        }

        /// <summary>
        /// Delete the game with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the game that we wish to remove.
        /// </param>
        public void DeleteGame(string name)
        {
            if (!ReferenceEquals(name, null))
            {
                try
                {
                    IEnumerable<Player> players = nameToGame[name].GetPlayers();
                    nameToGame.Remove(name);
                    foreach (Player myPlayer in players)
                    {
                        playerToGame[myPlayer] = null;
                        // clientToPlayer.Remove(myPlayer.Client);
                    }
                }
                catch
                {
                    // the game was already closed
                }
            }
        }

        /// <summary>
        /// Delete a Client from the Connector's data.
        /// </summary>
        /// <param name="client">
        /// The IClient that we wish to delete.
        /// </param>
        public void DeleteClient(IClient client)
        {
            Player p = this.clientToPlayer[client];
            clientToPlayer.Remove(client);
            playerToGame.Remove(p);
        }
    }
}
