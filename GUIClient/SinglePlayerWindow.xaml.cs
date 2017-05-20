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
using System.Windows.Shapes;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerWindow.xaml
    /// </summary>
    public partial class SinglePlayerWindow : Window
    {
        private TextBlock errorBox;

        ///private SinglePlayerVM singleVM;

        public TextBlock ErrorBox
        {
            get { return errorBox; }
            set { errorBox = value; }
        }
        
        public MazeInformationLayout Info
        {
            get { return mazeInfo; }
            set { mazeInfo = value; }
        }

        public StackPanel Stack
        {
            get { return stackPanel; }
        }

        public string MazeName
        {
            get { return mazeInfo.nameTextBox.Text; }
            set { }
        }

        public SinglePlayerWindow()
        {
            InitializeComponent();
           // singleVM = new SinglePlayerVM();
        }

        /// <summary>
        /// This is the function that will run when the user will click the OK button in this window.
        /// We don't want the view model to know about wpf at all.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SinglePlayerOkButton();
        }

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
