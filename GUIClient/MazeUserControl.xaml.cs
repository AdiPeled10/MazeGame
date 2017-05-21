using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using ViewModel;
using System.Windows.Threading;

namespace GUIClient
{
    /// <summary>
    /// delegate that notify listeners when a game is over.
    /// </summary>
    public delegate void GameOver();

    /// <summary>
    /// Interaction logic for MazeUserControl.xaml
    /// </summary>
    public partial class MazeUserControl : UserControl,INotifyPropertyChanged
    {
        /// <summary>
        /// Event that notify listeners when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event that notifies that game is done.
        /// </summary>
        public event GameOver Done;

        /// <summary>
        /// Invoke "PropertyChanged" member.
        /// </summary>
        /// <param name="propName"> The name of the property that has changed. </param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Message for mudal window.
        /// </summary>
        private string mudalMessage;

        /// <summary>
        /// The content for mudal window first button.
        /// </summary>
        private string mudalFirstButton;

        /// <summary>
        /// The content for mudal window second button.
        /// </summary>
        private string mudalSecondButton;

        /// <summary>
        /// MudalMessage property.
        /// Get or set the "mudalMessage" field.
        /// Also, if set, "NotifyPropertyChanged" is called.
        /// </summary>
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

        /// <summary>
        /// MudalFirstButton property.
        /// Get or set the "mudalFirstButton" field.
        /// Also, if set, "NotifyPropertyChanged" is called.
        /// </summary>
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

        /// <summary>
        /// MudalSecondButton property.
        /// Get or set the "mudalSecondButton" field.
        /// Also, if set, "NotifyPropertyChanged" is called.
        /// </summary>
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

        /// <summary>
        /// MazeCols property.
        /// Get or set the "mazeCols" field.
        /// Also, if set, redrawing the maze to the screen.
        /// </summary>
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

        /// <summary>
        /// Using a DependencyProperty as the backing store for MazeCols.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MazeColsProperty =
            DependencyProperty.Register("MazeCols", typeof(int), 
                typeof(MazeUserControl), new UIPropertyMetadata(UpdatedCols));

        /// <summary>
        /// Update d.MazeCols with args.NewValue.
        /// </summary>
        /// <param name="d"> MazeUserControl object. </param>
        /// <param name="args">
        /// DependencyPropertyChangedEventArgs with the field "NewValue" as int.
        /// </param>
        private static void UpdatedCols(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeCols = (int)args.NewValue;
        }

        /// <summary>
        /// MazeRows property.
        /// Get or set the "mazeRows" field.
        /// Also, if set, redrawing the maze to the screen.
        /// </summary>
        public int MazeRows
        {
            get { return (int)GetValue(MazeRowsProperty); }
            set {
                SetValue(MazeRowsProperty, value);
                board.Rows = value;
                board.DrawOnCanvas(MazeString);
            }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for MazeCols.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MazeRowsProperty =
            DependencyProperty.Register("MazeRows", typeof(int), 
                typeof(MazeUserControl), new UIPropertyMetadata(UpdatedRows));

        /// <summary>
        /// Update d.MazeRows with args.NewValue.
        /// </summary>
        /// <param name="d"> MazeUserControl object. </param>
        /// <param name="args">
        /// DependencyPropertyChangedEventArgs with the field "NewValue" as int.
        /// </param>
        private static void UpdatedRows(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeRows = (int)args.NewValue;
        }

        /// <summary>
        /// MazeString property.
        /// Get or set the "mazeString" field.
        /// Also, if set, redrawing the maze to the screen.
        /// </summary>
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

        /// <summary>
        /// Using a DependencyProperty as the backing store for MazeCols.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MazeStringProperty =
            DependencyProperty.Register("MazeString", typeof(string),
                typeof(MazeUserControl), new UIPropertyMetadata(MazeStringChanged));

        /// <summary>
        /// Update d.MazeString with args.NewValue.
        /// </summary>
        /// <param name="d"> MazeUserControl object. </param>
        /// <param name="args">
        /// DependencyPropertyChangedEventArgs with the field "NewValue" as string.
        /// </param>
        private static void MazeStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.MazeString = (string)args.NewValue;
        }

        /// <summary>
        /// StartLocation property.
        /// Get or set the "startLocation" field.
        /// Also, if set, redrawing the maze to the screen.
        /// </summary>
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

        /// <summary>
        /// Using a DependencyProperty as the backing store for MazeCols.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty StartLocationProperty =
            DependencyProperty.Register("StartLocation", typeof(Location), 
                typeof(MazeUserControl),new UIPropertyMetadata(UpdatedStart));

        /// <summary>
        /// Update d.StartLocation with args.NewValue.
        /// </summary>
        /// <param name="d"> MazeUserControl object. </param>
        /// <param name="args">
        /// DependencyPropertyChangedEventArgs with the field "NewValue" as Location.
        /// </param>
        private static void UpdatedStart(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.StartLocation = (Location)args.NewValue;
        }

        /// <summary>
        /// EndLocation property.
        /// Get or set the "endLocation" field.
        /// Also, if set, redrawing the maze to the screen.
        /// </summary>
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

        /// <summary>
        /// Tells if it's my maze or the other player maze.
        /// </summary>
        private bool isMine = true;

        /// <summary>
        /// IsMine property.
        /// Get or set the "isMine" field.
        /// </summary>
        public bool IsMine
        {
            get { return isMine; }
            set
            {
                isMine = value;
                board.IsMine = value;
            }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for MazeCols.
        /// This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty EndLocationProperty =
            DependencyProperty.Register("EndLocation", typeof(Location), 
                typeof(MazeUserControl),new UIPropertyMetadata(UpdateEnd));

        /// <summary>
        /// Update d.EndLocation with args.NewValue.
        /// </summary>
        /// <param name="d"> MazeUserControl object. </param>
        /// <param name="args">
        /// DependencyPropertyChangedEventArgs with the field "NewValue" as Location.
        /// </param>
        private static void UpdateEnd(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            MazeUserControl maze = (MazeUserControl)d;
            maze.EndLocation = (Location)args.NewValue;
        }


        /* The Begining of some Real Coding */


        /// <summary>
        /// The maze board which will manage the logic of drawing and movement on
        /// the maze.
        /// </summary>
        private MazeBoard board;

        /// <summary>
        /// The ViewModel object. Used as Model for this object.
        /// This class tells it when a key is pressed.
        /// </summary>
        private MazeViewModel vm;
       
        /// <summary>
        /// Constructor.
        /// </summary>
        public MazeUserControl()
        {
            InitializeComponent();
            board = new MazeBoard(myCanvas);
            // Add game ended function to event.
            board.GameDone += GameEnded;
            vm = new MazeViewModel(board);
        }

        /// <summary>
        /// Opens a windows with text and two buttons. The text content is "message",
        /// the first button content is "firstButton" and the second button content
        /// is "secondButton".
        /// One button closes the program. The other close the window and opens the main
        /// window(starting window).
        /// </summary>
        /// <param name="message"> The text from above. </param>
        /// <param name="firstButton"> The content from above. </param>
        /// <param name="secondButton"> The content from above. </param>
        private void GameEnded(string message, string firstButton, string secondButton)
        {
            MudalWindow window = new MudalWindow()
            {
                //Set data context.
                DataContext = this
            };
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

        /// <summary>
        /// Notify "vm" member what key was pressed.
        /// </summary>
        /// <param name="sender"> Meaningless. </param>
        /// <param name="e"> e.Key should contain the pressed key. </param>
        private void BorderKeyDown(object sender, KeyEventArgs e)
        {
            vm.KeyPressed(e.Key);
        }

        /// <summary>
        /// Gets the window and if this maze is our, it adds a listener to "KeyDown"
        /// of the window.
        /// </summary>
        /// <param name="sender"> Meaningless. </param>
        /// <param name="e"> Meaningless. </param>
        private void MazeLoaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            //Add only if it's my window.
            if (IsMine)
                window.KeyDown += BorderKeyDown;
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