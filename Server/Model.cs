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
    class Model : IModel
    {
        Dictionary<int, ISearcher<Position>> numToAlgorithm;
        Dictionary<string, ISearchGame> nameToGame;
        SolutionCache cache;
        IMazeGenerator generator;

        public Model(IMazeGenerator generator)
        {
            this.generator = generator;
            cache = new SolutionCache();
        }

        public Maze GenerateMaze(string name, int rows, int cols)
        {
            Maze myMaze = generator.Generate(rows, cols);
            //Add the new maze to the dictionary.
            nameToGame[name] = new MazeGame(name, myMaze);
            return myMaze;
        }

        public Solution<Position> ComputeSolution(string name, int algorithm)
        {
            if (cache.IsSolved(name))
            {
                //Maze already solved,get solution from cache.
                return cache.GetSolution(name);
            }
            try
            {
                ISearcher<Position> searcher = numToAlgorithm[algorithm];
                ISearchGame game = nameToGame[name];
                Solution<Position> solution = searcher.Search(game.AsSearchable());
                //Add solution to the cache.
                cache.AddSolution(name, solution);
                return solution;
            }
            catch (KeyNotFoundException exp)
            {
                //Didn't find the key given in name.
                return null;
            }

        }

        public void StartGame(string name, int rows, int cols)
        {
            if (nameToGame.Keys.Contains(name))
            {
                //There is an existing game,for now we will assume there aren't games with same name.
                //Start game.
                nameToGame[name].Start();
            }
            else
            {
                //Create a game with this name.
                Maze maze = generator.Generate(rows, cols);
                nameToGame[name] = new MazeGame(name, maze);
                //We will complete method start of Game later.
                nameToGame[name].Start();
            }
        }

        public List<string> GetList()
        {
            List<string> availableGames = new List<string>();
            foreach(ISearchGame game in nameToGame.Values)
            {
                if (!game.Started())
                {
                    //Game hasn't started yet.
                    availableGames.Add(game.GetName());
                }
            }
            return availableGames;
        }

        public void Join(string name,Player player)
        {
            //TODO- Check if the game is already full and stuff like that.
            nameToGame[name].AddPlayer(player);
        }

        public void Play(Direction move,Player player)
        {
            //TODO- Check about this cast.
            ISearchGame game = (ISearchGame)nameToGame.Values.Where(elem => elem.IsPlaying(player) == true);
            game.MovePlayer(player, move);
        }

        public void Close(string name)
        {
            //Close the game.
            nameToGame[name].Close();
            //Remove this game from out dictionary.
            nameToGame.Remove(name);
        }

        
    }
}
