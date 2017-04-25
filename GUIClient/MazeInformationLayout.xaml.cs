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

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MazeInformationLayout.xaml
    /// </summary>
    public partial class MazeInformationLayout : UserControl
    {
        public MazeInformationLayout()
        {
            InitializeComponent();
        }

        public string NameBox
        {
            get { return nameTextBox.Text; }
        }

        public int Rows
        {
            get { return Int32.Parse(rowsTextBox.Text); }
        }

        public int Cols
        {
            get { return Int32.Parse(columnsTextBox.Text); }
        }
    }
}
