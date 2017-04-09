using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IView
    {
        void SendServerResponseTo(string res, IClient c);
        void HandleClientRequest(string res, IClient c);
        void GetClientsRequests();
        void AddClient(IClient c); // IClient might be using TCP or UDP
    }
}
