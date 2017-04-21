using ClientForServer;

namespace Views
{
    /// <summary>
    /// This interface has the set of abilities that we expect from the View to have while
    /// implementing the MVC architectural pattern for our application.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Send the reponse of the server to the given client.
        /// </summary>
        /// <param name="res">
        /// The server's response.
        /// </param>
        /// <param name="c">
        /// The client that we wish to send the response to.
        /// </param>
        void SendServerResponseTo(string res, IClient c);

        /// <summary>
        /// Handle the request of the given client.
        /// </summary>
        /// <param name="res">
        /// The client's request.
        /// </param>
        /// <param name="c">
        /// The client which sent the request.
        /// </param>
        void HandleClientRequest(string res, IClient c);

        /// <summary>
        /// Get all of the client's request.
        /// </summary>
        void GetClientsRequests();

        /// <summary>
        /// Add the given IClient to our server.
        /// IClient might be using TCP or UDP
        /// </summary>
        /// <param name="c">
        /// The IClient that we will add.
        /// </param>
        void AddClient(IClient c);
    }
}
