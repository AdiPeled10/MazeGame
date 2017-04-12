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
    /*
     * This class will help us connect between a player and his game.
     * It has medium-high coupling with class Model. The only purpose of this class is to avoid handling
     * all the connections between objects in the Model. The Model maybe uses many of Connector methods but
     * it makes the Model simplier.
     */
    internal class Connector
    {
        private Dictionary<string, ISearchGame> nameToGame;
        private Dictionary<Player, ISearchGame> playerToGame;
        private Dictionary<IClient, Player> clientToPlayer;
        private int idCounter;
        //private 

        public Connector()
        {
            nameToGame = new Dictionary<string, ISearchGame>(20);
            playerToGame = new Dictionary<Player, ISearchGame>(20);
            clientToPlayer = new Dictionary<IClient, Player>(20);
            idCounter = 0;
        }

        public IEnumerable<ISearchGame> Games
        {
            get
            {
                return nameToGame.Values;
            }
        }

        public void AddClientToGame(IClient client, ISearchGame game)
        {
            playerToGame[GetPlayer(client)] = game;
        }

        public void AddClientToGame(IClient client, string name)
        {
            playerToGame[GetPlayer(client)] = nameToGame[name];
        }

        public void AddClientAsPlayer(IClient client, Player player) // why?
        {
            clientToPlayer[client] = player;
        }

        public void AddGame(string name, ISearchGame game)
        {
            nameToGame[name] = game;
        }

        public bool ContainsGame(string name)
        {
            return nameToGame.ContainsKey(name);
        }

        public Player GetPlayer(IClient client)
        {
            try
            {
                return clientToPlayer[client];
            }
            catch (KeyNotFoundException)
            {
                Player p = clientToPlayer[client] = new Player(client, idCounter);
                ++idCounter;
                return p;
            }
        }

        public ISearchGame GetGame(IClient player)
        {
            return GetGame(clientToPlayer[player]);
        }

        public ISearchGame GetGame(Player player)
        {
            return playerToGame[player];
        }

        public ISearchGame GetGame(string name)
        {
            return nameToGame[name];
        }

        public void DeleteGame(ISearchGame game)
        {
            IReadOnlyList<Player> players = game.GetPlayers();
            nameToGame.Remove(game.Name);
            foreach (Player myPlayer in players)
            {
                playerToGame.Remove(myPlayer);
                // clientToPlayer.Remove(myPlayer.Client);
            }
        }

        public void DeleteGame(string name)
        {
            IReadOnlyList<Player> players = nameToGame[name].GetPlayers();
            nameToGame.Remove(name);
            foreach (Player myPlayer in players)
            {
                playerToGame.Remove(myPlayer);
                // clientToPlayer.Remove(myPlayer.Client);
            }
        }
    }

    public delegate ISearchGame FromSerialized(string str);

    /*
     * TODO
     * security breach: if a muliplayer game has a name of a single player game it will hide
     * the existance of the single player game forever.
     */
    public class Model : IModel // consider usesing template so different games will have different models. it will allow us to have 2 different games with the same name.
    {
        private Dictionary<int, ISearcher<Position>> numToAlgorithm;
        private SolutionCache<string> cache;
        private ISearchGameGenerator generator;
        private FromSerialized fromSerialized;
        Connector connector; // TODO use this to get Player instead of getting Players as input
                             // or use it in commands to hide socket/clients from model (it may "demage"
                             // the controller, but not if will make the commands indepandece of the controller)

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
            // * TODO we don't save the maze in the dictionary because it's a single player maze and
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

        public ISearchGame GetGame(string str)
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

            ISearchGame game = GetGame(name);
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

        public void StartGame(string name, int rows, int cols, IClient creator)
        {
            // TODO allow creator of a game delete a game if only his playing their
            /*
             * If the game already exists, the user didn't used this command as he should and won't be getting a game.
             * If we would join the client anyway, we will ignore his requst to "rows", "cols" and maybe he does't want
             * to play in a different size maze(it's like the difference between requesting a 100 pieces puzzle and
             * getting a 2000 pieces puzzle).
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
                connector.AddGame(name, game);
                connector.AddClientToGame(creator, game);
            }
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

        public void Join(string name, IClient player)
        {
            // TODO Check if the game is already full and stuff like that.
            connector.AddClientToGame(player, name);
        }

        // TODO rewrite using connector or use connector at a command and just pass the game
        public void Play(Direction move, IClient player)
        {
            Player p = connector.GetPlayer(player);
            ISearchGame game = connector.GetGame(p);
            game.MovePlayer(p, move);
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
    }
}
