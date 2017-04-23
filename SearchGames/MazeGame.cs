using System;
using System.Collections.Generic;
using SearchAlgorithmsLib;
using MazeLib;

namespace SearchGames
{
    /// <summary>
    /// This is the MazeGame class, it implements the ISearchGame interface and implements it's
    /// methods to match the MazeGame that we want to create.
    /// </summary>
    public class MazeGame : ISearchGame
    {
        /// <summary>
        /// The maze which the game is played on.
        /// </summary>
        private Maze maze;

        /// <summary>
        /// The list of players in this game.
        /// </summary>
        private List<Player> players;

        /// <summary>
        /// The winner of this game.
        /// </summary>
        private Player winner;

        /// <summary>
        /// An event that's activated to tell when the game starts.
        /// TODO check what happens when it's empty but called
        /// </summary>
        public event Action TellMeWhenTheGameStarts;

        /// <summary>
        /// A delegate that tells us to start the MazeGame when some condition is true.
        /// Only a suggestion. a player can move before it's true.
        /// </summary>
        private StartWhenTrue startWhenTrue;

        /// <summary>
        /// The max number of players allowed in this game.
        /// TODO only not const until the client will handle single player.
        /// </summary>
        public int MaxPlayersAllowed = 2;

        /// <summary>
        /// A member that tells us if this game has ended.
        /// </summary>
        private bool hasEnded;

        /// <summary>
        /// Constructor for the MazeGame which gets the name and the Maze which
        /// the MazeGame operates on.
        /// </summary>
        /// <param name="name">
        /// The name of the maze.
        /// </param>
        /// <param name="maze">
        /// The maze that the game will be played on.
        /// </param>
        public MazeGame(string name, Maze maze)
        {
            this.maze = maze;
            this.maze.Name = name;
            this.players = new List<Player>(MaxPlayersAllowed);
            this.hasEnded = false;
            this.startWhenTrue = (g) => true;
        }

        /// <summary>
        /// Set the delegate that tells the game to start when some condition is true.
        /// </summary>
        /// <param name="func">
        /// The delegate that we will save as a member.
        /// </param>
        public void SetStartWhenTrue(StartWhenTrue func)
        {
            startWhenTrue = func;
        }

        /// <summary>
        /// Get name of this game.
        /// </summary>
        /// <return>
        /// The name of the game.
        /// </return>
        public string Name
        {
            get { return maze.Name; }
        }

        /// <summary>
        /// Get the number of players in this game.
        /// </summary>
        /// <return>
        /// The number of players in the game.
        /// </return>
        public int NumOfPlayer
        {
            get { return players.Count; }
        }

        /// <summary>
        /// Tells us if a player can join this game.
        /// </summary>
        /// <returns>
        /// Returns true if the game rules allow a new player to join at
        /// the moment of calling it, false otherwise.
        /// </returns>
        public bool CanAPlayerJoin()
        {
            return players.Count < MaxPlayersAllowed && !hasEnded;
        }

        /// <summary>
        /// Get the players in this game as a ReadOnlyList.
        /// </summary>
        /// <returns>
        /// A ReadOnlyList of the players in this game.
        /// </returns>
        public IReadOnlyList<Player> GetPlayers()
        {
            return players;
        }

        /**
         * Removes a player from the game. Due to the game logic of our choice, once it was called
         * the game ends and the remains player wins.
         */
        ///<summary>
        ///Removes a player from the game. Due to the game logic of our choice, once it was called
        ///the game ends and the remains player wins.
        /// </summary>
        public void RemovePlayer(Player player)
        {
            // TODO if the max number of player will become higher then 2, 
            // support "removing without ending the game if the game hasn't started"
            // TODO verify if a safety check of "does the player play in this game" is required.
            this.players.Remove(player);
            winner = players[0];
            // notify the other clients that a player has left the game in the future.
            winner.NotifyAChangeInTheGame("The other player has forfiet.");
            hasEnded = true;
        }

        /// <summary>
        /// Add a player to this MazeGame.
        /// </summary>
        /// <param name="player">
        /// The player that we are going to add to this game.
        /// </param>
        /// <returns>
        /// True if this player is added, false otherwise.
        /// </returns>
        public bool AddPlayer(Player player)
        {
            if (players.Count < MaxPlayersAllowed && !players.Contains(player))
            {
                players.Add(player);
                player.Location = maze.InitialPos;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return this maze game as a Searchable by using our adapter.
        /// </summary>
        /// <returns>
        /// Returns this MazeGame in the form of a Searchable.
        /// </returns>
        public ISearchable<Position> AsSearchable()
        {
            return new MazeToSearchableAdapter(maze);
        }

        /// <summary>
        /// Move a player on the game maze.
        /// </summary>
        /// <param name="player">
        /// The player that is moved.
        /// </param>
        /// <param name="move">
        /// The direction of the movement.
        /// </param>
        /// <return>
        /// True if the direction is legal,false otherwise.
        /// </return>
        public bool MovePlayer(Player player, Direction move)
        {
            // If the game hasn't ended. This prevents player from keep playing after someone has won.
            if (!HasEnded())
            {
                //Find the matching player which holds his location and if a player was found, move him.
                if ((bool)players.Find(player.Equals)?.Move(move, IsLegalMove) == false)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks for a Position if a move to this position is legal.
        /// </summary>
        /// <param name="loc">
        /// The Position that we will check if it's legal.
        /// </param>
        /// <exception cref="Exception">
        /// If the position is illegal we will know if an excption
        /// is thrown.
        /// </exception>
        /// <returns>
        /// True if it's legal, false otherwise.
        /// </returns>
        private bool IsLegalMove(Position loc)
        {
            try
            {
                return maze[loc.Row, loc.Col].Equals(CellType.Free);
            }
            catch // If it causes an exception, it's probably not legal.
            {
                return false;
            }
        }

        /// <summary>
        /// Tells us if a game can start.
        /// </summary>
        /// <returns>
        /// True if it can start,false otherwise.
        /// </returns>
        public bool CanStart()
        {
            return startWhenTrue(this);
        }

        /// <summary>
        /// Start this game.
        /// </summary>
        public void Start()
        {
            TellMeWhenTheGameStarts();
        }

        /// <summary>
        /// Tells us if this game has ended.
        /// </summary>
        /// <returns>
        /// True if the game ended, false otherwise.
        /// </returns>
        public bool HasEnded()
        {
            if (!hasEnded)
            {
                Position goal = maze.GoalPos;
                foreach (Player p in players)
                {
                    if (!p.Location.Equals(goal))
                    {
                        // Do nothing
                    }
                    else
                    {
                        winner = p;
                        hasEnded = true;
                        return true;
                    }
                }
            }
            return hasEnded;
        }

        /// <summary>
        /// Close this game.
        /// </summary>
        /// <param name="closingPlayer">
        /// The player which sent the Close command.
        /// </param>
        /// <param name="closingMessage">
        /// The message of the request to close.
        /// </param>
        public void Close(Player closingPlayer, string closingMessage = "{}")
        {
            // Notify the othe players the game is closed
            foreach (Player p in players)
            {
                if (!closingPlayer.Equals(p))
                {
                    // Sending an empty JSON object
                    p.NotifyAChangeInTheGame(closingMessage);
                }
            }
        }

        //Didn't delete it cause we may use it in the future.
        //public void DecalreWinner(string winnerMessage, string loserMessage)
        //{
        //    /*
        //     * TODO for now this method doesn't notify anyone because we don't need/know how to handle
        //     * it right now (it creates a problem of "when the client will see the message" if the client
        //     * is in "read input" state before the message got to him). In the future we probably move this
        //     * logic to the client application to easy the work of the server.
        //     */
        //    //winner.NotifyAChangeInTheGame(winnerMessage);
        //    hasEnded = true;
        //    foreach (Player p in players)
        //    {
        //        if (!winner.Equals(p))
        //        {
        //            //p.NotifyAChangeInTheGame(loserMessage);
        //        }
        //    }
        //}

        /// <summary>
        /// Get the SearchArea of this MazeGame by using the toString
        /// method of the Maze class.
        /// </summary>
        /// <returns>
        /// Representation of the maze as a string.
        /// </returns>
        public string GetSearchArea()
        {
            return maze.ToString();
        }

        //May use this later,so we didn't delete this.
        //public string Serialize()
        //{
        //    return Name + '\0' + maze.ToJSON();
        //}

        //public static MazeGame FromSerialized(string str)
        //{
        //    int splitAt = str.IndexOf('\0');
        //    string name = str.Substring(0, splitAt);
        //    Maze maze = Maze.FromJSON(str.Substring(splitAt + 1));
        //    return new MazeGame(name, maze);
        //}

        /// <summary>
        /// Convert this MazeGame to JSON format.
        /// </summary>
        /// <returns>
        /// Return a string of the JSON format of this class.
        /// </returns>
        public string ToJSON()
        {
            return maze.ToJSON();
        }

        /// <summary>
        /// Static method to convert back to Maze game from the JSON format.
        /// </summary>
        /// <param name="str">
        /// The name of the MazeGame.
        /// </param>
        /// <returns>
        /// The MazeGame that we have created.
        /// </returns>
        public static MazeGame FromJSON(string str)
        {
            Maze maze = Maze.FromJSON(str);
            return new MazeGame(maze.Name, maze);
        }

        /// <summary>
        /// Notify the players of the moves that the other players have made.
        /// </summary>
        /// <param name="format">
        /// The format that we will use to notify the players.
        /// </param>
        public void MakePlayersNotifyEachOtherAboutTheirMoves(FormatNotificationToListeners format)
        {
            MoveListener notifyFunc;
            // Delete old listeners and set format
            foreach (Player player in players)
            {
                player.ClearListeners();
                player.SetFormat(format);
            }

            // Set each player new listeners
            foreach (Player player in players)
            {
                notifyFunc = (move) =>
                {
                    try
                    {
                        player.NotifyAChangeInTheGame(move);
                    }
                    catch
                    {
                        // Do nothing, to avoid efecting the other players
                    }
                };
                foreach (Player p in players)
                {
                    if (!player.Equals(p))
                    {
                        p.NotifyMeWhenYouMove += notifyFunc;
                    }
                }
            }
        }
    }
}
