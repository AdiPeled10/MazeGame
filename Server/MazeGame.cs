using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;

namespace Server
{
    public class MazeGame : ISearchGame
    {
        private string name;
        private Maze maze;
        private List<Player> players;

        public MazeGame(string name, Maze maze)
        {
            this.maze = maze;
            this.name = name;
        }

        public void AddPlayer(Player player)
        {
            //TODO- Check if we need to check here that there are at most 2 players.
            players.Add(player);
        }

        public ISearchable<Position> AsSearchable()
        {
            return new MazeToSearchableAdapter(maze);
        }

        public bool IsPlaying(Player player)
        {
           if (FindPlayer(player) == null)
            {
                return false;
            }
            return true;
        }

        private Player FindPlayer(Player player)
        {
            foreach (Player myPlayer in players)
            {
                if (myPlayer.Equals(player.Client))
                {
                    //This player is playing in this game.
                    return myPlayer;
                }
            }
            return null;
        }

        public void MovePlayer(Player player,Direction move)
        {
            //Find the matching player which holds location.
            Player currentPlayer = FindPlayer(player);
            if (currentPlayer == null)
                return;
            currentPlayer.Move(move);
        }
        
    }
}
