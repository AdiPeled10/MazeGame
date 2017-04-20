using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientForServer;
using Controllers;

namespace Views
{
    public delegate void ClientListener();

    public class View : IView
    {
        protected IController con;
        protected event ClientListener Clients;

        public View(IController con)
        {
            this.con = con;
            this.Clients = () => { }; // To avoid exceptions while it's empty
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
        {// throws NullException when empty
            Clients(); // I may be using it wrong
        }

        public void AddClient(IClient c) // IClient might be using TCP or UDP
        {
            Clients += GenerateClientListener(c);
        }

        protected ClientListener GenerateClientListener(IClient c) // IClient might be using TCP or UDP
        {
            int placeAt = this.Clients.GetInvocationList().Count();
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
                catch (System.IO.IOException) // The client has disconnected
                {
                    try
                    {
                        con.DisconnectClient(c);
                    }
                    catch
                    {
                        // the client is new and have no resources
                    }
                    this.Clients -= (ClientListener)Clients.GetInvocationList()[placeAt];//this.GenerateClientListener(c);  //TODO hope really hard this will work.
                }
            };
        }
    }
}
