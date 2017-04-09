using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate void EventHandler<TEventArgs>(IView sender, TEventArgs e);
    public delegate void ClientListener();

    class View : IView
    {
        private IController con;
        protected event ClientListener Clients;

        // perhapse we can somehow enforce the do this because its not natural that the controller will pass us a client
        public void SendServerResponseTo(string res, IClient c)
        {
            c.SendResponse(res);
        }

        public void HandleClientRequest(string res, IClient c)
        {

        }

        public void GetClientsRequests()
        {
            Clients(); // I may be using it wrong
        }

        public void AddClient(IClient c) // IClient might be using TCP or UDP
        {
            Clients += GenerateClientListener(c);
        }

        protected ClientListener GenerateClientListener(IClient c) // IClient might be using TCP or UDP
        {
            return delegate()
            {
                try
                {
                    string req = c.RecvARequest(); // should return 1 commend
                    this.HandleClientRequest(req, c);
                }
                catch (System.Net.Sockets.SocketException) // whatever it will throw if the socket is empty
                {
                }
                catch (System.ObjectDisposedException) // whatever it will throw if the socket is closed
                {
                    this.Clients -= this.GenerateClientListener(c);  //TODO hope really hard this will work.
                }
            };
        }
    }
}
