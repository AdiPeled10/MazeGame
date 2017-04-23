using System;
using System.Collections.Generic;
using SearchAlgorithmsLib;
using MazeLib;

namespace SearchGames
{
    /// <summary>
    /// We will use this delegate in all the SearchGames to activate the game when
    /// something is going to happen, each game can start for a different reason by a
    /// different delegate,it lets our code to be generic.
    /// </summary>
    /// <param name="game">
    /// The game that we are going to start with this delegate.
    /// </param>
    /// <returns>
    /// Returns a boolean, true if the game is going to be started, false otherwise.
    /// </returns>
    public delegate bool StartWhenTrue(ISearchGame game);

    /// <summary>
    /// We will use this delegate to define all the functions that with a given Position
    /// tells us if this position is even legal in the game.
    /// </summary>
    /// <param name="loc">
    /// The Position that we are going to check.
    /// </param>
    /// <returns>
    /// A boolean, true if it's legal false otherwise.
    /// </returns>
    public delegate bool IsLegalPlayerLocation(Position loc);

    // TODO add support to "time since last move" or something like that.
    /// <summary>
    /// This is an interface that represents all the abilities that we expect from
    /// all the Searching Games that we will implement in the future, maybe we will implement
    /// a Search Game which has similar methods to the MazeGame in the future,creates a generic code.
    /// </summary>
    public interface ISearchGame
    {

        /// <summary>
        /// Listener that will be notified when the game starts.
        /// </summary>
        event Action TellMeWhenTheGameStarts;

        /// <summary>
        /// Property for the name of the game.Every SearchGame will have a name.
        /// </summary>
        /// <returns>
        /// A string which represents the name of this game.
        /// </returns>
        string Name
        {
            get;
        }

        /// <summary>
        /// Property for the number of players in the SearchGame.
        /// </summary>
        /// <returns>
        /// Integer which represents the number of players.
        /// </returns>
        int NumOfPlayer
        {
            get;
        }


        /// <summary>
        /// Tells us if a player can join this SearchGame.
        /// </summary>
        /// <returns>
        /// Returns true if the game rules allow a new player to join at the moment
        /// of calling it, false otherwise.
        /// </returns>
        bool CanAPlayerJoin();

        /// <summary>
        /// Add a player to the game if it can.
        /// </summary>
        /// <param name="player"></param>
        /// <returns>
        /// Returns "true" on success and "false" otherwise.
        /// </returns>
        bool AddPlayer(Player player);

        /// <summary>
        /// Returns the list of players in this game.
        /// </summary>
        /// <returns>
        /// ReadOnlyList because we don't want anyone to change the players.
        /// </returns>
        IReadOnlyList<Player> GetPlayers();

        /// <summary>
        /// Remove a player from this game.
        /// </summary>
        /// <param name="player">
        /// The player that we are going to remove.
        /// </param>
        void RemovePlayer(Player player);

        //Move a player.
        /// <summary>
        /// Move a player in the SearchGame.
        /// </summary>
        /// <param name="player">
        /// The player that we are going to move.
        /// </param>
        /// <param name="move">
        /// The direction that the player is going to move.
        /// </param>
        /// <return>
        /// True if the direction is legal,false otherwise.
        /// </return>
        bool MovePlayer(Player player, Direction move);

        //We left it here because we may use this in the future.
        //// returns
        //unsigned int GetMaxPlayersAllowed();

        /// <summary>
        /// Get the SearchGame as a Searchable.
        /// </summary>
        /// <returns>
        /// Returns a searchable that represent the search game (recommended to return an adapter).
        /// </returns>
        ISearchable<Position> AsSearchable();

        // set a function that wil detemine if the game can be started
        /// <summary>
        /// Set the delegate which is the function which will start the game when
        /// the condition is true.
        /// </summary>
        /// <param name="func">
        /// The function which we will use to start this game.
        /// </param>
        void SetStartWhenTrue(StartWhenTrue func);

        /// <summary>
        /// Start this SearchGame.
        /// </summary>
        void Start();

        /// <summary>
        /// Tells us if a game can be started.
        /// </summary>
        /// <returns>
        /// Returns true if the game can be started.
        /// </returns>
        bool CanStart();

        /// <summary>
        /// Tells us if this game has ended.
        /// </summary>
        /// <returns>
        /// Returns true if the game has ended.
        /// </returns>
        bool HasEnded();

        //We may use this in the future so we left it here.
        ///*
        // * TODO for now this method doesn't notify anyone because we don't need/know how to handle
        // * it right now (it creates a problem of "when the client will see the message" if the client
        // * is in "read input" state before the message got to him). In the future we probably move this
        // * logic to the client application to easy the work of the server.
        // */
        //void DecalreWinner(string winnerMessage, string losersMessage);


        ///<summary>
        ///Returns the Search Area of this search game.
        ///</summary>
        ///<returns>
        ///Returns the toString of the SearchArea.
        /// </returns>
        string GetSearchArea();

        /// <summary>
        /// Turn this SearchGame to JSON format.
        /// </summary>
        /// <returns>
        /// The string of the JSON format.
        /// </returns>
        string ToJSON();

        /// <summary>
        /// Make the players of the SearchGame to notify each other about their moves(multiplayer).
        /// </summary>
        /// <param name="format">
        /// The format of notification to the listeners.
        /// </param>
        void MakePlayersNotifyEachOtherAboutTheirMoves(FormatNotificationToListeners format);

        /// <summary>
        ///  Close this SearchGame, gets the player which sent the search message and
        ///  the closing message.
        /// </summary>
        /// <param name="closingPlayer">
        /// The player which sent the Close command.
        /// </param>
        /// <param name="closingMessage">
        /// The message that was sent.
        /// </param>
        void Close(Player closingPlayer, string closingMessage = "{}");

    }
}
