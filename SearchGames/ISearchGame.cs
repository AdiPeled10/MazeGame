using System;
using System.Collections.Generic;
using SearchAlgorithmsLib;
using MazeLib;

namespace SearchGames
{
    public delegate bool StartWhenTrue(ISearchGame game);
    public delegate bool IsLegalPlayerLocation(Position loc);

    // TODO add support to "time since last move" or something like that.
    public interface ISearchGame  // IGameWithMovingPlayers ?
    {
        // listener that will be notified when the game starts
        event Action TellMeWhenTheGameStarts;

        //Get name of this game.
        string Name
        {
            get;
        }

        int NumOfPlayer
        {
            get;
        }

        //returns true if the game rules allow a new player to join at the moment of calling it, false otherwise.
        bool CanAPlayerJoin();

        //Add a player to the game if it can. returns "true" on success and "false" otherwise.
        bool AddPlayer(Player player);

        IReadOnlyList<Player> GetPlayers();

        void RemovePlayer(Player player);

        //Move a player.
        void MovePlayer(Player player, Direction move); // consider copy the Direction to here to avoid dependecy

        //// returns
        //unsigned int GetMaxPlayersAllowed();

        //Returns a searchable that represent the search game (recommended to return an adapter)
        ISearchable<Position> AsSearchable();

        // set a function that wil detemine if the game can be started
        void SetStartWhenTrue(StartWhenTrue func);

        //Start the game.
        void Start();

        //returns true if the game can be started
        bool CanStart();

        // returns true if the game has ended.
        bool HasEnded();

        ///*
        // * TODO for now this method doesn't notify anyone because we don't need/know how to handle
        // * it right now (it creates a problem of "when the client will see the message" if the client
        // * is in "read input" state before the message got to him). In the future we probably move this
        // * logic to the client application to easy the work of the server.
        // */
        //void DecalreWinner(string winnerMessage, string losersMessage);

        // Returns a string that represents where the search occurs.
        // For example, if the game is a maze it will return a string representing the maze.
        string GetSearchArea();

        string ToJSON();

        void MakePlayersNotifyEachOtherAboutTheirMoves(FormatNotificationToListeners format);

        ////Tells us if the game already started.
        //bool Started();

        ////Close the game.
        //void Close();

        ////Check if player is playing in this game.
        //bool IsPlaying(Player player);
    }
}
