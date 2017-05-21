using System;
using System.Windows.Controls;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MazeFreePass.xaml
    /// </summary>
    public partial class MazeFreePass : UserControl
    {
        /// <summary>
        /// UserControl memebr which will draw to the screen ot use in the xaml.
        /// </summary>
        private UserControl control;

        /// <summary>
        /// Control property.
        /// UserControl memebr which will draw to the screen ot use in the xaml.
        /// </summary>
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

        /// <summary>
        /// Initialize the class(as a constructor would).
        /// </summary>
        /// <param name="e"> used as argument to the base class "OnInitialized". </param>
        protected override void OnInitialized(EventArgs e)
        {
            InitializeComponent();
            base.OnInitialized(e);
        }
    }
}
