using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUIClient;
using ViewModel;

namespace Model
{
    /// <summary>
    /// Save default settings entered in settings window.
    /// </summary>
     public class ApplicationSettingsModel : ISettingsModel
    {
        /// <summary>
        /// Save IP of the server in default property.
        /// </summary>
        public string ServerIP {
            get { return GUIClient.Properties.Settings.Default.ServerIP; }
            set { GUIClient.Properties.Settings.Default.ServerIP = value; }
        }

        /// <summary>
        /// Save port of the server in default property.
        /// </summary>
        public int ServerPort {
            get { return GUIClient.Properties.Settings.Default.ServerPort; }
            set { GUIClient.Properties.Settings.Default.ServerPort = value; }
        }

        /// <summary>
        /// Save number of rows of the maze of the server in default property.
        /// </summary>
        public int MazeRows
        {
            get { return GUIClient.Properties.Settings.Default.MazeRows; }
            set {GUIClient.Properties.Settings.Default.MazeRows = value; }
        }

        /// <summary>
        /// Save number of columns of the maze of the server in default property.
        /// </summary>
        public int MazeCols
        {
            get { return GUIClient.Properties.Settings.Default.MazeCols; }
            set {GUIClient.Properties.Settings.Default.MazeCols = value; }
        }

        /// <summary>
        /// Save identifier for searching algorithm in default property.
        /// </summary>
        public int SearchAlgorithm
        {
            get { return GUIClient.Properties.Settings.Default.SearchAlgorithm; }
            set {
                GUIClient.Properties.Settings.Default.SearchAlgorithm = value;
            }
        }

        public void SaveSettings() { GUIClient.Properties.Settings.Default.Save(); }
    }
}
