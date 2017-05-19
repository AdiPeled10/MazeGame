using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerWindow.xaml
    /// </summary>
    public partial class MultiPlayerWindow : Window
    {
        private string mazeName;
        private int rows;
        private int cols;

        /// <summary>
        /// View model of the multi player game.
        /// </summary>
        private MultiPlayerVM vm;

        /// <summary>
        ///  True if we can connect to the server false otherwise.
        /// </summary>
        private bool connected = true;

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
            vm.NotifyConnection += OpponentConnected;
            vm.CantFindServer += () =>
            {
                CantConnect win = new CantConnect();
                win.Show();
                connected = false;
            };
            DataContext = vm;
            vm.Connect();
        }


        /// <summary>
        /// Event which will occur when  start game button will be clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, RoutedEventArgs e)
        {

            //Start game via the view model.
            
            if (!connected)
            {
                //Cant connect to server.
                return;
            }

            waitingBlock.Text = "Waiting for opponent...";
            vm.GenerateMaze(maze.NameBox, maze.Rows, maze.Cols);
        }

        public void OpponentConnected()
        {
            Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                     new Action(() => {
                        
                         MultiPlayerMaze mazeWindow = new MultiPlayerMaze();
                         //Pass on the view model.
                         mazeWindow.VM = vm;
                         mazeWindow.Show();
                         this.Close();
                     }));
        }

      

        private void JoinClick(object sender, RoutedEventArgs e)
        {
            vm.JoinGame(comboBox.Text);
        }

        /// <summary>
        /// Each time drop down is clicked ask for list from server.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DropDownHandler(object sender,EventArgs e)
        {
            vm.ListCommand();
        } 
    }
}
