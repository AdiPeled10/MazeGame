using Model;

namespace ViewModel
{
    /// <summary>
    /// Enum for the search algorithms(for solutions).
    /// </summary>
    public enum Algorithm {BFS, DFS};

    /// <summary>
    /// This class contains default values to fields. They are used when
    /// they aren't given by the user.
    /// </summary>
    public class SettingsViewModel : ViewModel
    {
        /// <summary>
        /// The model is used to be notified when a setting is changed.
        /// </summary>
        private ISettingsModel model;

        /// <summary>
        /// MazeCols propery.
        /// Gets\sets the default numbers of columns in a maze.
        /// If set, also calls "NotifyPropertyChanged".
        /// </summary>
        public int MazeCols
        {
            get { return model.MazeCols; }
            set {
                model.MazeCols = value;
                NotifyPropertyChanged("MazeCols");
            }
        }

        /// <summary>
        /// MazeRows propery.
        /// Gets\sets the default numbers of rows in a maze.
        /// If set, also calls "NotifyPropertyChanged".
        /// </summary>
        public int MazeRows
        {
            get { return model.MazeRows; }
            set
            {
                model.MazeRows = value;
                NotifyPropertyChanged("MazeRows");
            }
        }

        /// <summary>
        /// SearchAlgorithm propery.
        /// Gets\sets the default search algorithm when solving a maze.
        /// If set, also calls "NotifyPropertyChanged".
        /// </summary>
        public int SearchAlgorithm
        {
            get { return model.SearchAlgorithm; }
            set
            {
                if (value == (int)Algorithm.BFS || value == (int)Algorithm.DFS)
                {
                    model.SearchAlgorithm = value;
                    NotifyPropertyChanged("SearchAlgorithm");
                }
            }
        }

        /// <summary>
        /// ServerPort propery.
        /// Gets\sets the default server port.
        /// If set, also calls "NotifyPropertyChanged".
        /// </summary>
        public int ServerPort
        {
            get { return model.ServerPort; }
            set {
                model.ServerPort = value;
                NotifyPropertyChanged("ServerPort");
            }
        }

        /// <summary>
        /// ServerIP propery.
        /// Gets\sets the default server IP.
        /// If set, also calls "NotifyPropertyChanged".
        /// </summary>
        public string ServerIP
        {
            get { return model.ServerIP; }
            set
            {
                model.ServerIP = value;
                NotifyPropertyChanged("ServerIP");
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="model"></param>
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
