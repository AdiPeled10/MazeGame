using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;

namespace Server
{
    // TODO add support to "time since last move" or something like that.
    public interface ISearchGame  // IGameWithMovingPlayers ?
    {
        //Get name of this game.
        string Name
        {
            get;
        }

        int NumOfPlayer
        {
            get;
        }

        string ToJSON();

        //returns true if the game rules allow a new player to join at the moment of calling it, false otherwise.
        bool CanAPlayerJoin();

        //Add a player to the game if it can. returns "true" on success and "false" otherwise.
        bool AddPlayer(Player player);

        //Move a player.
        void MovePlayer(Player player, Direction move); // TODO consider copy the Direction to here to avoid dependecy

        //// returns
        //unsigned int GetMaxPlayersAllowed();

        //Returns a searchable that represent the search game (recommended to return an adapter)
        ISearchable<Position> AsSearchable();

        // returns true if the game has ended.
        bool hasEnded();

        // serleize the game
        string Serialize();

        List<Player> GetPlayers();

        ////Start the game.
        //void Start();
        
        ////Tells us if the game already started.
        //bool Started();

        ////Close the game.
        //void Close();

        ////Check if player is playing in this game.
        //bool IsPlaying(Player player);
    }
}
