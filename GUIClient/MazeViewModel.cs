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
using GUIClient;

namespace ViewModel
{
    public class MazeViewModel : IViewModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int serverPort;

        private int mazeCols;

        private int mazeRows;

        private string mazeName;

        private MazeHandler handler;

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

        public MazeViewModel()
        {
        }

        public void OpenMenu(Window sender,Window opened)//Window window//)
        {
            opened.Show();
            sender.Close();
        }
        
        public void SinglePlayerOkButton(SinglePlayerWindow window)
        {
            //Delete previous error.
            if (window.ErrorBox != null)
            {
                window.Stack.Children.Remove(window.ErrorBox);
            }

            string error = "";
            if (window.Info.NameBox.Length == 0)
            {
                error = "Please enter a valid name.";
            }
            else if (window.Info.Rows == 0)
            {
                error = "Please enter valid number of rows.";
            }
            else if (window.Info.Cols == 0)
            {
                error = "Please enter a valid number of columns";
            }

            if (error.Length != 0)
            {
                //There was some error.
                window.ErrorBox = new TextBlock { Text = error, Foreground = Brushes.Red };
                window.Stack.Children.Add(window.ErrorBox);
                return;
            }
            SinglePlayerMaze popupMaze = new SinglePlayerMaze();
            popupMaze.MazeName = window.Info.NameBox;
            popupMaze.Rows = window.Info.Rows;
            popupMaze.Cols = window.Info.Cols;
            popupMaze.Show();
            window.Close();
        }

        public void RestartGame()
        {
            handler.Restart();
        }

        public void KeyPressed(Key key)
        {
            handler.KeyPressed(key);
        }

        public void SetHandler(Canvas canvas,int rows,int cols)
        {
            handler = new MazeHandler(canvas,rows,cols);
        }

        public void DrawMaze()
        {
            handler.DrawMaze();
        }

    }
}
