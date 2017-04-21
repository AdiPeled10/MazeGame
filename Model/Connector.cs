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
    internal class Connector
    {
        private Dictionary<string, ISearchGame> nameToGame;
        private Dictionary<Player, ISearchGame> playerToGame;
        private Dictionary<IClient, Player> clientToPlayer;
        private int idCounter;
        //private 

        public Connector()
        {
            nameToGame = new Dictionary<string, ISearchGame>(20);
            playerToGame = new Dictionary<Player, ISearchGame>(20);
            clientToPlayer = new Dictionary<IClient, Player>(20);
            idCounter = 0;
        }

        public IEnumerable<ISearchGame> Games
        {
            get
            {
                return nameToGame.Values;
            }
        }

        public void AddClientToGame(IClient client, ISearchGame game)
        {
            AddClientToGame(GetPlayer(client), game);
        }

        public void AddClientToGame(IClient client, string name)
        {
            AddClientToGame(GetPlayer(client), nameToGame[name]);
        }

        public void AddClientToGame(Player player, ISearchGame game)
        {
            playerToGame[player] = game;
        }

        public void AddClientToGame(Player player, string name)
        {
            playerToGame[player] = nameToGame[name];
        }

        public void AddClientAsPlayer(IClient client, Player player) // why?
        {
            clientToPlayer[client] = player;
        }

        public void AddGame(ISearchGame game)
        {
            nameToGame[game.Name] = game;
        }

        public bool ContainsGame(string name)
        {
            return nameToGame.ContainsKey(name);
        }

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

        public ISearchGame GetGame(IClient player)
        {
            return GetGame(clientToPlayer[player]);
        }

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

        public ISearchGame GetGame(string name)
        {
            return nameToGame[name];
        }

        public void DeleteGame(ISearchGame game)
        {
            DeleteGame(game?.Name);
        }

        public void DeleteGame(string name)
        {
            if (!ReferenceEquals(name, null))
            {
                IEnumerable<Player> players = nameToGame[name].GetPlayers();
                nameToGame.Remove(name);
                foreach (Player myPlayer in players)
                {
                    playerToGame[myPlayer] = null;
                    // clientToPlayer.Remove(myPlayer.Client);
                }
            }
        }

        public void DeleteClient(IClient client)
        {
            Player p = this.clientToPlayer[client];
            clientToPlayer.Remove(client);
            playerToGame.Remove(p);
        }
    }
}
