using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IRequestsQueue
    {
        void Add(Action request, IClient client);
        void Remove(IClient client);
    }
}
