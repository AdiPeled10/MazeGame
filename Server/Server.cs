using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Views;

namespace Server
{
    public class Server
    {
        private int port;
        private TcpListener listener;
        private IView view;
        private Thread accepter, clientService;

        public Server(int port, IView view)
        {
            this.port = port;
            this.view = view;
        }

        public void Start()
        {
            // creates a socket
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            // create server threads
            CreateAcceptingThread();
            CreateRequestReaderThread();

            // start listen to connection requests
            listener.Start();

            // tell the thread to start accepting
            accepter.Start();

            // tell the thread to start handling clients requests
            clientService.Start();
        }

        private void CreateAcceptingThread()
        {
            // create a thread that will accept new clients
            Thread thread = new Thread(() => {
                Console.WriteLine("Waiting for connections...");
                while (true)
                {
                    try
                    {
                        // accept a client
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");

                        // wrap it
                        MyTcpClient c = new MyTcpClient(client);

                        // add it to the view
                        view.AddClient(c);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });

            // set the thread as the accepter
            accepter = thread;
        }

        private void CreateRequestReaderThread()
        {
            // create a thread that will get clients requests and handle them
            Thread thread = new Thread(() => {
                //Thread.Sleep(100); // wait a little until a client will connect
                while (true)
                {
                    try
                    {
                        view.GetClientsRequests();
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine(e.ToString());
                    }
                }
            });

            // set the thread as the accepter
            clientService = thread;
        }

        public void Stop()
        {
            listener.Stop();
            accepter.Abort();
            clientService.Abort();
            accepter.Join();
            clientService.Join();
        }
    }
}
