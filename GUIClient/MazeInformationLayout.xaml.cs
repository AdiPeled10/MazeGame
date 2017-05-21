using System;
using System.Windows.Controls;

namespace GUIClient
{
    /// <summary>
    /// Interaction logic for MazeInformationLayout.xaml
    /// </summary>
    public partial class MazeInformationLayout : UserControl
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MazeInformationLayout()
        {
            InitializeComponent();
        }

        /// <summary>
        /// NameBox property.
        /// Returns the text in "nameTextBox" member(in the xaml).
        /// </summary>
        public string NameBox
        {
            get { return nameTextBox.Text; }
        }

        /// <summary>
        /// Rows property.
        /// Returns the text in "rowsTextBox" member(in the xaml) as int(or 0 if parsing fails).
        /// </summary>
        public int Rows
        {
            get
            {
                try
                {
                    return Int32.Parse(rowsTextBox.Text);
                } catch(Exception)
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Cols property.
        /// Returns the text in "columnsTextBox" member(in the xaml) as int(or 0 if parsing fails).
        /// </summary>
        public int Cols
        {
            get
            {
                try
                {
                    return Int32.Parse(columnsTextBox.Text);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
    }
}
