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
using System.ComponentModel;
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MultiPlayerWindow.xaml
    /// </summary>
    public partial class MultiPlayerWindow : Window,INotifyPropertyChanged
    {
        private string mazeName;
        private int rows;
        private int cols;
        private MultiPlayerWindowVM vm;

        public string MazeName
        {
            get { return mazeName; }
            set
            {
                if (mazeName != value)
                {
                    mazeName = value;
                    NotifyPropertyChanged("MazeName");
                }
            }
        }

        public int Rows
        {
            get { return rows; }
            set
            {
                if (rows != value)
                {
                    rows = value;
                    NotifyPropertyChanged("Rows");
                }
            }
        }

        public int Cols
        {
            get { return cols; }
            set
            {
                if (cols != value)
                {
                    cols = value;
                    NotifyPropertyChanged("Cols");
                }
            }
        }

        public MultiPlayerWindow()
        {
            vm = new MultiPlayerWindowVM();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Open multi player maze.
            MazeName = maze.nameTextBox.Text;
            MultiPlayerMaze mazeWindow = new MultiPlayerMaze();
            mazeWindow.Rows = 20;
            mazeWindow.Cols = 20;
            vm.OpenMenu(this, mazeWindow);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
