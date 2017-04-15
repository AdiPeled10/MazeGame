using MazeGeneratorLib;

namespace SearchGames
{
    public class MazeGameGenerator : ISearchGameGenerator
    {
        private IMazeGenerator generator;

        public MazeGameGenerator(IMazeGenerator generator)
        {
            this.generator = generator;
        }

        public ISearchGame GenerateSearchGame(string name, int rows, int cols)
        {
            return new MazeGame(name, generator.Generate(rows, cols));
        }
    }
}
