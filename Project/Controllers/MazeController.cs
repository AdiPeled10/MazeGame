using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using MazeLib;
using MazeGeneratorLib;
using SearchAlgorithmsLib;
using All;
using System.Threading.Tasks;

namespace Project.Controllers
{
    
    public class MazeController : ApiController
    {
        private static List<WebMaze> mazes = new List<WebMaze>();
        private static Model model = new Model(new MazeGameGenerator(new DFSMazeGenerator()));
        private static Dictionary<string, GameInfo> clientToGameInfo = new
            Dictionary<string, GameInfo>();
        private static List<GameInfo> gameInformation = new List<GameInfo>();
        private Dictionary<string, Algorithm> numToAlgo = new Dictionary<string, Algorithm>()
        {
            { "BFS",Algorithm.BFS },
            {"DFS",Algorithm.DFS }
        };

        [HttpGet]
        public IHttpActionResult GetMaze(string name,int rows,int cols)
        {
            MazeGame game = (MazeGame)model.GenerateNewGame(name, rows, cols);
            WebMaze newMaze = new WebMaze();
            newMaze.SetMaze(game.maze);
            //Add to list.
            mazes.Add(newMaze);
            return Ok(newMaze);
        }

        [HttpGet]
        public IHttpActionResult GetSolve(string name, string algo)
        {
            try
            {
                Solution<Position> solution = model.ComputeSolution(name, numToAlgo[algo]);
                //mazes.Add(newMaze);
                JObject solveObj = new JObject
                {
                    ["Name"] = name,
                    ["Solution"] = Converter.ToJSON(solution),
                    ["NodesEvaluated"] = solution.NodesEvaluated
                };
                return Ok(solveObj);
            } catch (KeyNotFoundException exp)
            {
                return NotFound();
            }

        }

        public static void StartGame(string clientId, string name, int rows, int cols, string username)
        {
            //Generate new game.
            GameInfo info = new GameInfo();
            MazeGame game = (MazeGame)model.GenerateNewGame(name, rows, cols);
            WebMaze myMaze = new WebMaze();
            //Set the maze.
            myMaze.SetMaze(game.maze);
            //Save info in GameInfo
            info.Maze = myMaze;
            info.FirstClient = clientId;
            info.FirstUsername = username;
            //Add to list.
            gameInformation.Add(info);
            clientToGameInfo[clientId] = info;
        }

        public static GameInfo JoinGame(string clientId, string name, string username)
        {
            //Find entry in list which contains name of game
            GameInfo info = gameInformation.Find(x => x.Maze.Name == name);
            info.SecondUsername = username;
            //Assign to dictionary.
            clientToGameInfo[clientId] = info;
            //Set info for second client.
            info.SecondClient = clientId;

            //Now return the maze that will be sent to both clients from the hub.
            return info;
        }

        public static string GetOpponent(string clientId)
        {
            try
            {
                return clientToGameInfo[clientId].GetOpponent(clientId);
            } catch(KeyNotFoundException exp)
            {
                return null;
            }
        }

        [Route("api/Maze/gamelist")]
        [HttpGet]
        public IHttpActionResult GetListOfAvailableGames()
        {
            List<string> lst = new List<string>();
            foreach (GameInfo game in gameInformation)
            {
                if (ReferenceEquals(null, game.SecondClient))
                {
                    lst.Add(game.Maze.Name);
                }
            }
            return Ok(lst);
        }

        public static async Task GameEnded(string clientId)
        {
            GameInfo game;
            try
            {
                game = clientToGameInfo[clientId];
            }
            catch
            {
                return;  // someone tried to cheat some extra wins
            }
            // get users from database
            UsersController db = new UsersController();
            User winner, looser;
            if (clientId == game.FirstClient)
            {
                winner = db.GetUser(game.FirstUsername);
                looser = db.GetUser(game.SecondUsername);
            }
            else
            {
                looser = db.GetUser(game.FirstUsername);
                winner = db.GetUser(game.SecondUsername);
            }
            if (!ReferenceEquals(winner, null) && !ReferenceEquals(looser, null)
                && !ReferenceEquals(winner, looser))
            {
                // add ranking
                ++winner.Wins;
                ++looser.Loses;
                // update the database
                await db.UpdateUserAsync(winner);
                await db.UpdateUserAsync(looser);
                db.TearDown();
            }
        }

    }
}
