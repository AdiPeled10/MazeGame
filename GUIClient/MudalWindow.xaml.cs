using System.Windows;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MudalWindow.xaml
    /// </summary>
    public partial class MudalWindow : Window
    {
        /// <summary>
        /// Method that will run when first button will be pressed.
        /// </summary>
        private RoutedEventHandler onFirstButton;

        /// <summary>
        /// Method that will run when second button will be pressed.
        /// </summary>
        private RoutedEventHandler onSecondButton;

        /// <summary>
        /// OnFirstButton property.
        /// Set "value" as "onFirstButton" and as a listener to firstButton.Click.
        /// </summary>
        public RoutedEventHandler OnFirstButton
        {
            set
            {
                onFirstButton = value;
                firstButton.Click += value;
            }
        }

        /// <summary>
        /// OnFirstButton property.
        /// Set "value" as "onSecondButton" and as a listener to secondButton.Click.
        /// </summary>
        public RoutedEventHandler OnSecondButton
        {
            set
            {
                onSecondButton = value;
                secondButton.Click += value;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MudalWindow()
        {
            InitializeComponent();
        }
    }
}
