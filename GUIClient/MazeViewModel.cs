using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;

namespace ViewModel
{
    /// <summary>
    /// Delegate that is activated when a key is pressed.
    /// </summary>
    /// <param name="key"> Key object </param>
    public delegate void KeyHandler(Key key);

    public class MazeViewModel : ViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// A dictionary that connects a key to his handler.
        /// </summary>
        private Dictionary<Key, KeyHandler> keyToHandler;

        /// <summary>
        /// Constructor.
        /// Initialize "keyToHandler" member.
        /// </summary>
        /// <param name="board"> MazeBoard object. </param>
        public MazeViewModel(MazeBoard board)
        {
            //Set default keyboards of dictionary.Initialize dictionary.
            keyToHandler = new Dictionary<Key, KeyHandler>()
            {
                { Key.Up, board.DrawPlayer },
                { Key.Down, board.DrawPlayer },
                { Key.Left, board.DrawPlayer },
                { Key.Right, board.DrawPlayer }
            };
        }

        /// <summary>
        /// Adds a handler to the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        public void AddKey(Key key, KeyHandler handler)
        {
            keyToHandler.Add(key, handler);
        }

        /// <summary>
        /// Removes "key" from "keyToHandler".
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(Key key)
        {
            keyToHandler.Remove(key);
        }

        /// <summary>
        /// Calls the handler of "key" as set in "keyToHandler".
        /// </summary>
        /// <param name="key"></param>
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
