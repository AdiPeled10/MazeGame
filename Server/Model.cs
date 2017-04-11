using System;
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
     * TODO
     * security breach: if a muliplayer game has a name of a single player game it will hide
     * the existance of the single player game forever.
     */
    public class Model : IModel // consider usesing template so different games will have different models. it will allow us to have 2 different games with the same name.
    {
        private Dictionary<int, ISearcher<Position>> numToAlgorithm;
        private Dictionary<string, ISearchGame> nameToGame;
        private SolutionCache cache;
        private ISearchGameGenerator generator;
        private FromSerialized fromSerialized;
        Connector connector; // TODO use this to get Player instead of getting Players as input
                             // or use it in commands to hide socket/clients from model (it may "demage"
                             // the controller, but not if will make the commands indepandece of the controller)

        // a model that doesn't support jsons
        public Model(ISearchGameGenerator generator)
        {
            numToAlgorithm = new Dictionary<int, ISearcher<Position>>();
            nameToGame = new Dictionary<string, ISearchGame>(); 
            //Save the generator.
            this.generator = generator;
            //Create a new cache for all the solutions.
            cache = new SolutionCache();
            fromSerialized = str => null;

            // initial numToAlgorithm
            numToAlgorithm[0] = new BFSSearcher<Position>();
            numToAlgorithm[1] = new DFSSearcher<Position>();
        }

        public Model(ISearchGameGenerator generator, FromSerialized fromSerialized)
        {
            numToAlgorithm = new Dictionary<int, ISearcher<Position>>();
            nameToGame = new Dictionary<string, ISearchGame>(); 
            this.generator = generator;
            cache = new SolutionCache();
            this.fromSerialized = fromSerialized;

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
                game = nameToGame[str];
            }
            catch (KeyNotFoundException)
            {
                //Didn't find the key given in name. Perhapse it's an single player game so the name is actually a json
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
                if (cache.IsSolved(name))
                {
                    //Maze already solved, get solution from cache.
                    return cache.GetSolution(name);
                }
                ISearcher<Position> searcher = numToAlgorithm[algorithm];
                Solution<Position> solution = searcher.Search(game.AsSearchable());
                //Add solution to the cache.
                cache.AddSolution(name, solution);
                return solution;
            }
            return null;
        }

        public void StartGame(string name, int rows, int cols, IClient creator)
        {
            /*
             * If the game already exists, the user didn't used this command as he should and won't be getting a game.
             * If we would join the client anyway, we will ignore his requst to "rows", "cols" and maybe he does't want
             * to play in a different size maze(it's like the difference between requesting a 100 pieces puzzle and
             * getting a 2000 pieces puzzle).
             */
            if (!nameToGame.ContainsKey(name) || nameToGame[name].NumOfPlayer == 0 || nameToGame[name].hasEnded())
            {
                // Create a game with this name.
                ISearchGame game = GenerateNewGame(name, rows, cols);
                connector.AddPlayerToGame(creator, nameToGame[name]);
            }
        }

        public List<string> GetJoinableGamesList()
        {
            List<string> availableGames = new List<string>();
            IEnumerable<ISearchGame> values = nameToGame.Values;
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
            connector.AddPlayerToGame(player, nameToGame[name]);
        }

        // TODO rewrite using connector or use connector at a command and just pass the game
        public void Play(Direction move, IClient player)
        {
            ISearchGame game = connector.GetGame(player);
            game.MovePlayer(connector.GetPlayer(player), move);
        }

        // remove unnecerssay games from the dictionary (games that have ended,
        // games that weren't active for very long time)
        public void Cleanup(string name)
        {
            foreach(KeyValuePair<string, ISearchGame> entry in nameToGame)
            {
                if (entry.Value.hasEnded())
                    nameToGame.Remove(entry.Key);
            }
        }

        public void Close(string name)
        {
            connector.DeleteGame(nameToGame[name]);
            nameToGame.Remove(name);
        }
    }
}
