﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using MazeGeneratorLib;
using SearchAlgorithmsLib;

namespace Server
{
    public delegate ISearchGame FromSerialized(string str);

    /*
     * security breach: if a muliplayer game has a name of a single player game it will hide
     * the existance of the single player game forever.
     */
    public class Model : IModel // consider usesing template so different games will have different models. it will allow us to have 2 different games with the same name.
    {
        private Dictionary<int, ISearcher<Position>> numToAlgorithm;
        private SolutionCache<string> cache;
        private ISearchGameGenerator generator;
        private FromSerialized fromSerialized;
        Connector connector;

        // a model that doesn't support jsons
        public Model(ISearchGameGenerator generator) : this(generator, str => null)
        {
            //numToAlgorithm = new Dictionary<int, ISearcher<Position>>();
            //nameToGame = new Dictionary<string, ISearchGame>(); 
            ////Save the generator.
            //this.generator = generator;
            ////Create a new cache for all the solutions.
            //cache = new SolutionCache();
            //fromSerialized = str => null;

            //// initial numToAlgorithm
            //numToAlgorithm[0] = new BFSSearcher<Position>();
            //numToAlgorithm[1] = new DFSSearcher<Position>();
        }

        public Model(ISearchGameGenerator generator, FromSerialized fromSerialized)
        {
            numToAlgorithm = new Dictionary<int, ISearcher<Position>>();
            this.generator = generator;
            cache = new SolutionCache<string>();
            this.fromSerialized = fromSerialized;
            this.connector = new Connector();

            // initial numToAlgorithm
            numToAlgorithm[0] = new BFSSearcher<Position>();
            numToAlgorithm[1] = new DFSSearcher<Position>();
        }

        public ISearchGame GenerateNewGame(string name, int rows, int cols)
        {
            ///*
            // * we don't save the maze in the dictionary because it's a single player maze and
            // * don't has to be known at the sever. So when we write the client we'll want to
            // * to do a mapping between inner names and real names(like a Nat), it may not resolve
            // * the issue of "knowing" 
            // */
            //if (!nameToGame.ContainsKey(name) || nameToGame[name].NumOfPlayer == 0 || nameToGame[name].hasEnded())
            //{
            //    ISearchGame game = generator.Generate(name, rows, cols);
            //    //Add the new maze to the dictionary.
            //    nameToGame[name] = game;
            //    return game;
            //}
            //return null; // risky, but seem the most appropriate.

            /*
             * we don't need to save the game in the dictionary because it's a single player game and
             * doesn't has to be known by the sever. So when we write the client we'll want/need to
             * make sure to do a mapping between inner names and their serialization and send only the
             * serialization when the server is needed (for a solution for example).
             * see "GetGame" for the only dependecy on/use of the serialization for single player.
             */
            return generator.GenerateSearchGame(name, rows, cols);
        }

        public ISearchGame GetGameByName(string str)
        {
            ISearchGame game;
            try
            {
                game = connector.GetGame(str);
            }
            catch (KeyNotFoundException)
            {
                // Didn't find the key given in name. Perhapse it's an single player game so the name is
                // actually a serialized ISearchGame
                try
                {
                    game = fromSerialized(str);
                }
                catch (Exception)
                {
                    //Didn't find a key with that name and The name isn't a json.
                    game =  null;
                }
            }
            return game;
        }

        public Solution<Position> ComputeSolution(string name, int algorithm)
        {

            ISearchGame game = GetGameByName(name);
            if (!ReferenceEquals(game, null))
            {
                // game was created/found
                string key = game.GetSearchArea();
                if (cache.IsSolved(key))
                {
                    //Maze already solved, get solution from cache.
                    return cache.GetSolution(key);
                }
                Solution<Position> solution = numToAlgorithm[algorithm].Search(game.AsSearchable());
                //Add solution to the cache.
                cache.AddSolution(key, solution);
                return solution;
            }
            return new Solution<Position>();
        }

        public ISearchGame StartGame(string name, int rows, int cols, IClient creator)
        {
            /*
             * If the game already exists, the user didn't used this command as he should and won't be getting a game.
             * If we would join the client anyway, we will ignore his requst to "rows", "cols" and maybe he does't want
             * to play in a different size maze(it's like the difference between requesting a 100 pieces puzzle and
             * getting a 2000 pieces puzzle).
             * If a game with that name already exists, We will generate a new game for the client if and only if
             * he's the only player is the exitising game or, the exisiting game has ended.
             */
            bool toCreate = true;
            if (connector.ContainsGame(name))
             {
                ISearchGame g = connector.GetGame(name);
                // g.NumOfPlayer > 0 always because the server know only multiplayer games
                // replace the existing game if it has ended or the only player in it in the one who asks to replace it.
                toCreate = g.HasEnded() || (g.NumOfPlayer == 1 && g.GetPlayers().Contains(connector.GetPlayer(creator)));
                if (toCreate)
                {
                    connector.DeleteGame(g);
                }
            }
            if (toCreate)
            {
                // Create a game with this name.
                ISearchGame game = GenerateNewGame(name, rows, cols);
                connector.AddGame(game);
                connector.AddClientToGame(creator, game);
                return game;
            }
            return null;
        }

        public List<string> GetJoinableGamesList()
        {
            List<string> availableGames = new List<string>();
            IEnumerable<ISearchGame> values = connector.Games;
            foreach (ISearchGame game in values)
            {
                if (!game.CanAPlayerJoin())
                {
                    // this is probably the more common case, so it's empty for speed
                }
                else
                {
                    // The game allows a player to join.
                    availableGames.Add(game.Name);
                }
            }
            return availableGames;
        }

        public bool Join(string name, IClient player)
        {
            Player p = connector.GetPlayer(player);
            ISearchGame game = connector.GetGame(name);
            // if the player isn't a part of an existing game and the game allows a player to join
            if (ReferenceEquals(null, connector.GetGame(p)) && game.CanAPlayerJoin())
            {
                connector.AddClientToGame(p, name);
                game.AddPlayer(p);
                return true;
            }
            return false;
        }

        // set "name" to the game name and returns the list of players
        // Moves the client and returns the game where the player was moved
        public void Play(Direction move, IClient player)
        {
            Player p = connector.GetPlayer(player);
            ISearchGame game = connector.GetGame(p);
            game.MovePlayer(p, move);
        }

        public Player GetPlayer(IClient player)
        {
            return connector.GetPlayer(player);
        }

        public ISearchGame GetGameOf(IClient player)
        {
            return connector.GetGame(player);
        }

        public ISearchGame GetGameOf(Player player)
        {
            return connector.GetGame(player);
        }

        // remove unnecerssay games from the dictionary (games that have ended,
        // games that weren't active for very long time)
        public void Cleanup()
        {
            IEnumerable<ISearchGame> games = connector.Games;
            foreach (ISearchGame g in games)
            {
                if (g.HasEnded())
                {
                    // this.close(entry.Key);
                    connector.DeleteGame(g);
                }
            }
        }

        public void Close(string name)
        {
            connector.DeleteGame(name);
        }

        public void RemoveClient(IClient client)
        {
            Player p = connector.GetPlayer(client);
            ISearchGame game = connector.GetGame(p);
            // remove the clients player from his current game
            game?.RemovePlayer(p);
            // delete the clients player from the server
            connector.DeleteClient(client);
            // delete the game if the forfeit of the player made it end
            if (!ReferenceEquals(game, null) && game.HasEnded())
            {
                connector.DeleteGame(game);
            }
        }
    }
}
