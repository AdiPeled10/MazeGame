
namespace Model
{
    /// <summary>
    /// The interface for any setting class. It gives default values for many
    /// requeired values for the application to work.
    /// </summary>
    public interface ISettingsModel
    {
        /// <summary>
        /// The server IP address.
        /// </summary>
        string ServerIP { get; set; }

        /// <summary>
        /// The server port number.
        /// </summary>
        int ServerPort { get; set; }

        /// <summary>
        /// The default number of rows in a new maze.
        /// </summary>
        int MazeRows { get; set; }

        /// <summary>
        /// The default number of cols in a new maze.
        /// </summary>
        int MazeCols { get; set; }

        /// <summary>
        /// The default search algorithm when solving a maze.
        /// </summary>
        int SearchAlgorithm { get; set; }

        /// <summary>
        /// A method to save the current setting for this run and the next runs.
        /// </summary>
        void SaveSettings();
    }
}
