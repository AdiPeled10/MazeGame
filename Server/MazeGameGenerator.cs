using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeGeneratorLib;
using MazeLib;

namespace Server
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
