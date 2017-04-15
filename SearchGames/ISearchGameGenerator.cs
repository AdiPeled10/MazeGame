namespace SearchGames
{
    public interface ISearchGameGenerator
    {
        ISearchGame GenerateSearchGame(string name, int rows, int cols);
    }
}
