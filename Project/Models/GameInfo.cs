
namespace Project.Models
{
    /// <summary>
    /// This class holds a multiplayer game information.
    /// </summary>
    public class GameInfo
    {
        public WebMaze Maze { get; set; }
        public string FirstClient { get; set; }
        public string SecondClient { get; set; }
        public string FirstUsername { get; set; }
        public string SecondUsername { get; set; }

        /// <summary>
        /// get the other player. If id isn't the id of this game players,
        /// null will be returned
        /// </summary>
        /// <param name="id"> string - id of a player </param>
        /// <returns> string - the id of the other player</returns>
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