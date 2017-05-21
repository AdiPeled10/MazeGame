using System.Windows;
using System.Reflection;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This is a part of the "View" component in the "MVVM" standard.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //ODO set data context to be viewmodel all data binding goes through there.
        }

        /// <summary>
        /// Closes this window and opens a single player window.
        /// </summary>
        /// <param name="sender"> An object. Not meaningful. </param>
        /// <param name="args"> Not meaningful. </param>
        public void SinglePlayerClick(object sender,RoutedEventArgs args) 
        {
            SinglePlayerWindow window = new SinglePlayerWindow();
            window.Show();
            this.Close();
        }

        /// <summary>
        /// Closes this window and opens a nulti player window.
        /// </summary>
        /// <param name="sender"> An object. Not meaningful. </param>
        /// <param name="args"> Not meaningful. </param>
        private void MultiPlayerClick(object sender,RoutedEventArgs args)
        {
            MultiPlayerWindow window = new MultiPlayerWindow();
            window.Show();
            this.Close();
        }

        /// <summary>
        /// Closes this window and opens the setting window.
        /// </summary>
        /// <param name="sender"> An object. Not meaningful. </param>
        /// <param name="args"> Not meaningful. </param>
        private void SettingsClick(object sender,RoutedEventArgs args)
        {
            try
            {
                SettingsWindow window = new SettingsWindow();
                window.Show();
                this.Close();
            } catch (TargetInvocationException) { }
        }
    }
}
