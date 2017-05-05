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
    /// <summary>
    /// Interaction logic for MazeUserControl.xaml
    /// </summary>
    public partial class MazeUserControl : UserControl
    {
       // private Grid grid;
        private int rows;
        private int cols;
        private string maze;

        public string Maze
        {
            get { return maze; }
            set { maze = value; }
        }

        public int Rows
        {
            get { return rows; }
            set {
                rows = value;
            }
        }

        public int Cols
        {
            get { return cols;}
            set { cols = value; }
        }

        /// <summary>
        /// This is here to help us test the design when the MVVM
        /// will be ready we will get the maze from the ViewModel.
        /// </summary>
        private void GenerateRandomMaze()
        {
            //For now generate the string for the maze here to test the design.
            //Later we will get it through the ViewModel.
            int loopVal = Rows * Cols;
            string myString = "";
            Random rand = new Random();
            int randomNumber = rand.Next(0,loopVal) ;
            for (int i = 0; i < loopVal; i++)
            {
                if (i != randomNumber)
                {
                    if (rand.Next(0, 2) == 0)
                    {
                        myString += '0';
                    }
                    else
                    {
                        myString += '1';
                    }
                }
                else
                {
                    myString += '*';
                }

            }
            maze = myString;
        }

        public MazeUserControl()
        {
        }

        public void CreateDynamicGrid()
        {
            Rectangle current;
            
            //Add rows and cols to grid.
            for (int i = 0; i < Rows; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }

            for (int j = 0;j < Cols; j++)
            {
                grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                     if (maze.ToCharArray()[i * Cols + j] == '0')
                    {
                        current = new Rectangle
                        {
                            Width = double.NaN,
                            Height = Width,
                        Fill = Brushes.Black,
                        Stroke = Brushes.Black
                        };
                        grid.Children.Add(current);
                        Grid.SetRow(current, i);
                        Grid.SetColumn(current, j);
                    } else if (maze.ToCharArray()[i * Cols + j] == '*')
                    {
                        current = new Rectangle
                        {
                            Width = Height,
                            Fill = Brushes.Pink,
                            Stroke = Brushes.Pink
                        };
                        grid.Children.Add(current);
                        Grid.SetRow(current, i);
                        Grid.SetColumn(current, j);
                    }

                }
            }
            //Content = new Label { Content = "liorrrrrrrrrrrrrr", Margin= new Thickness(50) };
            border.Child = grid;
            Content = border;
            //Done drawing everything went fine.
            //InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
       {
            InitializeComponent();
            GenerateRandomMaze();
            CreateDynamicGrid();
            base.OnInitialized(e);
       }

    }
}
