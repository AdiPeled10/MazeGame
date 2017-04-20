using System;
using ClientForServer;

namespace Controllers
{
    public interface IRequestsQueue
    {
        void Add(Action request, IClient client);

        void Remove(IClient client);
    }
}
