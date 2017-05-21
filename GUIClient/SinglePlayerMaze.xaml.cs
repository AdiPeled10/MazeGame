using System.Windows;
using System.ComponentModel;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerMaze.xaml
    /// </summary>
    public partial class SinglePlayerMaze : Window
    {
        /// <summary>
        /// The number of rows in the maze.
        /// </summary>
        private int rows;

        /// <summary>
        /// The number of cols in the maze.
        /// </summary>
        private int cols;

        /// <summary>
        /// The width of a rectangle on the screen.
        /// </summary>
        private int realWidth;

        /// <summary>
        /// The height of a rectangle on the screen.
        /// </summary>
        private int realHeight;

        /// <summary>
        /// The maze name.
        /// </summary>
        private string mazeName;

        /// <summary>
        /// Set to false if connection failed,true otherwise.
        /// </summary>
        private bool connected = true;

        /// <summary>
        /// The Model for this class.
        /// </summary>
        private SinglePlayerVM vm;

        /// <summary>
        /// VM property.
        /// Set the "vm" member.
        /// </summary>
        public SinglePlayerVM VM
        {
            set {
                vm = value;
                DataContext = vm;
                vm.CantFindServer += () =>
                {
                    CantConnect win = new CantConnect();
                    win.Show();
                    connected = false;
                };
                vm.Connect();
                
            }
        }

        /// <summary>
        /// Connected property.
        /// Get the "connected" member value.
        /// </summary>
        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// MazeName property.
        /// Get or set the "mazeName" member.
        /// </summary>
        public string MazeName
        {
            get { return mazeName; }
            set
            {
                if (mazeName != value)
                {
                    mazeName = value;
                    //NotifyPropertyChanged("MazeName");
                }
            }
        }

        /// <summary>
        /// RealWidth property.
        /// Get or set the "realWidth" member.
        /// </summary>
        public int RealWidth
        {
            get { return realWidth; }
            set
            {
                if (realWidth != value)
                {
                    realWidth = value;
                   // NotifyPropertyChanged("RealWidth");
                }
            }
        }

        /// <summary>
        /// RealHeight property.
        /// Get or set the "realHeight" member.
        /// </summary>
        public int RealHeight
        {
            get { return realHeight; }
            set
            {
                if (realHeight != value)
                {
                    realHeight = value;
                   // NotifyPropertyChanged("RealHeight");
                }
            }
        }

        /// <summary>
        /// Rows property.
        /// Get or set the "rows" member.
        /// also effects "RealHeight".
        /// </summary>
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                if  (rows != value)
                {
                    rows = value;
                    //this.Height = 30 * rows;
                    /*  double buttonHeight = Height / (rows + 1);
                      if (buttonHeight < 30)
                      {
                          restartButton.Height = buttonHeight;
                          solveButton.Height = buttonHeight;
                          mainMenu.Height = buttonHeight;
                      } else
                      {
                          restartButton.Height = 30;
                          solveButton.Height = 30;
                          mainMenu.Height = 30;
                      }*/
                    // stackPanel.Height = 4;
                    RealHeight = 30 * rows; 
                    //NotifyPropertyChanged("Rows");
                }
            }
        }

        /// <summary>
        /// Cols property.
        /// Get or set the "cols" member.
        /// also effects "RealWidth".
        /// </summary>
        public int Cols
        {
            get
            {
                return cols;
            }

            set
            {
                if (cols != value)
                {
                    cols = value;
                    // this.Width = 30 * Cols;
                    // stackPanel.Width = Width / (Cols + 1);
                    RealWidth = 30 * cols;
                   // NotifyPropertyChanged("Cols");
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SinglePlayerMaze()
        {
            InitializeComponent();
            maze.Done += GameDone;
        }

        ///// <summary>
        ///// Event to notify 
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        //public void NotifyPropertyChanged(string propName)
        //{
        //    if (this.PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //}

        /// <summary>
        /// Calls vm.GenerateMaze(MazeName, Rows, Cols);
        /// </summary>
        public void Generate()
        {
            vm.GenerateMaze(MazeName, Rows, Cols);
        }

        /// <summary>
        /// Closes this window.
        /// </summary>
        public void GameDone()
        {
            this.Close();
        }

        /// <summary>
        /// Calls vm.AskForSolution(vm.MazeName);
        /// </summary>
        public void GetSolution()
        {
            //int start_x = (int)vm.StartLocation.X, start_y = (int)vm.StartLocation.Y;
            //int end_x = (int)vm.EndLocation.X, end_y = (int)vm.EndLocation.Y;
            //vm.AskForSolution(rows + "," + cols + "," + start_x + "," + start_y + "," +
            //    end_x + "," + end_y + "," + vm.MazeString);
            vm.AskForSolution(vm.MazeName);
        }

        /// <summary>
        /// Calls vm.CloseConnection();
        /// </summary>
        public void CloseConnection()
        {
            vm.CloseConnection();
        }

        /// <summary>
        /// Opens a mudali window that asks the user if his sure and offers two
        /// choice buttons. One will close this window and open a MainWindow intance,
        /// the other closes the mudali window.
        /// </summary>
        /// <param name="sender"> Irrelevant. </param>
        /// <param name="e"> Irrelevant. </param>
        private void MainMenuClick(object sender, RoutedEventArgs e)
        {
            MudalWindow win = new MudalWindow()
            {
                DataContext = maze
            };
            maze.MudalMessage = "Are you sure?";
            maze.MudalFirstButton = "Continue";
            maze.MudalSecondButton = "Cancel";
            win.OnFirstButton = new RoutedEventHandler((object send, RoutedEventArgs args) =>
            {
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
                win.Close();
            });

            //Don't do anything if cancel is chosen.
            win.OnSecondButton = new RoutedEventHandler((object send, RoutedEventArgs args) => {
                win.Close();
            });
            win.Show();
        }

        /// <summary>
        /// Will be triggered when the restart button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartButtonClick(object sender, RoutedEventArgs e)
        {
            maze.Restart();
        }

        /// <summary>
        /// Gets the solution to the maze(with the name "mazeName") and then calls
        /// "maze.Solve(solution)".
        /// </summary>
        /// <param name="sender"> Irrelevant. </param>
        /// <param name="e"> Irrelevant. </param>
        private void SolveClick(object sender,RoutedEventArgs e)
        {
            //Get solution from view model.
            string solution = vm.GetMazeSolution(MazeName);
            if (solution == null)
                return;
            //Notify the maze user control.
            maze.Solve(solution);
        }

    }
}
