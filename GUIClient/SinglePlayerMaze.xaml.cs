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
    /// Interaction logic for SinglePlayerMaze.xaml
    /// </summary>
    public partial class SinglePlayerMaze : Window
    {
        private int rows;
        private int cols;

        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                rows = value;
            }
        }

        public int Cols
        {
            get
            {
                return cols;
            }

            set
            {
                cols = value;
            }
        }

        public SinglePlayerMaze()
        {
            ////InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            Grid grid = new Grid();
            MazeUserControl maze = new MazeUserControl();
            grid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            grid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });
            maze.Cols = Cols;
            maze.Rows = Rows;
            grid.Children.Add(maze);
            Grid.SetRow(maze, 0);
            Grid.SetColumn(maze, 0);
            dockPanel.Children.Add(grid);
            DockPanel.SetDock(grid, Dock.Top);
           // grid.Children.Add(maze);
           // Grid.SetRow(maze,1);
           // Grid.SetColumn(maze, 1);
            base.OnInitialized(e);
        }

    }
}
