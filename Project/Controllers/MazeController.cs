using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Models;
using Newtonsoft.Json.Linq;
using MazeLib;
using MazeGeneratorLib;
using SearchAlgorithmsLib;
using All;

namespace Project.Controllers
{
    
    public class MazeController : ApiController
    {
        private static List<WebMaze> mazes = new List<WebMaze>();
        private static Model model = new Model(new MazeGameGenerator(new DFSMazeGenerator()));
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

       
    }
}
