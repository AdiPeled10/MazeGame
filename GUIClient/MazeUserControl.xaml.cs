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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;


namespace GUIClient
{

    /// <summary>
    /// Interaction logic for MazeUserControl.xaml
    /// </summary>
    public partial class MazeUserControl : UserControl
    {
        // private Grid grid;
        /// <summary>
        /// The number of rows in the maze.
        /// </summary>
        private int rows;

        /// <summary>
        /// The number of columns in the maze.
        /// </summary>
        private int cols;

        /// <summary>
        /// The maze board which will manage the logic of drawing and movement on
        /// the maze.
        /// </summary>
        private MazeBoard board;

        private MazeViewModel vm;


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
            set { cols = value; }
        }
        

       

        public MazeUserControl()
        {
        }


        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            board = new MazeBoard(myCanvas);
            vm = new MazeViewModel( rows, cols,board);
            board.DrawOnCanvas(vm.Maze, rows, cols);
            base.OnInitialized(e);
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            vm.KeyPressed(e.Key);
        }

       
        private void MazeLoaded(object sender,RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += Border_KeyDown;
        }

        public void Restart()
        {
            board.RestartGame();
        }


    }
}