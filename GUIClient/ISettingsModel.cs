using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface ISettingsModel
    {
        string ServerIP { get; set; }
        int ServerPort { get; set; }
        int MazeRows { get; set; }
        int MazeCols { get; set; }
        int SearchAlgorithm { get; set; }

        void SaveSettings();
    }
}
