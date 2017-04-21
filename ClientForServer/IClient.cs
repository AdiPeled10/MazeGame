namespace ClientForServer
{
    /// <summary>
    /// This interface will hold the set of abilities that we expect from a client that communicates
    /// with our server.It will implement the ICanBeNotified interface because we expect from him to be
    /// able to get notified of events.
    /// </summary>
    public interface IClient : ICanbeNotified
    {

        /// <summary>
        /// Tells us if the client has a request.
        /// </summary>
        /// <returns>
        /// True if he does, false otherwise.
        /// </returns>
        bool HasARequest();

        /// <summary>
        /// Receive a request from the client.
        /// Every request will be separated by a special char, maybe 0b1111 11111
        /// </summary>
        /// <returns>
        /// The request as a string.
        /// </returns>
        string RecvARequest();  

        /// <summary>
        /// Send a response to the client.
        /// </summary>
        /// <param name="res">
        /// The response that we will send as a string.
        /// </param>
        void SendResponse(string res);

        /// <summary>
        ///  Removes allocated resources relating the client communication.
        /// </summary>
        void Disconnect();

    }
}
