using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ViewModel
{
    /// <summary>
    /// Notifies when the game ends.
    /// It excpect listeners to creates 2 buttons with the content passed in the
    /// arguments(one content fow each button, accordingly).
    /// </summary>
    /// <param name="message"> The ending message. </param>
    /// <param name="firstButton"> The first button content. </param>
    /// <param name="secondButton"> The second button content. </param>
    public delegate void Ended(string message, string firstButton, string secondButton);

    /// <summary>
    /// This class is part of the "View" in the "MVVM" standard.
    /// It draws, moves and update the maze on the screen.
    /// </summary>
    public class MazeBoard
    {
        /// <summary>
        /// event that notify when the game ends.
        /// </summary>
        public event Ended GameDone;

        /// <summary>
        /// The maze drawing that is on the screen.
        /// </summary>
        private Canvas maze;

        /// <summary>
        /// The location that the player starts in.
        /// </summary>
        private Location startingPosition;

        /// <summary>
        /// Serial number of starting location,we only need this specific serial number.
        /// </summary>
        private int startSerialNumber;

        /// <summary>
        /// The location that the player need to arrive in oreder to win.
        /// </summary>
        private Location endPosition;

        /// <summary>
        /// The image that represent the player.
        /// </summary>
        private ImageSource playerLogo;

        /// <summary>
        /// The string that represents the maze. 0 means a free pass and 1 means a wall.
        /// </summary>
        private string mazeString;

        /// <summary>
        /// The number of rows in the maze.
        /// </summary>
        private int rows;

        /// <summary>
        /// The number of cols in the maze.
        /// </summary>
        private int cols;

        /// <summary>
        /// Rows property.
        /// </summary>
        /// <value>
        /// The number of rows in the current maze.
        /// </value>
        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        /// <summary>
        /// Cols property.
        /// </summary>
        /// <value>
        /// The number of cols in the current maze.
        /// </value>
        public int Cols
        {
            get { return cols; }
            set { cols = value; }
        }

        /// <summary>
        /// MazeString property. It's value is a sequance of 0,1
        /// where 1 represents a wall and 0 reprenets a free pass.
        /// </summary>
        /// <value>
        /// A sequance of 0,1 where 1 represents a wall and 0 reprenets a free pass.
        /// </value>
        public string MazeString
        {
            get { return mazeString; }
            set { mazeString = value; }
        }

        /// <summary>
        /// StartingPosition property.
        /// </summary>
        /// <value>
        /// A new starting location.
        /// </value>
        public Location StartingPosition
        {
            set { startingPosition = value; }
        }

        /// <summary>
        /// EndPosition property.
        /// </summary>
        /// <value>
        /// A new ending location.
        /// </value>
        public Location EndPosition
        {
            set { endPosition = value; }
        }

        /// <summary>
        /// Save ImageSource,that way we won't need to load the image again and again;
        /// </summary>
        private MazeLogic logic;

        /// <summary>
        /// Width of every rectangle of the maze,this will help us in the keydown event
        /// to calculate the players location after a horizontal movement.
        /// </summary>
        private double widthPerRect;

        /// <summary>
        /// Height of every rectangle of the maze,this will help us in the keydown event
        /// to calculate the players location after a vertical movement.
        /// </summary>
        private double heightPerRect;

        /// <summary>
        /// WidthPerRect property.
        /// The width of a single squre in the maze.
        /// </summary>
        /// <value>
        /// New width for each rectangle.
        /// </value>
        public double WidthPerRect
        {
            get { return widthPerRect; }
            set { widthPerRect = value; }
        }

        /// <summary>
        /// HeightPerRect property.
        /// The height of a single squre in the maze.
        /// </summary>
        /// <value>
        /// New height for each rectangle.
        /// </value>
        public double HeightPerRect
        {
            get { return heightPerRect; }
            set { heightPerRect = value; }
        }

        /// <summary>
        /// PlayerLogo property.
        /// The image of the player in the maze.
        /// </summary>
        /// <value>
        /// New image for the player.
        /// </value>
        public ImageSource PlayerLogo
        {
            get { return playerLogo; }
            set { playerLogo = value; }
        }

        /// <summary>
        /// Maze property.
        /// The maze drawing that is on the screen.
        /// </summary>
        /// <value>
        /// New maze drawing.
        /// </value>
        public Canvas Maze
        {
            get { return maze; }
            set { maze = value; }
        }

        /// <summary>
        /// Constructor.
        /// Creates the logic member.
        /// </summary>
        /// <param name="canvas"> Empty Canvas to fill with the maze drawing. </param>
        public MazeBoard(Canvas canvas)
        {
            maze = canvas;
            logic = new MazeLogic();
        }

        /// <summary>
        /// True if it's my maze false if it's enemy maze.
        /// </summary>
        private bool isMine;

        /// <summary>
        /// IsMine property.
        /// True if it's my maze false if it's enemy maze.
        /// </summary>
        /// <value>
        /// New boolean value to set "IsMine" with.
        /// </value>
        public bool IsMine
        {
            get { return isMine; }
            set
            {
                isMine = value;
            }
        }

        /// <summary>
        /// Draw a new maze to the screen if it can(all the required members are set).
        /// memebers requirments for this to work:
        ///     rows > 0
        ///     cols > 0
        ///     startingPosition != null
        ///     endPosition != null
        /// Also, strMaze != null is required. It will be the new "mazeString".
        /// </summary>
        /// <param name="strMaze"> a string at the format as specified in "mazeString" comments. </param>
        public void DrawOnCanvas(string strMaze)
        {
            if (rows == 0 || cols == 0 || strMaze == null || startingPosition == null
                    || endPosition == null)
            {
                return;
            }
            double heightPerRectangle = maze.Height / rows;
            double widthPerRectangle = maze.Width / cols;
            WidthPerRect = widthPerRectangle;
            HeightPerRect = heightPerRectangle;
            Rectangle current;
            double currentX = 0, currentY = 0;
            int k = 0,serialNumber = 1;
            for (int i = 0; i < rows; i++)
            {
                currentX = 0;
                for (k = 0; k < cols; k++)
                {
                    if (i == startingPosition.X && k == startingPosition.Y)
                    {
                        //This is starting position,here comes mario.
                        //Save image source as private property.
                        if (isMine)
                            playerLogo = new BitmapImage(new Uri("Images\\mario.png", UriKind.Relative));
                        else
                            playerLogo = new BitmapImage(new Uri("Images\\luigi.png", UriKind.Relative));
                        current = new Rectangle
                        {
                            Width = widthPerRectangle,
                            Height = heightPerRectangle,
                            Fill = new ImageBrush { ImageSource = playerLogo }
                        };

                        //Save the player's location.
                        //Set starting location in correct coordinates.
                        startingPosition = new Location(currentX, currentY);
                        logic.PlayerLocation = startingPosition;
                        //Set serial number of player,sometimes comparing double doesn't work
                        logic.PlayerSerialNumber = serialNumber;
                        startSerialNumber = serialNumber;
                    } else if (i == endPosition.X && k == endPosition.Y)
                    {
                        ImageSource goal = new BitmapImage(new Uri("Images\\goal.png", UriKind.RelativeOrAbsolute));
                        current = new Rectangle
                        {
                            Width = widthPerRectangle,
                            Height = heightPerRectangle,
                            Fill = new ImageBrush { ImageSource = goal }
                        };
                        endPosition = new Location(currentX, currentY);
                    } else if (strMaze.ToCharArray()[rows * i + k] == '0')
                    {
                        //Draw white rectangle.
                        current = new Rectangle
                        {
                            Width = widthPerRectangle,
                            Height = heightPerRectangle,
                            Fill = new SolidColorBrush(System.Windows.Media.Colors.White),
                            Stroke = Brushes.White,
                            StrokeThickness = 0.05
                        };

                    }
                    else 
                    {
                        //Draw black rectangle.Illegal location.
                        logic.AddIllegal(serialNumber);
                        current = new Rectangle
                        {
                            Width = widthPerRectangle,
                            Height = heightPerRectangle,
                            Fill = new SolidColorBrush(System.Windows.Media.Colors.Black),
                            Stroke = Brushes.White,
                            StrokeThickness = 0.05
                        };
                    }
                    Canvas.SetLeft(current, currentX);
                    Canvas.SetTop(current, currentY);
                    //Add location to the map.
                    logic.AddToMap(serialNumber, new Location(currentX, currentY));
                    maze.Children.Add(current);
                    currentX += widthPerRectangle;
                    serialNumber++;
                }
                currentY += heightPerRectangle;
            }
        }

        /// <summary>
        /// Draws the player on the maze.
        /// Given a valid key(see "CalculateLocation" for valid keys) it will
        /// move the player to a different locaion depending on the key.
        /// </summary>
        /// <param name="key"> Key class that determine the movement. </param>
        public void DrawPlayer(Key key)
        {
            Location loc = CalculateLocation(key);
            if (loc == null)
                return;
            if (ReplacePlayerLocation(loc))
            {
                if (isMine)
                    GameDone("Congrats you won!", "Back to main menu", "Close game");
                else
                    GameDone("You lost.\n Good luck next time.", "Back to main menu", "Close game");
                //Update view that game is over and update view model.
            }
        }

        /// <summary>
        /// Redraw the player at the given "loc" and redraw the current location of
        /// the player as an free pass.
        /// </summary>
        /// <param name="loc"> The player new location. </param>
        /// <returns bool>True if player got to goal,false otherwise</returns>
        private bool ReplacePlayerLocation(Location loc)
        {
            //Delete player from previous location
            Rectangle white = new Rectangle
            {
                Width = widthPerRect,
                Height = heightPerRect,
                Fill = new SolidColorBrush(System.Windows.Media.Colors.White),
                Stroke = Brushes.White,
                StrokeThickness = 0.05
            };
            //Add to canvas and draw it on player logo.
            Canvas.SetLeft(white, logic.PlayerLocation.X);
            Canvas.SetTop(white, logic.PlayerLocation.Y);
            maze.Children.Add(white);

            //Now draw the player
            Rectangle playerRect = new Rectangle
            {
                Width = widthPerRect,
                Height = heightPerRect,
                Fill = new ImageBrush { ImageSource = playerLogo }
            };
            Canvas.SetLeft(playerRect, loc.X);
            Canvas.SetTop(playerRect, loc.Y);
            logic.PlayerLocation = loc;
            maze.Children.Add(playerRect);
            //Return true if player got to the end of the maze.
            if (loc.Equals(endPosition))
                return true;
            return false;
        }

        /// <summary>
        /// Creates the location of the player if he moved using "key". It uses
        /// the member "logic" to calculate the above location.
        /// key is only meaningful for: Key.Right, Key.Left, Key.Up, Key.Down.
        /// </summary>
        /// <param name="key"> Key value that determine the location.</param>
        /// <returns> The location of the player if he moved using "key". </returns>
        private Location CalculateLocation(Key key)
        {
            Location newLocation;
            double xLimit = maze.Width - widthPerRect;
            double yLimit = maze.Height - heightPerRect;
            switch (key)
            {
                case Key.Up:
                    {
                        if (logic.PlayerSerialNumber - cols < 0)
                        {
                            //Out of bounds.
                            return null;
                        }
                        newLocation = logic.GetLocation(logic.PlayerSerialNumber - cols);
                        if (!logic.IsIllegal(
                            logic.PlayerSerialNumber - cols))
                        {
                            //Location is llegal.
                            logic.PlayerSerialNumber = logic.PlayerSerialNumber - cols;
                            return newLocation;
                        }
                        break;
                    }

                case Key.Down:
                    {
                        if (logic.PlayerSerialNumber + cols > rows * cols)
                        {
                            //Out of bounds
                            return null;
                        }

                        newLocation = logic.GetLocation(logic.PlayerSerialNumber + cols);
                        if (!logic.IsIllegal(
                            logic.PlayerSerialNumber + cols))
                        {
                            logic.PlayerSerialNumber = logic.PlayerSerialNumber + cols;
                            return newLocation;
                        }
                        break;
                    }

                case Key.Left:
                    {
                        if (logic.PlayerSerialNumber - 1 < 0)
                        {
                            //Out of bounds.
                            return null;
                        }
                        newLocation = logic.GetLocation(logic.PlayerSerialNumber - 1);
                        if (!logic.IsIllegal(
                               logic.PlayerSerialNumber - 1))
                        {
                            logic.PlayerSerialNumber = logic.PlayerSerialNumber - 1;
                            return newLocation;
                        }
                        break;
                    }

                case Key.Right:
                    {
                       if (logic.PlayerSerialNumber % cols == 0 )
                        {
                            //We are at the end of the row,out of bounds.
                            return null;
                        }
                        newLocation = logic.GetLocation(logic.PlayerSerialNumber + 1);
                        if (!logic.IsIllegal(
                            logic.PlayerSerialNumber + 1))
                        {
                            logic.PlayerSerialNumber = logic.PlayerSerialNumber + 1;
                            return newLocation;
                        }
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
            return null;
        }

        /// <summary>
        /// Move the player to it's starting position and redraw the goal image
        /// at the ending location.
        /// </summary>
        public void RestartGame()
        {
            ReplacePlayerLocation(startingPosition);
            logic.PlayerSerialNumber = startSerialNumber;
            ImageSource goal = new BitmapImage(new Uri("Images\\goal.png", UriKind.RelativeOrAbsolute));
            Rectangle goalRec = new Rectangle
            {
                Height = maze.Height / rows,
                Width = maze.Width / cols,
                Fill = new ImageBrush { ImageSource = goal }
            };
            Canvas.SetLeft(goalRec, endPosition.X);
            Canvas.SetTop(goalRec, endPosition.Y);
            maze.Children.Add(goalRec);
        }

        /// <summary>
        /// Creates a list of serial numbers that represent locations(see Logic class)
        /// according to the solution string. Then it calls "SolutionAnimation" with the
        /// above list as its input.
        /// </summary>
        /// <param name="solution">
        /// A string representing a legal path from starting location
        /// to ending location. "solution" contains only 0,1,2,3.
        /// </param>
        public void AnimateSolution(string solution)
        {
            char[] arr = solution.ToCharArray();
            char currentChar;
            int length = arr.Length;
            List<int> animationSerialNumbers = new List<int>();
            //Save current serial number and add it as first element of list.
            int currentSerial = logic.PlayerSerialNumber;
            for (int i = 0;i < length; i++)
            {
                currentChar = arr[i];
                switch (currentChar)
                {
                    case '0':
                        {
                            //Go left.
                            currentSerial -= 1;
                            animationSerialNumbers.Add(currentSerial);
                            break;
                        }
                    case '1':
                        {
                            //Go right.
                            currentSerial += 1;
                            animationSerialNumbers.Add(currentSerial);
                            break;
                        }
                    case '2':
                        {
                            //Go up.
                            currentSerial -= cols;
                            animationSerialNumbers.Add(currentSerial);
                            break;
                        }
                    case '3':
                        {
                            //Go down.
                            currentSerial += cols;
                            animationSerialNumbers.Add(currentSerial);
                            break;
                        }
                    default:
                        {
                            //Do nothing.
                            break;
                        }
                }
            }

            //Now create the animation.
            SolutionAnimation(animationSerialNumbers);
        }

        /// <summary>
        /// Calls Activate.
        /// </summary>
        /// <param name="serialNumbers">
        /// List of serial numbers that represent locations(see Logic class)
        /// </param>
        public void SolutionAnimation(List<int> serialNumbers)
        {
            Activate(serialNumbers);
        }

        /// <summary>
        /// Runs an animation of who the player could get from the starting location
        /// to the goal location.
        /// </summary>
        /// <param name="serialNumbers">
        /// List of serial numbers that represent locations(see Logic class)
        /// </param>
        public async void Activate(List<int> serialNumbers)
        {
            Location current;
            foreach(int num in serialNumbers)
            {
                await Task.Delay(200);
                current = logic.GetLocation(num);
                //Move the player to this location.
                ReplacePlayerLocation(current);
            }
        }

    }
}
