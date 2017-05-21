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
using System.Reflection;
using ViewModel;
using Model;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// The ViewModel. This class sets the "setting" of "vm".
        /// </summary>
        private SettingsViewModel vm;

        /// <summary>
        /// VM property.
        /// Get or set "vm" member.
        /// </summary>
        public SettingsViewModel VM
        {
            get { return vm; }
            set { vm = value; }
        }

        /// <summary>
        /// Constructor.
        /// Creares "vm" as SettingViewModel.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            vm = new SettingsViewModel(new ApplicationSettingsModel());
            try
            {
                DataContext = vm;
            } catch (TargetInvocationException)
            {

            }
        }

        /// <summary>
        /// Handles the event of the user clicking the cancel button.
        /// It closes this window and opens the main window(MainWindow).
        /// </summary>
        /// <param name="sender"> Meaningles. </param>
        /// <param name="e"> Meaningles. </param>
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        /// <summary>
        /// Handles the event of the user clicking the "save setting" button.
        /// It saves the setting, closes this window and opens the main
        /// window(MainWindow).
        /// </summary>
        /// <param name="sender"> Meaningles. </param>
        /// <param name="e"> Meaningles. </param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            vm.SaveSettings();
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
