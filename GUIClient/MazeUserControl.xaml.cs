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


namespace GUIClient
{

    public delegate void KeyHandler(Key key);
    /// <summary>
    /// Interaction logic for MazeUserControl.xaml
    /// </summary>
    public partial class MazeUserControl : UserControl
    {
        // private Grid grid;
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

        private Dictionary<Key,KeyHandler> keyToHandler;


        /// <summary>
        /// Hashset of all locations which are illegal, we will use this in the KeyDown
        /// event, in a single player game we don't want to communicate with the server
        /// instead we will shut down the communication once we will get the maze and it's
        /// solution,the communication will hold only in the case of a multiplayer.
        /// We chose to give this functionality to the view because the ViewModel has a role
        /// to react to changes in the model but in a single player game we want to save the
        /// overhead of letting the socket remain open when we don't need the server.
        /// We use hashset because it is much faster than a list.
        /// </summary>
        //private MazeLogic logic;

        /// <summary>
        /// Width of every rectangle of the maze,this will help us in the keydown event
        /// to calculate the players location after a horizontal movement.
        /// </summary>
        private double widthPerRect;

        /// <summary>
        /// Height of every rectangle of the maze,this will help us in the keydown event
        /// to calculate the players location after a vertical movement.
        /// </summary>
        private double heightPerRect;

        private MazeBoard board;

        /// <summary>
        /// Save ImageSource,that way we won't need to load the image again and again;
        /// </summary>
        private ImageSource playerLogo;

        public ImageSource PlayerLogo
        {
            get { return playerLogo; }
        }


        public string Maze
        {
            get { return maze; }
            set { maze = value; }
        }

        public int Rows
        {
            get { return rows; }
            set
            {
                rows = value;
            }
        }

        public int Cols
        {
            get { return cols; }
            set { cols = value; }
        }

   

     

        

        /// <summary>
        /// This is here to help us test the design when the MVVM
        /// will be ready we will get the maze from the ViewModel.
        /// </summary>
        private void GenerateRandomMaze()
        {


            //For now generate the string for the maze here to test the design.
            //Later we will get it through the ViewModel.
            int loopVal = Rows * Cols;
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

        public MazeUserControl()
        {
      
        }

        public void AddKey(Key key,KeyHandler handler)
        {
            keyToHandler.Add(key, handler);
        }

        public void RemoveKey(Key key)
        {
            keyToHandler.Remove(key);
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            //Set our board.
            board = new MazeBoard(myCanvas);
            //Set default keyboards of dictionary.Initialize dictionary.
            keyToHandler = new Dictionary<Key, KeyHandler>();
            keyToHandler.Add(Key.Up, board.DrawPlayer);
            keyToHandler.Add(Key.Down, board.DrawPlayer);
            keyToHandler.Add(Key.Left, board.DrawPlayer);
            keyToHandler.Add(Key.Right, board.DrawPlayer);

            //Generate some random maze.
            GenerateRandomMaze();
            board.DrawOnCanvas(maze,rows,cols);
            base.OnInitialized(e);
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                keyToHandler[e.Key](e.Key);
            } catch (KeyNotFoundException ) {
                Console.Write("Oh snap");
            }
            /*Location newLocation;
            double xLimit = myCanvas.Width - widthPerRect;
            double yLimit = myCanvas.Height - heightPerRect;
             switch (e.Key)
             {
                 case Key.Up: {
                        //Check if this location is llegal.
                        newLocation = new Location(logic.PlayerLocation.X,
                           logic.PlayerLocation.Y - heightPerRect);
                         if (!logic.IsIllegal(newLocation,xLimit,yLimit)) {
                            DrawPlayer(newLocation);
                         }
                         break;
                     }

                 case Key.Down:
                    {
                        newLocation = new Location(logic.PlayerLocation.X,
                            logic.PlayerLocation.Y + heightPerRect);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
                            DrawPlayer(newLocation);
                        }
                        break;
                    }

                 case Key.Left:
                    {
                        newLocation = new Location(logic.PlayerLocation.X - widthPerRect,
                            logic.PlayerLocation.Y);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
                            DrawPlayer(newLocation);
                        }
                        break;
                    }

                 case Key.Right:
                    {
                        newLocation = new Location(logic.PlayerLocation.X + widthPerRect,
                            logic.PlayerLocation.Y);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
                            DrawPlayer(newLocation);
                        }
                        break;
                    }

                 default: { break; }
             }
             */
        }

       
        private void MazeLoaded(object sender,RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Border_KeyDown;
        }
    }
}