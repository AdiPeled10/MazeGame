using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface ISearchGameGenerator
    {
        ISearchGame GenerateSearchGame(string name, int rows, int cols);
    }
}
