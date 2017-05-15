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
using System.ComponentModel;
using Model;

namespace ViewModel
{
    public enum Algorithm { BFS,DFS};
    public class SettingsViewModel : ViewModel
    {
        
        private ISettingsModel model;

        public int MazeCols
        {
            get { return model.MazeCols; }
            set {
                model.MazeCols = value;
                NotifyPropertyChanged("MazeCols");
            }
        }

        public int MazeRows
        {
            get { return model.MazeRows; }
            set
            {
                model.MazeRows = value;
                NotifyPropertyChanged("MazeRows");
            }
        }

        public int SearchAlgorithm
        {
            get { return model.SearchAlgorithm; }
            set
            {
                //switch()
            }
        }

        public int ServerPort
        {
            get { return model.ServerPort; }
            set {
                model.ServerPort = value;
                NotifyPropertyChanged("ServerPort");
            }
        }

        public string ServerIP
        {
            get { return model.ServerIP; }
            set
            {
                model.ServerIP = value;
                NotifyPropertyChanged("ServerIP");
            }
        }

        public SettingsViewModel(ISettingsModel model)
        {
            this.model = model;
        }

        /// <summary>
        /// Save default settings that the user applied.
        /// </summary>
        public void SaveSettings()
        {
            model.SaveSettings();
        }
    }
}
