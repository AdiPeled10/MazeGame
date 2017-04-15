using System;
using System.Collections.Generic;
using SearchAlgorithmsLib;
using MazeLib;

namespace SearchGames
{
    public class MazeGame : ISearchGame
    {
        private Maze maze;
        private List<Player> players;
        private Player winner;
        public event Action TellMeWhenTheGameStarts; // TODO check what happens when it's empty but called 
        private StartWhenTrue startWhenTrue;
        public const int MaxPlayerAllowed = 2;
        private bool hasEnded;

        public MazeGame(string name, Maze maze)
        {
            this.maze = maze;
            this.maze.Name = name;
            this.players = new List<Player>();
            this.hasEnded = false;
            this.startWhenTrue = (g) => true;
        }

        public void SetStartWhenTrue(StartWhenTrue func)
        {
            startWhenTrue = func;
        }

        //Get name of this game.
        public string Name
        {
            get { return maze.Name; }
        }

        public int NumOfPlayer
        {
            get { return players.Count; }
        }

        //returns true if the game rules allow a new player to join at the moment of calling it, false otherwise.
        public bool CanAPlayerJoin()
        {
            return players.Count < MaxPlayerAllowed && !hasEnded;
        }

        public IReadOnlyList<Player> GetPlayers()
        {
            return players;
        }

        /**
         * Removes a player from the game. Due to the game logic of our choice, once it was called
         * the game ends and the remains player wins.
         */
        public void RemovePlayer(Player player)
        {
            // TODO if the max number of player will become higher then 2, 
            // support "removing without ending the game if the game hasn't started"
            // TODO verify if a safety check of "does the player play in this game" is required.
            this.players.Remove(player);
            winner = players[0];
            hasEnded = true;
        }

        public bool AddPlayer(Player player)
        {
            if (players.Count < MaxPlayerAllowed && !players.Contains(player))
            {
                players.Add(player);
                return true;
            }
            return false;
        }

        public ISearchable<Position> AsSearchable()
        {
            return new MazeToSearchableAdapter(maze);
        }

        public void MovePlayer(Player player, Direction move)
        {
            //Find the matching player which holds his location and if a player was found, move him.
            players.Find(player.Equals)?.Move(move, maze.Cols, maze.Rows);
        }

        public bool CanStart()
        {
            return startWhenTrue(this);
        }

        public void Start()
        {
            TellMeWhenTheGameStarts();
        }

        public bool HasEnded()
        {
            if (!hasEnded)
            {
                Position goal = maze.GoalPos;
                foreach (Player p in players)
                {
                    if (!p.Location.Equals(goal))
                    {
                        // do nothing
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

        public void DecalreWinner(string winnerMessage, string loserMessage)
        {
            players.Remove(winner);
            winner.NotifyAChangeInTheGame(winnerMessage);
            hasEnded = true;
            foreach (Player p in players)
            {
                p.NotifyAChangeInTheGame(loserMessage);
            }
        }

        public string GetSearchArea()
        {
            return maze.ToString();
        }

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

        public string ToJSON()
        {
            return maze.ToJSON();
        }

        public static MazeGame FromJSON(string str)
        {
            //JObject mazeGameObj = JObject.Parse(str);
            Maze maze = Maze.FromJSON(str);// (string)mazeGameObj["Maze"]);
            return new MazeGame(maze.Name, maze);// (string)mazeGameObj["Name"], maze);
        }

        public void MakePlayersNotifyEachOtherAboutTheirMoves(FormatNotificationToListeners format)
        {
            MoveListener notifyFunc;
            // delete old listeners and set format
            foreach (Player player in players)
            {
                player.ClearListeners();
                player.SetFormat(format);
            }

            // set each player new listeners
            foreach (Player player in players)
            {
                notifyFunc = (move) => player.NotifyAChangeInTheGame(move);
                foreach (Player p in players)
                {
                    if (!player.Equals(p))
                    {
                        p.NotifyMeWhenYouMove += notifyFunc;
                    }
                }
            }
        }

        //    public bool IsPlaying(Player player)
        //    {
        //       if (FindPlayer(player) == null)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }

        //    private Player FindPlayer(Player player)
        //    {
        //        foreach (Player myPlayer in players)
        //        {
        //            if (myPlayer.Equals(player.Client))
        //            {
        //                //This player is playing in this game.
        //                return myPlayer;
        //            }
        //        }
        //        return null;
        //    }
    }
}
