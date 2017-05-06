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
using ViewModel;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for SinglePlayerWindow.xaml
    /// </summary>
    public partial class SinglePlayerWindow : Window
    {
        private TextBlock errorBox;

        private MazeViewModel vm;

        public TextBlock ErrorBox
        {
            get { return errorBox; }
            set { errorBox = value; }
        }
        
        public MazeInformationLayout Info
        {
            get { return mazeInfo; }
            set { mazeInfo = value; }
        }

        public StackPanel Stack
        {
            get { return stackPanel; }
        }

        public SinglePlayerWindow()
        {
            vm = new MazeViewModel();
            InitializeComponent();

        }

        /// <summary>
        /// This is the function that will run when the user will click the OK button in this window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.SinglePlayerOkButton(this);
        }
    }
}
