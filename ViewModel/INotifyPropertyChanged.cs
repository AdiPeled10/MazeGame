using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    /// <summary>
    /// This is the delegate for the PropertyChangedEventHandler which will
    /// represent all the functions that will be activated when a property of
    /// the ViewModel will be changed to act as an observable and use the Data
    /// Binding to change the view by using this delegate.
    /// </summary>
    /// <param name="sender">
    /// The observer that changed the property.
    /// </param>
    /// <param name="e">
    /// All the relevant arguments,which property was changed,to which value it
    /// was changed and more.
    /// </param>
    public delegate void PropertyChangedEventHandler(
        object sender, PropertyChangedEventArgs e);
    
    /// <summary>
    /// A class I created for now to make the PropertyChangedEventHandler have his own
    /// arguments which we can expand later.
    /// </summary>
    public class PropertyChangedEventArgs : EventArgs { }

    /// <summary>
    /// The INotifyPropertyChanged interface will represent all classes that we want
    /// to notify the view when a property of this class was changed during run time to
    /// implement basically the observer design pattern and make each property of a class
    /// that implements the INotifyPropertyChanged interface be observable.
    /// </summary>
    public interface INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}
