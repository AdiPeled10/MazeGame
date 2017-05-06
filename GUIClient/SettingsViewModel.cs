using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace ViewModel
{
    public class SettingsViewModel : WindowViewModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int serverPort;

        private int mazeCols;

        private int mazeRows;

        public int MazeCols
        {
            get { return mazeCols; }
            set { mazeCols = value; }
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
    }
}
