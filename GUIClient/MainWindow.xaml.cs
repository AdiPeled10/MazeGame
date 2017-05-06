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
        IViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            //TODO set data context to be viewmodel all data binding goes through there.
        }

        public void SinglePlayerClick(object sender,RoutedEventArgs args) 
        {
            /* Window popupWindow = new Window();
             //Set width and height of the window to be medium sized.
             popupWindow.Width = 400;
             popupWindow.Height = 400;
             //Create StackPanel layout to hold Name,Rows,Cols of generated maze.
             StackPanel myStackPanel = new StackPanel();
             myStackPanel.Children.Add(new MazeInformationLayout());
             //myStackPanel.Children.Add(GenerateTextBox("Name:"));
             //myStackPanel.Children.Add(GenerateTextBox("Number of Rows:"));
             //myStackPanel.Children.Add(GenerateTextBox("Number of Columns:"));
             //Add Ok button.
             myStackPanel.Children.Add(new Button { Width = 40,Content = "OK",
                 Height = 40, Margin = new Thickness(50)});
             popupWindow.Content = myStackPanel;*/
            SinglePlayerWindow popupWindow = new SinglePlayerWindow();
            popupWindow.Show();
            this.Close();
        }

        /// <summary>
        /// This function will generate a label and a TextBox next to it in
        /// a StackPanel layout.
        /// </summary>
        /// <param name="nameOfField">
        /// The name of the label.
        /// </param>
        /// <returns></returns>
        private StackPanel GenerateTextBox(string nameOfField)
        {
            StackPanel myStackPanel = new StackPanel { Orientation = Orientation.Vertical };
            myStackPanel.Children.Add(new Label { Content = nameOfField });
            myStackPanel.Children.Add(new TextBox());
            return myStackPanel;
        }

        private void MultiPlayerClick(object sender,RoutedEventArgs args)
        {
            MultiPlayerWindow window = new MultiPlayerWindow();
            window.Show();
        }

        private void SettingsClick(object sender,RoutedEventArgs args)
        {
            SettingsWindow window = new SettingsWindow();
            window.Show();
            this.Close();
        }

    }
}
