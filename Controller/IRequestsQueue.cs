using System;
using ClientForServer;

namespace Controller
{
    public interface IRequestsQueue
    {
        void Add(Action request, IClient client);

        void Remove(IClient client);
    }
}
