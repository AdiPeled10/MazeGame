using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using ClientForServer;

namespace Controllers
{
    internal class TaskTokenPair
    {
        public TaskTokenPair(Task task, CancellationTokenSource tokenSource)
        {
            this.task = task;
            this.tokenSource = tokenSource;
        }
        public Task task;
        public CancellationTokenSource tokenSource;
    }

    public class RequestsQueue : IRequestsQueue
    {
        private Dictionary<IClient, TaskTokenPair> clientToLastRequest;

        public RequestsQueue()
        {
            clientToLastRequest = new Dictionary<IClient, TaskTokenPair>(32);
        }

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
                clientToLastRequest[client].task = previousRequest.ContinueWith(ParameterWeWontUse => request(),
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

        public void Remove(IClient client)
        {
            clientToLastRequest[client].tokenSource.Cancel();
            clientToLastRequest.Remove(client);
        }
    }
}
