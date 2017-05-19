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

        public RoutedEventHandler OnFirstButton
        {
            set
            {
                onFirstButton = value;
                firstButton.Click += value;
            }
        }

        public RoutedEventHandler OnSecondButton
        {
            set
            {
                onSecondButton = value;
                secondButton.Click += value;
            }
        }

        public MudalWindow()
        {
            InitializeComponent();
        }
    }
}
