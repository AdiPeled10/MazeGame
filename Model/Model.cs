using System;
using System.Collections.Generic;
using System.Linq;
using MazeLib;
using SearchAlgorithmsLib;
using SearchGames;
using ClientForServer;

namespace Models
{
    /// <summary>
    /// Delegate that helps us serialize different names.
    /// </summary>
    /// <param name="str">
    /// The string that we will serialize.
    /// </param>
    /// <returns>
    /// Get the game from the string.
    /// </returns>
    public delegate ISearchGame FromSerialized(string str);

    /*
     * security breach: if a muliplayer game has a name of a single player game it will hide
     * the existance of the single player game forever.
     */
    /// <summary>
    /// This is the model that we will use to implement the MVC architectural pattern, it will implement all
    /// the methods that we have in the IModel interface to create the functionality,algorithms and handle all
    /// the data of the application.
    /// Security breach: if a muliplayer game has a name of a single player game it will hide 
    /// the existance of the single player game forever.
     /// </summary>
    public class Model : IModel 
    {
        /// <summary>
        /// Dictionary that matches between an algorithm to it's Searcher.
        /// </summary>
        private Dictionary<Algorithm, ISearcher<Position>> numToAlgorithm;

        /// <summary>
        /// A cache of solutions that we will use to save solutions that were already computed.
        /// </summary>
        private SolutionCache<string> cache;

        /// <summary>
        /// A generator the SearchGame.
        /// </summary>
        private ISearchGameGenerator generator;

        /// <summary>
        /// The delegate that we defined above of functions that get a string and
        /// returns a SearchGame.
        /// </summary>
        private FromSerialized fromSerialized;

        /// <summary>
        /// A connector that will help us handle all the collections between Players,IClients
        /// and SearchGames.
        /// </summary>
        Connector connector;

   
        /// <summary>
        /// Constructor of the Model class that gets a SearchGame generator and 
        /// sets the FromSerialized by default.
        /// </summary>
        /// <param name="generator">
        /// The SearchGameGenerator that we will use to create SearchGames.
        /// </param>
        public Model(ISearchGameGenerator generator) : this(generator, str => null)
        {
            //Maybe use it later,saved here for now.
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

        /// <summary>
        /// Constructor for the model class which gets the generator for the SearchGames and the 
        /// delegate that we will use to get SearchGames from serialized strings.
        /// </summary>
        /// <param name="generator">
        /// The generator that will help us create SearchGames.
        /// </param>
        /// <param name="fromSerialized">
        /// A function that will return a SearchGame from a Serialized string.
        /// </param>
        public Model(ISearchGameGenerator generator, FromSerialized fromSerialized)
        {
            numToAlgorithm = new Dictionary<Algorithm, ISearcher<Position>>();
            this.generator = generator;
            cache = new SolutionCache<string>();
            this.fromSerialized = fromSerialized;
            this.connector = new Connector();

            // Initial numToAlgorithm
            numToAlgorithm[Algorithm.BFS] = new BFSSearcher<Position>();
            numToAlgorithm[Algorithm.DFS] = new DFSSearcher<Position>();
        }

        /// <summary>
        /// Generate a new game, based on the generate command.
        /// </summary>
        /// <param name="name">
        /// The name of the game that is going to be generated.
        /// </param>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="cols">
        /// The number of columns.
        /// </param>
        /// <returns>
        /// The SearchGame that will be generated.
        /// </returns>
        public ISearchGame GenerateNewGame(string name, int rows, int cols)
        {
            //// TODO check this works
            ///*
            // * we don't need to save the game in the dictionary because it's a single player game and
            // * doesn't has to be known by the sever. So when we write the client we'll want/need to
            // * make sure to do a mapping between inner names and their serialization and send only the
            // * serialization when the server is needed (for a solution for example).
            // * see "GetGame" for the only dependecy on/use of the serialization for single player.
            // */
            //return generator.GenerateSearchGame(name, rows, cols);

            // TODO - also effected the code of GenerateMazeCommand.
            // No need to know the name of the single player game, but while the
            // client application is stupid will know it. Afterward we can replace it with the coe above
            ISearchGame game = generator.GenerateSearchGame(name, rows, cols);
            connector.AddGame(game);
            return game;
        }


        /// <summary>
        /// Get a specific game by it's name.
        /// </summary>
        /// <exception cref="KeyNotFoundException">
        /// In case there isn't a game with this name there is no key of it's name in the dictionary.
        /// </exception>
        /// <param name="str">
        /// The game's name.
        /// </param>
        /// <returns>
        /// The SearchGame which it's name is the input string.
        /// </returns>
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

        /// <summary>
        /// Compute the solution of the SearchGame with the given name by using
        /// the algorithm in the input.
        /// </summary>
        /// <param name="name">
        /// The name of the SearchGame that will be solved.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm that we will use.
        /// </param>
        /// <returns>
        /// The solution that was computed.
        /// </returns>
        public Solution<Position> ComputeSolution(string name, Algorithm algorithm)
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

        /// <summary>
        /// Start the game with the given name,number of rows,number of columns and creator.
        /// </summary>
        /// <param name="name">
        /// The name of the game.
        /// </param>
        /// <param name="rows">
        /// Number of rows.
        /// </param>
        /// <param name="cols">
        /// Number of columns.
        /// </param>
        /// <param name="creator">
        /// The creator of this game.
        /// </param>
        /// <returns>
        /// The SearchGame that was started.
        /// </returns>
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
            Player p = connector.GetPlayer(creator);
            ISearchGame g1 = connector.GetGame(p), g = null;
            if (!ReferenceEquals(null, g1))
            {
                toCreate = g1.HasEnded() || (g1.NumOfPlayer == 1 && g1.GetPlayers().Contains(p));
            }
            if (connector.ContainsGame(name))
             {
                g = connector.GetGame(name);
                /* g.NumOfPlayer > 0 always because the server know only multiplayer games
                replace the existing game if it has ended or the only player in it in the 
                one who asks to replace it.*/
                toCreate &= (g.HasEnded() || (g.NumOfPlayer == 1 && g.GetPlayers().Contains(p)));
            }
            if (toCreate)
            {
                // delete exitsing games
                connector.DeleteGame(g);
                try
                {
                    connector.DeleteGame(g1);
                } catch(Exception)
                {
                    //The game with this name was deleted
                }
                // Create a game with this name.
                ISearchGame game = GenerateNewGame(name, rows, cols);
                connector.AddGame(game);
                connector.AddClientToGame(creator, game);
                game.AddPlayer(connector.GetPlayer(creator));
                return game;
            }
            return null;
        }

        /// <summary>
        /// Get the list of all the games that clients can join.
        /// </summary>
        /// <returns>
        /// The list of the names of the games that we can join.
        /// </returns>
        public List<string> GetJoinableGamesList()
        {
            IEnumerable<ISearchGame> values = connector.Games;
            List<string> availableGames = new List<string>(values.Count());
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

        /// <summary>
        /// Let the given player join the game with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the game that the player wishes to join.
        /// </param>
        /// <param name="player">
        /// The player that sent the Join command.
        /// </param>
        /// <returns>
        /// True if he joined the game, false otherwise.
        /// </returns>
        public bool Join(string name, IClient player)
        {
            Player myPlayer = connector.GetPlayer(player);
            ISearchGame game = connector.GetGame(name);
            // if the player isn't a part of an existing game and the game allows a player to join
            if (ReferenceEquals(null, connector.GetGame(myPlayer)) && game.CanAPlayerJoin())
            {
                connector.AddClientToGame(myPlayer, name);
                return game.AddPlayer(myPlayer);
            } /*else if (!ReferenceEquals(null,connector.GetGame(myPlayer))) {
                player.SendResponse(@"Deleted game named: " + connector.GetGame(myPlayer).Name
                    + " to generate game: " + name);
                connector.DeleteGame(connector.GetGame(myPlayer));
                connector.AddClientToGame(myPlayer, name);
                return game.AddPlayer(myPlayer);
            }*/
            return false;
        }

        /// <summary>
        /// Delete a game from the connector.
        /// </summary>
        /// <param name="game">
        /// Game that will be deleted.
        /// </param>
        public void DeleteGame(ISearchGame game)
        {
            connector.DeleteGame(game);
        }
        /// <summary>
        /// Set "name" to the game name and returns the list of players
        /// Moves the client and returns the game where the player was moved
        /// </summary>
        /// <param name="move">
        /// The direction of movement.
        /// </param>
        /// <param name="player">
        /// The player that sent the Play command.
        /// </param>
        /// <return>
        /// True if the direction is legal,false otherwise.
        /// </return>
        public bool Play(Direction move, IClient player,string isExit)
        {
            Player p = connector.GetPlayer(player);
            ISearchGame game = connector.GetGame(p);
            return game.MovePlayer(p, move, isExit);
        }

        /// <summary>
        /// Get the player of the given IClient.
        /// </summary>
        /// <param name="player">
        /// The IClient that we are looking for his player.
        /// </param>
        /// <returns>
        /// Returns the player of this IClient by using the connector.
        /// </returns>
        public Player GetPlayer(IClient player)
        {
            return connector.GetPlayer(player);
        }

        /// <summary>
        /// Get the game of the given IClient by using the connector.
        /// </summary>
        /// <param name="player">
        /// The player that we are looking for his game.
        /// </param>
        /// <returns>
        /// The player's game.
        /// </returns>
        public ISearchGame GetGameOf(IClient player)
        {
            return connector.GetGame(player);
        }

        /// <summary>
        /// Get the game of the given player by using the connector.
        /// </summary>
        /// <param name="player">
        /// The player that we are looking for his game.
        /// </param>
        /// <returns>
        /// This player's game.
        /// </returns>
        public ISearchGame GetGameOf(Player player)
        {
            return connector.GetGame(player);
        }

        /// <summary>
        ///Remove unnecerssay games from the dictionary (games that have ended,
        // games that weren't active for very long time).
        /// </summary>
        public void Cleanup()
        {
            IEnumerable<ISearchGame> games = connector.Games;
            foreach (ISearchGame g in games)
            {
                if (g.HasEnded())
                {
                    connector.DeleteGame(g);
                }
            }
        }
        /// <summary>
        /// Close the game with the given name.
        /// </summary>
        /// <param name="name">
        /// The name of the game that will be closed.
        /// </param>
        /// <param name="player">
        /// The client that sent the Close command.
        /// </param>
        public void Close(string name, IClient player)
        {
            GetGameByName(name)?.Close(connector.GetPlayer(player));
            connector.DeleteGame(name);
        }

        /// <summary>
        /// Remove a client from all of the data in the connector.
        /// </summary>
        /// <param name="client">
        /// The client that we wish to remove.
        /// </param>
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
