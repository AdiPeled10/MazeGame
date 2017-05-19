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
using System.ComponentModel;
using ViewModel;
using System.Windows.Threading;

namespace GUIClient
{

    public delegate void GameOver();

    /// <summary>
    /// Interaction logic for MazeUserControl.xaml
    /// </summary>
    public partial class MazeUserControl : UserControl,INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event that notifies that game is done.
        /// </summary>
        public event GameOver Done;

        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Message for mudal window.
        /// </summary>
        private string mudalMessage;

        private string mudalFirstButton;

        private string mudalSecondButton;

        public string MudalMessage
        {
            get { return mudalMessage; }
            set
            {
                if (mudalMessage != value)
                {
                    mudalMessage = value;
                    NotifyPropertyChanged("MudalMessage");
                }
            }
        }

        public string MudalFirstButton
        {
            get { return mudalFirstButton; }
            set
            {
                if (mudalFirstButton != value)
                {
                    mudalFirstButton = value;
                    NotifyPropertyChanged("MudalFirstButton");
                }
            }
        }

        public string MudalSecondButton
        {
            get { return mudalSecondButton; }
            set
            {
                if (mudalSecondButton != value)
                {
                    mudalSecondButton = value;
                    NotifyPropertyChanged("MudalSecondButton");
                }
            }
        }

        public int MazeCols
        {
            get { return (int)GetValue(MazeColsProperty); }
            set {
                SetValue(MazeColsProperty, value);
                //Set value in MazeBoard.
                board.Cols = value;
                board.DrawOnCanvas(MazeString);
            }
        }

        // Using a DependencyProperty as the backing store for MazeCols.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MazeColsProperty =
            DependencyProperty.Register("MazeCols", typeof(int), 
                typeof(MazeUserControl), new UIPropertyMetadata(UpdatedCols));

        private static void UpdatedCols(DependencyObject d,DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeCols = (int)args.NewValue;
        }


        public int MazeRows
        {
            get { return (int)GetValue(MazeRowsProperty); }
            set {
                SetValue(MazeRowsProperty, value);
                board.Rows = value;
                board.DrawOnCanvas(MazeString);
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
                    board.DrawOnCanvas(value);
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
                    board.DrawOnCanvas(MazeString);
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
                    board.DrawOnCanvas(MazeString);
                }
                catch (NullReferenceException)
                {
                }
            }
        }

        //Tells if it's my maze or the other player's maze.
        private bool isMine = true;

        public bool IsMine
        {
            get { return isMine; }
            set
            {
                isMine = value;
                board.IsMine = value;
            }
        }

        // Using a DependencyProperty as the backing store for EndLocation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndLocationProperty =
            DependencyProperty.Register("EndLocation", typeof(Location), 
                typeof(MazeUserControl),new UIPropertyMetadata(UpdateEnd));

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
            //Add game ended function to event.
            board.GameDone += GameEnded;
            vm = new MazeViewModel(board);
        }

        private void GameEnded(string message,string firstButton,string secondButton)
        {
            MudalWindow window = new MudalWindow();
            //Set data context.
            window.DataContext = this;
            MudalMessage = message;
            MudalFirstButton = firstButton;
            MudalSecondButton = secondButton;
            //Set methods that will run for each button.
            window.OnFirstButton = new RoutedEventHandler((object sender,RoutedEventArgs args) =>
            {
                //Return to main menu.
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                //Close mudal window.
                window.Close();
            });

            window.OnSecondButton = new RoutedEventHandler((object sender, RoutedEventArgs args)=>
            {
                //Close the window.
                window.Close();
            });
            try
            {
                Done();
            } catch (NullReferenceException) { }
            window.Show();
        }


        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            vm.KeyPressed(e.Key);
        }

       
        private void MazeLoaded(object sender,RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            //Add only if it's my window.
            if (IsMine)
                window.KeyDown += Border_KeyDown;
        }

        /// <summary>
        /// Restart the game.
        /// </summary>
        public void Restart()
        {
            MudalWindow win = new MudalWindow();
            win.DataContext = this;
            MudalMessage = "Are you sure?";
            MudalFirstButton = "Continue";
            MudalSecondButton = "Cancel";
            win.OnFirstButton = new RoutedEventHandler((object sender,RoutedEventArgs args) => 
            {
                board.RestartGame();
                win.Close();
            });
            
            //Don't do anything if cancel is chosen.
            win.OnSecondButton = new RoutedEventHandler((object sender,RoutedEventArgs args)=> {
                win.Close();
            });
            win.Show();
        }

        /// <summary>
        /// Activate animation that solves the maze.
        /// </summary>
        /// <param name="solution"></param>
        public void Solve(string solution)
        {
            MudalWindow win = new MudalWindow();
            win.DataContext = this;
            MudalMessage = "Are you sure?";
            MudalFirstButton = "Continue";
            MudalSecondButton = "Cancel";
            win.OnFirstButton = new RoutedEventHandler((object sender, RoutedEventArgs args) =>
            {
                //Return to start point of maze.
                board.RestartGame();
                board.AnimateSolution(solution);
                win.Close();
            });

            //Don't do anything if cancel is chosen.
            win.OnSecondButton = new RoutedEventHandler((object sender, RoutedEventArgs args) => {
                win.Close();
            });
            win.Show();
        }

        /// <summary>
        /// Move player in given direction,used in multiplayer.
        /// </summary>
        /// <param name="direction"></param>
        public void MovePlayer(string direction)
        {
            //Convert string to key.
            try
            {
                Application.Current.Dispatcher.BeginInvoke(
                   DispatcherPriority.Background,
                    new Action(() =>
                    {
                        KeyConverter k = new KeyConverter();
                        Key myKey = (Key)k.ConvertFromString(direction);
                        vm.KeyPressed(myKey);
                    }
                    ));
            } catch (Exception)
            {

            }

        }


    }
}