using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using ClientForServer;

namespace Controllers
{
    /// <summary>
    /// This internal class will save a pair of a task and it's cancellation token
    /// which will stop the task when we wish to stop it.
    /// </summary>
    internal class TaskTokenPair
    {
        /// <summary>
        /// Constructor of the TaskTokenPair which is given the task
        /// and the cancellation token.
        /// </summary>
        /// <param name="task">
        /// The task which will be saved.
        /// </param>
        /// <param name="tokenSource">
        /// The cancellation token for this class.
        /// </param>
        public TaskTokenPair(Task task, CancellationTokenSource tokenSource)
        {
            this.task = task;
            this.tokenSource = tokenSource;
        }
        /// <summary>
        /// The saved task for this pair.
        /// </summary>
        public Task task;

        /// <summary>
        /// The saved CancellationTokenSource for this pair.
        /// </summary>
        public CancellationTokenSource tokenSource;
    }

    /// <summary>
    /// This RequestsQueue will implement the IRequestQueue interface to handle
    /// all the requests the server is going to get in a queue.
    /// </summary>
    public class RequestsQueue : IRequestsQueue
    {
        /// <summary>
        /// A dictionary that matches each Client to it's TaskTokenPair.
        /// </summary>
        private Dictionary<IClient, TaskTokenPair> clientToLastRequest;

        /// <summary>
        /// Constructor of the request queue.
        /// </summary>
        public RequestsQueue()
        {
            clientToLastRequest = new Dictionary<IClient, TaskTokenPair>(32);
        }

        /// <summary>
        /// Add a request as an action and a client to this queue.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when there is no matching key in the dictionary because
        /// the given client is a new client.
        /// </exception>
        /// <param name="client">
        /// The client that sent this request.
        /// </param>
        public void Add(Action request, IClient client)
        {
            
            Task previousRequest;
            try
            {
                // Get the previous request
                previousRequest = clientToLastRequest[client].task;
                /** 
                 * Set a continuation task to the previous request (it will be executed only when
                 * "previousRequest" is done) and set the previous request as the new continuation task.
                 */
                clientToLastRequest[client].task = previousRequest.ContinueWith(
                    ParameterWeWontUse => request(),
                    clientToLastRequest[client].tokenSource.Token);
            }
            catch (KeyNotFoundException)
            {
                // This is the client first time in this queue.
                // create cancellation token
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                // create his first Task
                previousRequest = new Task(request, tokenSource.Token);
                // start his request handling
                previousRequest.Start();
                // set the task in the dictionary
                clientToLastRequest[client] = new TaskTokenPair(previousRequest, tokenSource);
            }
        }

        /// <summary>
        /// Remove the given client from the queue.
        /// </summary>
        /// <param name="client">
        /// The client that we wish to remove.
        /// </param>
        public void Remove(IClient client)
        {
            clientToLastRequest[client].tokenSource.Cancel();
            clientToLastRequest.Remove(client);
        }
    }
}
