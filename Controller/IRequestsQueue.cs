using System;
using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// This interface represents the functions that we expect each Requests queue to have which
    /// is basically to add a request from a client and remove a request.
    /// </summary>
    public interface IRequestsQueue
    {
        /// <summary>
        /// Add the given request as an Action which is basically a delegate
        /// and the client which requested it.
        /// </summary>
        /// <param name="request">
        /// The request that was sent.
        /// </param>
        /// <param name="client">
        /// The client that sent this request.
        /// </param>
        void Add(Action request, IClient client);

        /// <summary>
        /// Remove the client and all of his requests from the queue.
        /// </summary>
        /// <param name="client"></param>
        void Remove(IClient client);
    }
}
