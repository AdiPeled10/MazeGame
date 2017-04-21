namespace ClientForServer
{
    /// <summary>
    /// An interface for all the classes that can be notified for some events.
    /// </summary>
    public interface ICanbeNotified
    {
        /// <summary>
        /// Notify with a message.
        /// </summary>
        /// <param name="message">
        /// The notificiation as a string.
        /// </param>
        void Notify(string message);
    }
}
