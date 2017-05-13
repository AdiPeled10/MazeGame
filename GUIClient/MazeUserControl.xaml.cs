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





        public int MazeCols
        {
            get { return (int)GetValue(MazeColsProperty); }
            set {
                SetValue(MazeColsProperty, value);
                board.DrawOnCanvas(MazeString, MazeRows, MazeCols);
            }
        }

        // Using a DependencyProperty as the backing store for MazeCols.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeColsProperty =
            DependencyProperty.Register("MazeCols", typeof(int), 
                typeof(MazeUserControl), new UIPropertyMetadata(UpdatedCols));

        private static void UpdatedCols(DependencyObject d,DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeRows = (int)args.NewValue;
        }


        public int MazeRows
        {
            get { return (int)GetValue(MazeRowsProperty); }
            set {
                SetValue(MazeRowsProperty, value);
                board.DrawOnCanvas(MazeString, MazeRows, MazeCols);
            }
        }

        // Using a DependencyProperty as the backing store for MazeRows.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeRowsProperty =
            DependencyProperty.Register("MazeRows", typeof(int), 
                typeof(MazeUserControl), new UIPropertyMetadata(UpdatedRows));

       private static void UpdatedRows(DependencyObject d,DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeRows = (int)args.NewValue;
        }


        public string MazeString
        {
            get { return (string)GetValue(MazeStringProperty); }
            set
            {
                SetValue(MazeStringProperty, value);
                try
                {
                    //Set maze string in board.
                    board.MazeString = value;
                    board.DrawOnCanvas(value, MazeRows, MazeCols);
                } catch(NullReferenceException) { }
            }
        }

        // Using a DependencyProperty as the backing store for MazeString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeStringProperty =
            DependencyProperty.Register("MazeString", typeof(string),
                typeof(MazeUserControl), new UIPropertyMetadata(MazeStringChanged));

        private static void MazeStringChanged(DependencyObject d,DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeString = (string)args.NewValue;
        }



        public Location StartLocation
        {
            get { return (Location)GetValue(StartLocationProperty); }
            set
            {
                SetValue(StartLocationProperty, value);
                try
                {
                    board.StartingPosition = value;
                } catch(NullReferenceException) { }
            }
        }

        // Using a DependencyProperty as the backing store for StartLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartLocationProperty =
            DependencyProperty.Register("StartLocation", typeof(Location), 
                typeof(MazeUserControl),new UIPropertyMetadata(UpdatedStart));

        private static void UpdatedStart(DependencyObject d,DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.StartLocation = (Location)args.NewValue;
        }


        public Location EndLocation
        {
            get { return (Location)GetValue(EndLocationProperty); }
            set
            {
                SetValue(EndLocationProperty, value);
                //Set value in MazeBoard.
                try
                {
                    board.EndPosition = value;
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        // Using a DependencyProperty as the backing store for EndLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndLocationProperty =
            DependencyProperty.Register("EndLocation", typeof(Location), 
                typeof(MazeUserControl));

        private static void UpdateEnd(DependencyObject d,DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.EndLocation = (Location)args.NewValue;
        }


        /// <summary>
        /// The maze board which will manage the logic of drawing and movement on
        /// the maze.
        /// </summary>
        private MazeBoard board;

        private MazeViewModel vm;


        

       

        public MazeUserControl()
        {
            InitializeComponent();
            board = new MazeBoard(myCanvas);
            vm = new MazeViewModel(board);
        }


        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            board.Maze = myCanvas;
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