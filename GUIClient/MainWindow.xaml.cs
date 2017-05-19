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
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;

        public MainWindow()
        {
            vm = new MainWindowViewModel();
            InitializeComponent();
            //TODO set data context to be viewmodel all data binding goes through there.
        }

        public void SinglePlayerClick(object sender,RoutedEventArgs args) 
        {
            SinglePlayerWindow window = new SinglePlayerWindow();
            window.Show();
            this.Close();
        }

        private void MultiPlayerClick(object sender,RoutedEventArgs args)
        {
            MultiPlayerWindow window = new MultiPlayerWindow();
            window.Show();
            this.Close();
        }

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
