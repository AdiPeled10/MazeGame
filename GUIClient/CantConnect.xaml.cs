using System.Windows;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for CantConnect.xaml
    /// </summary>
    public partial class CantConnect : Window
    {
        /// <summary>
        /// Constructor.
        /// Most logic is in the XAML file.
        /// </summary>
        public CantConnect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handler to when the "back" button is pressed.
        /// It will close this window and open the main window.
        /// </summary>
        /// <param name="sender"> An object. Not meaningful. </param>
        /// <param name="e"> Not meaningful. </param>
        private void BackButton(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}
