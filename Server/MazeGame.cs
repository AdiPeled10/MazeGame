﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;
using Newtonsoft.Json.Linq;

namespace Server
{
    public class MazeGame : ISearchGame
    {
        private Maze maze;
        private List<Player> players;
        private const int MaxPlayerAllowed = 2;

        public MazeGame(string name, Maze maze)
        {
            this.maze = maze;
            this.maze.Name = name;
            this.players = new List<Player>();
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
            return players.Count < MaxPlayerAllowed;
        }

        // TODO return Immutable list for safety
        public IReadOnlyList<Player> GetPlayers()
        {
            return players;
        }

        public bool AddPlayer(Player player)
        {
            //TODO Check how does it compares 2 players.
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

        //TODO decide if we want to notify user if the player isn't a part of the game.
        public void MovePlayer(Player player, Direction move)
        {
            //Find the matching player which holds his location and if a player was found, move him.
            players.Find(player.Equals)?.Move(move, maze.Cols, maze.Rows); // TODO make sure find works
            //FindPlayer(player)?.Move(move, maze.Cols, maze.Rows);
        }

        public bool HasEnded()
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
                    return true;
                }
            }
            return false;
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
            // TODO verify the maze ToJSON doen't do a lot we do
            JObject mazeObj = new JObject();
            mazeObj["Name"] = Name;
            mazeObj["Maze"] = maze.ToJSON(); // TODO verify it's the same as ToString
            mazeObj["Rows"] = maze.Rows;
            mazeObj["Cols"] = maze.Cols;
            Position start = maze.InitialPos;
            JObject startObj = new JObject();
            startObj["Row"] = start.Row;
            startObj["Col"] = start.Col;
            mazeObj["Start"] = startObj;
            Position end = maze.GoalPos;
            JObject endObj = new JObject();
            endObj["Row"] = end.Row;
            endObj["Col"] = end.Col;
            mazeObj["End"] = endObj;
            return mazeObj.ToString();
        }

        public static MazeGame FromJSON(string str)
        {
            JObject mazeGameObj = JObject.Parse(str);
            Maze maze = Maze.FromJSON((string)mazeGameObj["Maze"]);
            return new MazeGame((string)mazeGameObj["Name"], maze);
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
