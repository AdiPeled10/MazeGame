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

namespace GUIClient
{
    public class MazeBoard
    {
        private Canvas maze;

        private ImageSource playerLogo;

        private MazeLogic logic;

        private double widthPerRect;

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
        }

        public MazeBoard(Canvas canvas)
        {
            maze = canvas;
            logic = new MazeLogic();
        }

        public void DrawOnCanvas(string strMaze,int rows,int cols)
        {
            double heightPerRectangle = maze.Height / rows;
            double widthPerRectangle = maze.Width / cols;
            WidthPerRect = widthPerRectangle;
            HeightPerRect = heightPerRectangle;
            Rectangle current;
            double currentX = 0, currentY = 0;
            int k = 0;
            for (int i = 0; i < rows; i++)
            {
                currentX = 0;
                for (k = 0; k < cols; k++)
                {
                    if (strMaze.ToCharArray()[rows * i + k] == '0')
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
                    else if (strMaze.ToCharArray()[rows * i + k] == '1')
                    {
                        //Draw black rectangle.
                        logic.AddIllegal(new Location(currentX, currentY));
                        current = new Rectangle
                        {
                            Width = widthPerRectangle,
                            Height = heightPerRectangle,
                            Fill = new SolidColorBrush(System.Windows.Media.Colors.Black),
                            Stroke = Brushes.White,
                            StrokeThickness = 0.05
                        };
                    }
                    else
                    {

                        //Save image source as private property.
                        playerLogo = new BitmapImage(new Uri("mario.png", UriKind.Relative));
                        current = new Rectangle
                        {
                            Width = widthPerRectangle,
                            Height = heightPerRectangle,
                            Fill = new ImageBrush { ImageSource = playerLogo }
                        };

                        //Save the player's location.
                        logic.PlayerLocation = new Location(currentX, currentY);
                    }
                    Canvas.SetLeft(current, currentX);
                    Canvas.SetTop(current, currentY);
                    maze.Children.Add(current);
                    currentX += widthPerRectangle;
                }
                currentY += heightPerRectangle;
            }
        }

        public void DrawPlayer(Key key)
        {
            Location loc = CalculateLocation(key);
            if (loc == null)
                return;
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
                        newLocation = new Location(logic.PlayerLocation.X,
                           logic.PlayerLocation.Y - heightPerRect);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
                            //Location is llegal.
                            return newLocation;
                        }
                        break;
                    }

                case Key.Down:
                    {
                        newLocation = new Location(logic.PlayerLocation.X,
                           logic.PlayerLocation.Y + heightPerRect);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
                           return newLocation;
                        }
                        break;
                    }

                case Key.Left:
                    {
                        newLocation = new Location(logic.PlayerLocation.X - widthPerRect,
                            logic.PlayerLocation.Y);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
                            return newLocation;
                        }
                        break;
                    }

                case Key.Right:
                    {
                        newLocation = new Location(logic.PlayerLocation.X + widthPerRect,
                            logic.PlayerLocation.Y);
                        if (!logic.IsIllegal(newLocation, xLimit, yLimit))
                        {
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

    }
}
