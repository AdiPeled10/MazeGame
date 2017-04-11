using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{

    //This class will help us connect between a player and his game.
    public class Connector
    {
        private Dictionary<Player, ISearchGame> playerToGame;
        private Dictionary<IClient, Player> clientToPlayer;
        private int idCounter;
        //private 

        public Connector()
        {
            playerToGame = new Dictionary<Player, ISearchGame>();
            clientToPlayer = new Dictionary<IClient, Player>();
            idCounter = 0;
        }

        public void AddPlayerToGame(IClient player,ISearchGame game)
        {
            Player myPlayer = new Player(player, idCounter);
            idCounter++;
            clientToPlayer[player] = myPlayer;
            playerToGame[myPlayer] = game;
        }
        
        public void AddClientToPlayer(IClient client,Player player)
        {
            clientToPlayer[client] = player;
        }

        public Player GetPlayer(IClient client)
        {
            try
            {
                return clientToPlayer[client];
            } catch (KeyNotFoundException exp)
            {
                return null;
            }
        }

        public ISearchGame GetGame(IClient player)
        {
            try
            {
                return playerToGame[clientToPlayer[player]];
            } catch (KeyNotFoundException exp)
            {
                return null;
            }
        }

        public void DeleteGame(ISearchGame game)
        {
            List<Player> players = game.GetPlayers();
            foreach (Player myPlayer in players)
            {
                playerToGame.Remove(myPlayer);
                clientToPlayer.Remove(myPlayer.Client);
            }
        }
    }
}
