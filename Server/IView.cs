using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IView
    {
        IController cont;
        event ClientListener Clients;

        void SendServerResponseTo(string res, Iclient c);
        void HandleClientRequest(string res, Iclient c);
        string GetAClientRequest();
        void AddClient(IClient c); // IClient might be using TCP or UDP
    }
}
