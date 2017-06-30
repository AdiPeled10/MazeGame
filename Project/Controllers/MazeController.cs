using System.Collections.Generic;
using System.Web.Http;
using Project.Models;
using Newtonsoft.Json.Linq;
using MazeLib;
using MazeGeneratorLib;
using SearchAlgorithmsLib;
using All;
using System.Threading.Tasks;

namespace Project.Controllers
{
    /// <summary>
    /// Controller for handling maze related requests.
    /// </summary>
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

        /// <summary>
        /// Generates a new maze with name as its name, with rows rows and cols cols.
        /// </summary>
        /// <param name="name"> string </param>
        /// <param name="rows"> int </param>
        /// <param name="cols"> int </param>
        /// <returns> HTTP OK with the generated WebMaze </returns>
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

        /// <summary>
        /// Solve the maze with the given name using DFS or BFS algorithm(determined by algo argument).
        /// </summary>
        /// <param name="name"> string - name of existing single player maze </param>
        /// <param name="algo"> "BFS" or "DFS" - The algorithm to solve with the maze </param>
        /// <returns> HTTP Ok with the solution or NotFound if the maze wasn't found </returns>
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

        /// <summary>
        /// generate a new multi player game with the name name. This function adds the
        /// generating client to the game.
        /// </summary>
        /// <param name="clientId"> string - The requesting client id </param>
        /// <param name="name"> string - the generated maze name </param>
        /// <param name="rows"> int - the number of rows in the generated maze </param>
        /// <param name="cols"> int - the number of cols in the generated maze </param>
        /// <param name="username"> string - The requesting client username(he has to be signed in) </param>
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

        /// <summary>
        /// This function joins the client to the multiplayer game named name.
        /// </summary>
        /// <param name="clientId"> string - The requesting client id </param>
        /// <param name="name"> The existing multiplayer game </param>
        /// <param name="username"> string - The requesting client username(he has to be signed in) </param>
        /// <returns> GameInfo class object - the multiplayer game corresponding GameInfo </returns>
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

        /// <summary>
        /// returns the id of the client that the client with clientId plays against.
        /// </summary>
        /// <param name="clientId"> a client id </param>
        /// <returns> the id of the opponent </returns>
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

        /// <summary>
        /// returns the list of joinable multiplayer games.
        /// </summary>
        /// <returns> HTTP Ok with a list </returns>
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

        /// <summary>
        /// This function adds 1 to the number of winnings of the user of the client with
        /// clientId.
        /// This function adds 1 to the number of loses of the user that plays against the
        /// client with clientId.
        /// This function deletes their multiplayer game.
        /// </summary>
        /// <param name="clientId"> The winning client </param>
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
