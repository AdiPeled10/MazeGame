namespace SearchGames
{
    /// <summary>
    /// This interface will help us decide which SearchGame we are going to generate during
    /// run time, that way we can use dynamic binding.
    /// </summary>
    public interface ISearchGameGenerator
    {
        /// <summary>
        /// Generate a search game,this is the main reason we use this interface in order to
        /// implement the Factory desing pattern.
        /// </summary>
        /// <param name="name">
        /// The name of the game that we are going to generate.
        /// </param>
        /// <param name="rows">
        /// The number of rows in the SearchGame.
        /// </param>
        /// <param name="cols">
        /// The number of columns in the SearchGame.
        /// </param>
        /// <returns>
        /// The SearchGame that we have generated.
        /// </returns>
        ISearchGame GenerateSearchGame(string name, int rows, int cols);
    }
}
