using System.ComponentModel;

namespace ViewModel
{
    /// <summary>
    /// Abstract class that all view model classes will implement because all 
    /// of them will use the PropertyChanged event for data binding with the view.
    /// </summary>
    public abstract class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify listeners that some property was changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke "PropertyChanged" given the name of the changed property.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName) {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
