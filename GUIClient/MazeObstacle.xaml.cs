using System;
using System.Windows.Controls;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MazeObstacle.xaml
    /// </summary>
    public partial class MazeObstacle : UserControl
    {
        /// <summary>
        /// A member just to hold the UserControl(for binding purposes and such).
        /// </summary>
        private UserControl control;

        /// <summary>
        /// Control property.
        /// Get or set "control" member.
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
        /// Constructor.
        /// </summary>
        public MazeObstacle()
        {
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

        /// <summary>
        /// In our string representation of the maze an obstacle
        /// is represented by 1.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "1";
        }

    }
}
