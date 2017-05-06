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
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MazeViewModel vm;

        public MainWindow()
        {
            vm = new MazeViewModel();
            InitializeComponent();
            //TODO set data context to be viewmodel all data binding goes through there.
        }

        public void SinglePlayerClick(object sender,RoutedEventArgs args) 
        {
            vm.OpenMenu(this,new SinglePlayerWindow());
            //this.Close();
        }

        private void MultiPlayerClick(object sender,RoutedEventArgs args)
        {
            vm.OpenMenu(this, new MultiPlayerWindow());
        }

        private void SettingsClick(object sender,RoutedEventArgs args)
        {
            vm.OpenMenu(this, new SettingsWindow());
        }

    }
}
