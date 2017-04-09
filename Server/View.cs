﻿using System;
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

        public void SendServerResponseTo(string res, IClient c);

        public void HandleClientRequest(string res, IClient c);

        public void GetClientRequests()
        {
            Clients(); // I may be using it wrong
        }

        public void AddClient(IClient c) // IClient might be using TCP or UDP
        {
            Clients += GenerateClientListener(c);
        }

        protected ClientListener GenerateClientListener(IClient c) // IClient might be using TCP or UDP
        {
            c.SetUnblocking(); // it's unsafe to this just once and not every time before "recv",
                               // but for now we'll pass on the safer overhead
            ClientListener f = delegate()
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
