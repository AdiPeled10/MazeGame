using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class GameInfo
    {
        public WebMaze Maze { get; set; }
        public string FirstClient { get; set; }
        public string SecondClient { get; set; }
        public string FirstUsername { get; set; }
        public string SecondUsername { get; set; }

        public string GetOpponent(string id)
        {
            if (FirstClient == id)
            {
                return SecondClient;
            } else if (SecondClient == id)
            {
                return FirstClient;
            } else
            {
                return null;
            }
        }


    }
}