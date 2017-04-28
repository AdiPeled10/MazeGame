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

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerWindow.xaml
    /// </summary>
    public partial class SinglePlayerWindow : Window
    {
        private TextBlock errorBox;
        
        public SinglePlayerWindow()
        {
            InitializeComponent();

        }

        /// <summary>
        /// This is the function that will run when the user will click the OK button in this window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Delete previous error.
            if (errorBox != null )
            {
                stackPanel.Children.Remove(errorBox);
            }

            string error = "";
            if (mazeInfo.NameBox.Length == 0)
            {
                error = "Please enter a valid name.";
            } else if (mazeInfo.Rows == 0)
            {
                error = "Please enter valid number of rows.";
            } else if (mazeInfo.Cols == 0)
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
            popupMaze.Rows = mazeInfo.Rows;
            popupMaze.Cols = mazeInfo.Cols;
            popupMaze.Name = mazeInfo.NameBox;
            popupMaze.Show();
        }
    }
}
