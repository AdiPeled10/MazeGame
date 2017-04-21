using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientForServer;
using Controllers;

namespace Views
{
    /// <summary>
    /// This delegate represents all the functions that we will use as listeners
    /// to the clients.
    /// </summary>
    public delegate void ClientListener();

    /// <summary>
    /// The view class implements the IView interface which holds all the methods that we
    /// expect the View to implement in order to implement the MVC architectural pattern.
    /// </summary>
    public class View : IView
    {
        /// <summary>
        /// The controller that our application uses.
        /// </summary>
        protected IController con;

        /// <summary>
        /// The event which is raised when we get notified by the clients.
        /// </summary>
        protected event ClientListener Clients;

        /// <summary>
        /// This class helps us close the clients properly.
        /// </summary>
        private class PointerToClientListener
        {
            /// <summary>
            /// A client listener that points to the given client.
            /// </summary>
            public ClientListener pointer;
        }

        /// <summary>
        /// Constructor of the View class that gets the Controller.
        /// </summary>
        /// <param name="con">
        /// The Controller that we use.
        /// </param>
        public View(IController con)
        {
            this.con = con;
        }


        /// <summary>
        /// Send the server's response to the given client.
        /// TODO  Perhapse we can somehow enforce the do
        /// this because its not natural that the controller will pass us a client.
        /// </summary>
        /// <param name="res"></param>
        /// <param name="c"></param>
        public void SendServerResponseTo(string res, IClient c)
        {
            c.SendResponse(res);
        }

        /// <summary>
        /// Handle the client's request.
        /// </summary>
        /// <param name="req">
        /// The request of the client.
        /// </param>
        /// <param name="c">
        /// The client that sent this request.
        /// </param>
        public void HandleClientRequest(string req, IClient c)
        {
            con.ExecuteCommand(req, c);
        }
        /// <summary>
        /// Get all of the client's requests.
        /// </summary>
        public void GetClientsRequests()
        {
            Clients(); 
        }

        /// <summary>
        /// Add the given client to our server.Specifically it's listener.
        ///  IClient might be using TCP or UDP
        /// </summary>
        /// <param name="c">
        /// The client that we will add.
        /// </param>
        public void AddClient(IClient c) 
        {
            Clients += GenerateClientListener(c);
        }

        /// <summary>
        /// Generate a ClientListener function for the given client.
        /// </summary>
        /// <exception>
        /// Catches an IOException when the client is disconnected.
        /// </exception>
        /// <param name="c">
        /// The client that we generate a listener for.
        /// </param>
        /// <returns>
        /// The delegate of the ClientListener.
        /// </returns>
        protected ClientListener GenerateClientListener(IClient c) 
        {
            PointerToClientListener thisClient = new PointerToClientListener();
            ClientListener clientListener = delegate()
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
                    this.Clients -= thisClient.pointer;  //TODO hope really hard this will work.
                }
            };
            thisClient.pointer = clientListener;
            return clientListener;
        }
    }
}
