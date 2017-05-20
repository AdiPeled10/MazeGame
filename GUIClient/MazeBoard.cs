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
using System.Windows.Threading;
using System.Threading;

namespace ViewModel
{
    public delegate void Ended(string message, string firstButton, string secondButton);

    public class MazeBoard
    {
        public event Ended GameDone;

        private Canvas maze;

        /// <summary>
        /// The location that the player started in.
        /// </summary>
        private Location startingPosition;

        /// <summary>
        /// Serial number of starting location,we only need this specific serial number.
        /// </summary>
        private int startSerialNumber;

        private Location endPosition;

        private ImageSource playerLogo;

        private string mazeString;

        private int rows;

        private int cols;

        public int Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        public int Cols
        {
            get { return cols; }
            set { cols = value; }
        }

        public string MazeString
        {
            get { return mazeString; }
            set { mazeString = value; }
        }

        public Location StartingPosition
        {
            set { startingPosition = value; }
        }

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

        public double WidthPerRect
        {
            get { return widthPerRect; }
            set { widthPerRect = value; }
        }

        public double HeightPerRect
        {
            get { return heightPerRect; }
            set { heightPerRect = value; }
        }

        public ImageSource PlayerLogo
        {
            get { return playerLogo; }
            set { playerLogo = value; }
        }

        public Canvas Maze
        {
            get { return maze; }
            set { maze = value; }
        }

        public MazeBoard(Canvas canvas)
        {
            maze = canvas;
            logic = new MazeLogic();
        }

        /// <summary>
        /// True if it's my maze false if it's enemy maze.
        /// </summary>
        private bool isMine;

        public bool IsMine
        {
            get { return isMine; }
            set
            {
                isMine = value;
            }
        }

        public void DrawOnCanvas(string strMaze)
        {
            //FOR NOW FIX LATER.
            strMaze = mazeString;
            //REMEMBER TO FIX LATER.
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
        /// 
        /// </summary>
        /// <param name="loc"></param>
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
        /// Move the player to it's starting position.
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

        public void AnimateSolution(string solution)
        {
            //TODO- Check if this can be done only when player wasn't moved at all.
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

        public void SolutionAnimation(List<int> serialNumbers)
        {
            int length = serialNumbers.Count;
            Activate(serialNumbers,length);
        }

        public async void Activate(List<int> serialNumbers, int length)
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
