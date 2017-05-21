using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerWindow.xaml
    /// </summary>
    public partial class SinglePlayerWindow : Window
    {
        /// <summary>
        /// A box that her job is to show errors if there are any.
        /// </summary>
        private TextBlock errorBox;

        /// <summary>
        /// ErrorBox property.
        /// Get or set the "errorBox" member.
        /// </summary>
        public TextBlock ErrorBox
        {
            get { return errorBox; }
            set { errorBox = value; }
        }

        /// <summary>
        /// Info property
        /// Get or set "mazeInfo" member
        /// </summary>
        public MazeInformationLayout Info
        {
            get { return mazeInfo; }
            set { mazeInfo = value; }
        }

        /// <summary>
        /// Stack property.
        /// Get "stackPanel" member
        /// </summary>
        public StackPanel Stack
        {
            get { return stackPanel; }
        }

        /// <summary>
        /// MazeName property.
        /// Get "mazeInfo.nameTextBox.Text" member
        /// </summary>
        public string MazeName
        {
            get { return mazeInfo.nameTextBox.Text; }
            set { }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SinglePlayerWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This is the function that will run when the user will click the OK button in this window.
        /// We don't want the view model to know about wpf at all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            SinglePlayerOkButton();
        }

        /// <summary>
        /// Generate a new maze.
        /// This function generate a new maze game and its solution with the given
        /// parameters in the window boxes or from the default setting(if the fitting
        /// box is empty). If the name box is empty an error message apears.
        /// </summary>
        public void SinglePlayerOkButton()
        {
            //Delete previous error.
            if (errorBox != null)
            {
                stackPanel.Children.Remove(errorBox);
            }

            string error = "";
            if (mazeInfo.NameBox.Length == 0)
            {
                //We will require the user to enter a name.
                error = "Please enter a valid name.";
            }
            if (error.Length != 0)
            {
                //There was some error.
                errorBox = new TextBlock { Text = error, Foreground = Brushes.Red };
                stackPanel.Children.Add(errorBox);
                return;
            }
            SinglePlayerMaze popupMaze = new SinglePlayerMaze();
            popupMaze.VM = new SinglePlayerVM();

            //Check if there was a disconnection.

            if (!popupMaze.Connected)
            {
                //There is a disconnection close and return.
                popupMaze.Close();
                return;
            }
            popupMaze.MazeName = mazeInfo.NameBox;
            
            //Check if rows and cols were entered otherwise take default
            if (mazeInfo.Rows > 0)
            {
                popupMaze.Rows = mazeInfo.Rows;
            } else
            {
                //Read from default.
                popupMaze.Rows = Properties.Settings.Default.MazeRows;
            }
            if (mazeInfo.Cols > 0)
            {
                popupMaze.Cols = mazeInfo.Cols;
            }
            else
            {
                //Read from default.
                popupMaze.Cols = Properties.Settings.Default.MazeCols;
            }

            popupMaze.Generate();
            popupMaze.GetSolution();
            //Close connection in single player.
            popupMaze.CloseConnection();
            popupMaze.Show();
            this.Close();
        }
    }
}
