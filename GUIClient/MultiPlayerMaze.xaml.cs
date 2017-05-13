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
using System.Runtime.CompilerServices;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerMaze.xaml
    /// </summary>
    public partial class MultiPlayerMaze : Window
    {
        private int rows;
        private int cols;
        private MultiPlayerMazeVM vm;

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
            set
            {
                cols = value;
            }
        }

        public MultiPlayerMaze()
        {
            vm = new MultiPlayerMazeVM();
        }


        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            Grid mazes = new Grid();
            mazes.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            });

            mazes.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Auto)
            });

            mazes.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            mazes.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });

            TextBox myBox = new TextBox
            {
                Text = "My board",
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.Blue)
            };
            TextBox otherBox = new TextBox
            {
                Text = "Other player's board",
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.Green)
            };
            mazes.Children.Add(myBox);
            Grid.SetRow(myBox, 0);
            Grid.SetColumn(myBox, 0);

            mazes.Children.Add(otherBox);
            Grid.SetRow(otherBox, 0);
            Grid.SetColumn(otherBox, 1);

            MazeUserControl myMaze = new MazeUserControl();
            //Set margin of the my maze.
            Thickness margin = myMaze.Margin;
            margin.Left = 10;
            margin.Right = 20;
            myMaze.Margin = margin;
            
            MazeUserControl otherMaze = new MazeUserControl();
            //Set the margin of other maze.
            Thickness otherMargin = otherMaze.Margin;
            otherMargin.Left = 10;
            otherMargin.Right = 20;
            otherMaze.Margin = otherMargin;

            //Set rows and cols of other maze.
            myMaze.MazeRows = rows;
            myMaze.MazeCols = cols;
            otherMaze.MazeRows = rows;
            otherMaze.MazeCols = cols;
            mazes.Children.Add(myMaze);
            Grid.SetRow(myMaze, 1);
            Grid.SetColumn(myMaze, 0);
            mazes.Children.Add(otherMaze);
            Grid.SetRow(otherMaze, 1);
            Grid.SetColumn(otherMaze, 1);

            mainGrid.Children.Add(mazes);
            Grid.SetRow(mazes, 1);
            Grid.SetColumn(mazes, 0);

            Content = mainGrid;

            base.OnInitialized(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
