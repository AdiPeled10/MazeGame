using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ViewModel;
using System.Diagnostics;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerWindow.xaml
    /// </summary>
    public partial class MultiPlayerWindow : Window
    {
        /// <summary>
        /// The current maze name.
        /// </summary>
        private string mazeName;

        /// <summary>
        /// The current maze number of rows.
        /// </summary>
        private int rows;

        /// <summary>
        /// The current maze number of columns.
        /// </summary>
        private int cols;

        /// <summary>
        /// View model of the multi player game.
        /// </summary>
        private MultiPlayerVM vm;

        /// <summary>
        ///  True if we can connect to the server false otherwise.
        /// </summary>
        private bool connected = true;

        /// <summary>
        /// String that represents name of the maze.
        /// </summary>
        public string MazeName
        {
            get { return mazeName; }
            set
            {
                if (mazeName != value)
                {
                    mazeName = value;
                    
                }
            }
        }

        /// <summary>
        /// Rows property.
        /// Get or set "rows" member.
        /// </summary>
        public int Rows
        {
            get { return rows; }
            set
            {
                if (rows != value)
                {
                    rows = value;
                }
            }
        }

        /// <summary>
        /// Cols property.
        /// Get or set "cols" member.
        /// </summary>
        public int Cols
        {
            get { return cols; }
            set
            {
                if (cols != value)
                {
                    cols = value;
                }
            }
        }

        /// <summary>
        /// Constructor of multi player window.
        /// </summary>
        public MultiPlayerWindow()
        {
            InitializeComponent();
            vm = new MultiPlayerVM();
            vm.NotifyConnection += OpponentConnected;
            vm.CantFindServer += () =>
            {
                CantConnect win = new CantConnect();
                win.Show();
                connected = false;
            };
            DataContext = vm;
            vm.Connect();
        }

        /// <summary>
        /// Event which will occur when  start game button will be clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            //Start game via the view model.
            int cols, rows;
            if (!connected)
            {
                //Cant connect to server.
                return;
            }

            if (maze.NameBox.Length == 0)
            {
                //We will require the user to enter a name.
                grid.Children.Add(new TextBlock { Text = "Please enter a valid name.",
                    Foreground = System.Windows.Media.Brushes.Red });
                return;
            }

            //Check if rows and cols were entered otherwise take default
            if (maze.Rows > 0)
            {
                rows = maze.Rows;
            }
            else
            {
                //Read from default.
                rows = Properties.Settings.Default.MazeRows;
            }
            if (maze.Cols > 0)
            {
                cols = maze.Cols;
            }
            else
            {
                //Read from default.
                cols = Properties.Settings.Default.MazeCols;
            }

            waitingBlock.Text = "Waiting for opponent...";
            vm.GenerateMaze(maze.NameBox, rows, cols);
        }

        /// <summary>
        /// Opens a window that shows the game.
        /// </summary>
        public void OpponentConnected()
        {
            Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                     new Action(() => {
                         MultiPlayerMaze mazeWindow = new MultiPlayerMaze()
                         {
                             //Pass on the view model.
                             VM = vm
                         };
                         mazeWindow.Show();
                         this.Close();
                     }));
        }

        /// <summary>
        /// Joins the selected joinable multi player game.
        /// </summary>
        private void JoinClick(object sender, RoutedEventArgs e)
        {
            if (comboBox.Text.Length > 0)
                vm.JoinGame(comboBox.Text);
        }

        /// <summary>
        /// Each time drop down is clicked ask for list from server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropDownHandler(object sender,EventArgs e)
        {
            vm.ListCommand();
        }

        /// <summary>
        /// Check if window was closed through the code with Close method or from
        /// UI if it's from the code it's fine otherwise opponent left the game,send
        /// correct message to server.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed)
            {
                // Closed some other way.Send exit.
                vm.Disconnected();
            }
            base.OnClosing(e);
        }
    }
}
