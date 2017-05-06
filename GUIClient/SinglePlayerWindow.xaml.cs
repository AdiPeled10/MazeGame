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

        private SinglePlayerWindowVM vm;

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

        public SinglePlayerWindow()
        {
            vm = new SinglePlayerWindowVM();
            InitializeComponent();

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
                error = "Please enter a valid name.";
            }
            else if (mazeInfo.Rows == 0)
            {
                error = "Please enter valid number of rows.";
            }
            else if (mazeInfo.Cols == 0)
            {
                error = "Please enter a valid number of columns";
            }

            if (error.Length != 0)
            {
                //There was some error.
                errorBox = new TextBlock { Text = error, Foreground = Brushes.Red };
                stackPanel.Children.Add(errorBox);
                return;
            }
            SinglePlayerMaze popupMaze = new SinglePlayerMaze();
            popupMaze.MazeName = mazeInfo.NameBox;
            popupMaze.Rows = mazeInfo.Rows;
            popupMaze.Cols = mazeInfo.Cols;
            popupMaze.Show();
            this.Close();
        }
    }
}
