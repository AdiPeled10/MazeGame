using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerWindow.xaml
    /// </summary>
    public partial class MultiPlayerWindow : Window,INotifyPropertyChanged
    {
        private string mazeName;
        private int rows;
        private int cols;

        /// <summary>
        /// View model of the multi player game.
        /// </summary>
        private MultiPlayerVM vm;



        /// <summary>
        /// String that represents name of the maze.
        /// </summary>
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

        public int Rows
        {
            get { return rows; }
            set
            {
                if (rows != value)
                {
                    rows = value;
                    NotifyPropertyChanged("Rows");
                }
            }
        }

        public int Cols
        {
            get { return cols; }
            set
            {
                if (cols != value)
                {
                    cols = value;
                    NotifyPropertyChanged("Cols");
                }
            }
        }

        /// <summary>
        /// Constructor of multi player window.
        /// </summary>
        public MultiPlayerWindow()
        {
            InitializeComponent();
            vm = new MultiPlayerVM();
            vm.notifyConnection += OpponentConnected;
            DataContext = vm;
        }


        /// <summary>
        /// Event which will occur when  start game button will be clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {

            //Start game via the view model.
            waitingBlock.Text = "Waiting for opponent...";
            vm.GenerateMaze(maze.NameBox, maze.Rows, maze.Cols);
        }

        public void OpponentConnected()
        {
            MultiPlayerMaze mazeWindow = new MultiPlayerMaze();
            mazeWindow.Show();
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
