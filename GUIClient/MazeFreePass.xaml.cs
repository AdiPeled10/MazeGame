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
    /// Interaction logic for MazeFreePass.xaml
    /// </summary>
    public partial class MazeFreePass : UserControl
    {
        private UserControl control;

        public UserControl Control
        {
            set
            {
                control = value;
                Content = value;
            }
            get { return control; }
        }

        /// <summary>
        /// We want the representation of the maze to be as generic as possible,
        /// that's why the MazeFreePass and MazeObstacle are both user controls that build
        /// the maze and they can be any user control that we want.
        /// TODO-Maybe because of size limitations we need to add a ViewBox to add boundaries
        /// to the user control.
        /// </summary>
        public MazeFreePass()
        {
        }

        /// <summary>
        /// In our representation of the maze a location which is a FreePass
        /// meaning the player can pass through it is represented by 0.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "0";
        }

        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            base.OnInitialized(e);
        }
    }
}
