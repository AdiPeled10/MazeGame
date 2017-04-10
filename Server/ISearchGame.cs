using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;

namespace Server
{
    public interface ISearchGame
    {
        //Add a player to this game.
        void AddPlayer(Player player);

        
        ISearchable<Position> AsSearchable();

        //Start the game.
        void Start();
        
        //Tells us if the game already started.
        bool Started();

        //Get name of this game.
        string GetName();

        //Close the game.
        void Close();

        //Check if player is playing in this game.
        bool IsPlaying(Player player);

        //Move a player.
        void MovePlayer(Player player,Direction move);
    }
}
