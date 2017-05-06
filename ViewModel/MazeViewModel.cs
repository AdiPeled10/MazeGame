using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MazeViewModel : IViewModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int serverPort;

        private int mazeCols;

        private int mazeRows;

        private string mazeName;

        public string MazeName
        {
            get { return mazeName; }
            set { mazeName = value; }
        }

        public int MazeCols
        {
            get { return mazeCols; }
            set { mazeCols = value;  }
        }

         public int MazeRows
        {
            get { return mazeRows; }
            set { mazeRows = value; }
        }

        public int ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }

        public void OpenSinglePlayerMenu()
        {
            
        }

    }
}
