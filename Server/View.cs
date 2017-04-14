using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public delegate void ClientListener();

    public class View : IView
    {
        protected IController con;
        protected event ClientListener Clients;

        public View(IController con)
        {
            this.con = con;

        }

        // perhapse we can somehow enforce the do this because its not natural that the controller will pass us a client
        public void SendServerResponseTo(string res, IClient c)
        {
            c.SendResponse(res);
        }

        public void HandleClientRequest(string req, IClient c)
        {
            con.ExecuteCommand(req, c);
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
                    if (c.HasARequest())
                    {
                        string req = c.RecvARequest(); // should return 1 request
                        this.HandleClientRequest(req, c);
                    }
                }
                catch (System.ObjectDisposedException) // The client has disconnected
                {
                    con.DisconnectClient(c);
                    this.Clients -= this.GenerateClientListener(c);  //TODO hope really hard this will work.
                }
            };
        }
    }
}
