using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ViewModel;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerMaze.xaml
    /// </summary>
    public partial class MultiPlayerMaze : Window
    {
        /// <summary>
        /// Number of rows in the maze.
        /// </summary>
        private int rows;

        /// <summary>
        /// Number of cols in the maze.
        /// </summary>
        private int cols;

        /// <summary>
        /// The ViewModel. This class uses it as a "Model" .
        /// </summary>
        private MultiPlayerVM vm;

        /// <summary>
        /// VM property.
        /// Get or set "vm" member.
        /// </summary>
        public MultiPlayerVM VM
        {
            get { return vm; }
            set
            {
                vm = value;
                DataContext = vm;
                vm.Movement += otherMaze.MovePlayer;
                vm.NotifyDisconnection += OpponentDisconnected;
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
                rows = value;
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
                cols = value;
            }
        }

        /// <summary>
        /// Constructor.
        /// Calls "ListenToOpponentMoves" in the end.
        /// </summary>
        public MultiPlayerMaze()
        {
            InitializeComponent();
            myMaze.Done += GameDone;
            otherMaze.Done += GameDone;
            KeyDown += SendPlay; 
            ListenToOpponentMoves();
        }

        /// <summary>
        /// Shows on screen a message that says that the other player have left the game.
        /// This used when the other player disconnected while the game is still on.
        /// </summary>
        public void OpponentDisconnected()
        {
            Application.Current.Dispatcher.BeginInvoke(
                   DispatcherPriority.Background,
                    new Action(() => {
                        otherBox.Text = "Your opponent has left the game.";
                        otherBox.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Red);
                        }));
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
            vm.Disconnected();
            base.OnClosing(e);
            if (!wasCodeClosed)
            {
                // without the program doesn't really exit.
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Notify the view model that the game is over.
        /// </summary>
        public void GameDone()
        {
            vm.GameOver();
            this.Close();
        }

        /// <summary>
        /// Begins a new thread that reads the opponent moves and show them on the screen.
        /// </summary>
        public void ListenToOpponentMoves()
        {
            Thread thread = new Thread(() => { vm.ListenToOpponent(); });
            Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                     new Action(() => { thread.Start(); }));
        }

        /// <summary>
        /// Activated when user clicks back to main menu,we will send a message to the server
        /// that notifies that we need to notify the other player of disconnection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Notify that player got disconnected.
            vm.Disconnected();
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        /// <summary>
        /// Moves the player in the direction specified in "e.Key"(id it can).
        /// </summary>
        /// <param name="sender"> meaningless. </param>
        /// <param name="e"> e.Key is the direction to move toward. </param>
        private void SendPlay(object sender, KeyEventArgs e)
        {
            //Send to view model.
            string strKey = e.Key.ToString().ToLower();
            switch (strKey)
            {
                case "up":
                case "down":
                case "left":
                case "right":
                    {
                        vm.PlayInDirection(strKey);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
