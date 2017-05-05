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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerMaze.xaml
    /// </summary>
    public partial class SinglePlayerMaze : Window,INotifyPropertyChanged
    {
        private int rows;
        private int cols;
        private int realWidth;
        private int realHeight;
        private string mazeName;

        public string MazeName
        {
            get { return mazeName; }
            set
            {
                if (mazeName != value)
                {
                    mazeName = value;
                    NotifyPropertyChanged("MazeName");
                }
            }
        }
        public int RealWidth
        {
            get { return realWidth; }
            set
            {
                if (realWidth != value)
                {
                    realWidth = value;
                    NotifyPropertyChanged("RealWidth");
                }
            }
        }

        public int RealHeight
        {
            get { return realHeight; }
            set
            {
                if (realHeight != value)
                {
                    realHeight = value;
                    NotifyPropertyChanged("RealHeight");
                }
            }
        }

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
                    NotifyPropertyChanged("Rows");
                }
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
                if (cols != value)
                {
                    cols = value;
                    // this.Width = 30 * Cols;
                    // stackPanel.Width = Width / (Cols + 1);
                    RealWidth = 30 * cols;
                    NotifyPropertyChanged("Cols");
                }
            }
        }

        public SinglePlayerMaze()
        {
            ////InitializeComponent();
            this.DataContext = this;
        }

       public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
