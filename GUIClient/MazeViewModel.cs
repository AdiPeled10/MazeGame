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

    public class MazeViewModel : ViewModel, INotifyPropertyChanged
    {

        private Dictionary<Key, KeyHandler> keyToHandler;

        /// <summary>
        /// TODO- Maybe change input to user control instead of canvas-flexibility.
        /// </summary>
        /// <param name="control"></param>
        public MazeViewModel(MazeBoard board)
        {

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
                return;
            }

        }



    }
}
