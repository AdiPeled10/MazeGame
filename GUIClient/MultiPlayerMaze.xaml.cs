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
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using ViewModel;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerMaze.xaml
    /// </summary>
    public partial class MultiPlayerMaze : Window
    {
        private int rows;
        private int cols;
        private MultiPlayerVM vm;


        public MultiPlayerVM VM
        {
            get { return vm; }
            set
            {
                vm = value;
                DataContext = vm;
                vm.Movement += otherMaze.MovePlayer;
                vm.NotifyDisconnection += OpponentDisconnected;
            }
        }


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
            set
            {
                cols = value;
            }
        }


        


        public MultiPlayerMaze()
        {
            InitializeComponent();
            myMaze.Done += GameDone;
            otherMaze.Done += GameDone;
            KeyDown += SendPlay; 
            ListenToOpponentMoves();

        }

        public void OpponentDisconnected()
        {
            Application.Current.Dispatcher.BeginInvoke(
                   DispatcherPriority.Background,
                    new Action(() => {
                        otherBox.Text = "Your opponent has left the game.";
                        otherBox.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Red);
                        }));
        }

        /// <summary>
        /// Check if window was closed through the code with Close method or from
        /// UI if it's from the code it's fine otherwise opponent left the game,send
        /// correct message to server.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            bool wasCodeClosed = new StackTrace().GetFrames().FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed)
            {
                // Closed some other way.Send exit.
                vm.Disconnected();
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// Notify the view model that the game is over.
        /// </summary>
        public void GameDone()
        {
            vm.GameOver();
            this.Close();
        }

        public void ListenToOpponentMoves()
        {
            Thread thread = new Thread(() => { vm.ListenToOpponent(); });
            Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                     new Action(() => { thread.Start(); }));
        }

        /// <summary>
        /// Activated when user clicks back to main menu,we will send a message to the server
        /// that notifies that we need to notify the other player of disconnection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Notify that player got disconnected.
            vm.Disconnected();
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void SendPlay(object sender, KeyEventArgs e)
        {
            //Send to view model.
            string strKey = e.Key.ToString().ToLower();
            switch (strKey)
            {
                case "up":
                case "down":
                case "left":
                case "right":
                    {
                        vm.PlayInDirection(strKey);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
