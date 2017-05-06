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
    public delegate void KeyHandler(Key key);

    public class MazeViewModel : IViewModel,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The number of rows in the maze.
        /// </summary>
        private int rows;

        /// <summary>
        /// The number of columns in the maze.
        /// </summary>
        private int cols;

        /// <summary>
        /// String representation of the maze.
        /// </summary>
        private string maze;

        private Dictionary<Key, KeyHandler> keyToHandler;

        public string Maze
        {
            get { return maze; }
            set { maze = value; }
        }

        /// <summary>
        /// TODO- Maybe change input to user control instead of canvas-flexibility.
        /// </summary>
        /// <param name="control"></param>
        public MazeViewModel(int rows, int cols,MazeBoard board)
        {
            this.cols = cols;
            this.rows = rows;
            GenerateRandomMaze();
           
            //Set default keyboards of dictionary.Initialize dictionary.
            keyToHandler = new Dictionary<Key, KeyHandler>();
            keyToHandler.Add(Key.Up, board.DrawPlayer);
            keyToHandler.Add(Key.Down, board.DrawPlayer);
            keyToHandler.Add(Key.Left, board.DrawPlayer);
            keyToHandler.Add(Key.Right, board.DrawPlayer);
        }

        public void AddKey(Key key, KeyHandler handler)
        {
            keyToHandler.Add(key, handler);
        }

        public void RemoveKey(Key key)
        {
            keyToHandler.Remove(key);
        }

        public void KeyPressed(Key key)
        {
            try
            {
                keyToHandler[key](key);
            }
            catch (KeyNotFoundException)
            {
                Console.Write("Oh snap");
            }

        }


        /// <summary>
        /// This is here to help us test the design when the MVVM
        /// will be ready we will get the maze from the ViewModel.
        /// TODO- Will be gone when we will get from server.
        /// </summary>
        private void GenerateRandomMaze()
        {


            //For now generate the string for the maze here to test the design.
            //Later we will get it through the ViewModel.
            int loopVal = rows * cols;
            string myString = "";
            Random rand = new Random();
            int randomNumber = rand.Next(0, loopVal);
            for (int i = 0; i < loopVal; i++)
            {
                if (i != randomNumber)
                {
                    if (rand.Next(0, 2) == 0)
                    {
                        myString += '0';
                    }
                    else
                    {
                        myString += '1';
                    }
                }
                else
                {
                    myString += '*';
                }

            }
            maze = myString;
        }
   
    }
}
