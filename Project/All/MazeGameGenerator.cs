using MazeGeneratorLib;

namespace All
{
    /// <summary>
    /// This is the generator for the MazeGame.
    /// </summary>
    public class MazeGameGenerator : ISearchGameGenerator
    {
        /// <summary>
        /// The generator which we will use to generate different mazes.
        /// </summary>
        private IMazeGenerator generator;

        /// <summary>
        /// Constructor for the MazeGameGenerator.
        /// </summary>
        /// <param name="generator">
        /// The generator we will use.
        /// </param>
        public MazeGameGenerator(IMazeGenerator generator)
        {
            this.generator = generator;
        }

        /// <summary>
        /// Generate a maze based on the IMazeGenerator that we got in
        /// the constructor.
        /// </summary>
        /// <param name="name">
        /// The name of the maze.
        /// </param>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="cols">
        /// The number of columns.
        /// </param>
        /// <returns>
        /// Returns the MazeGame that was created.
        /// </returns>
        public ISearchGame GenerateSearchGame(string name, int rows, int cols)
        {
            return new MazeGame(name, generator.Generate(rows, cols));
        }
    }
}
