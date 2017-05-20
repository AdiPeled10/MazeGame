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
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerMaze.xaml
    /// </summary>


    public partial class SinglePlayerMaze : Window
    {
        private int rows;
        private int cols;
        private int realWidth;
        private int realHeight;
        private string mazeName;

        /// <summary>
        /// Set to false if connection failed,true otherwise.
        /// </summary>
        private bool connected = true;

        private SinglePlayerVM vm;

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

        public bool Connected
        {
            get { return connected; }
        }

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

        public SinglePlayerMaze()
        {
            InitializeComponent();
            maze.Done += GameDone;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Generate()
        {
            vm.GenerateMaze(MazeName, Rows, Cols);
        }

        public void GameDone()
        {
            this.Close();
        }

        public void GetSolution()
        {
            //int start_x = (int)vm.StartLocation.X, start_y = (int)vm.StartLocation.Y;
            //int end_x = (int)vm.EndLocation.X, end_y = (int)vm.EndLocation.Y;
            //vm.AskForSolution(rows + "," + cols + "," + start_x + "," + start_y + "," +
            //    end_x + "," + end_y + "," + vm.MazeString);
            vm.AskForSolution(vm.MazeName);
        }

        public void CloseConnection()
        {
            vm.CloseConnection();
        }

        private void mainMenu_Click(object sender, RoutedEventArgs e)
        {
            MudalWindow win = new MudalWindow();
            win.DataContext = maze;
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
        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            maze.Restart();
        }

        private void Solve_Click(object sender,RoutedEventArgs e)
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
