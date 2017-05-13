using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Views;

namespace Server
{
    /// <summary>
    /// This is the class which implements our Server which handle all of the clients
    /// and uses the View while handling them.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// The port of this server.
        /// </summary>
        private int port;
        /// <summary>
        /// The TcpListener that we will use in order to listen to clients.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// The view that the server is going to use to handle client requests.
        /// </summary>
        private IView view;
        
        /// <summary>
        /// Threads to accept clients and service them while letting the main thread run properly.
        /// </summary>
        private Thread accepter, clientService;

        /// <summary>
        /// Constructor of the Server class.
        /// </summary>
        /// <param name="port">
        /// The port of the server.
        /// </param>
        /// <param name="view">
        /// The view that the server will use to handle client requests.
        /// </param>
        public Server(int port, IView view)
        {
            this.port = port;
            this.view = view;
        }

        /// <summary>
        /// Start the server and start listening to clients.
        /// </summary>
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
            Console.WriteLine("Started acceptor");
            accepter.Start();

            // tell the thread to start handling clients requests
            Console.WriteLine("Started client service");
            clientService.Start();
        }

        /// <summary>
        /// Create the thread that will be used to accept
        /// clients while letting the main thread run.
        /// </summary>
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
                        //Mutex mut = new Mutex();
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");

                        // wrap it
                        MyTcpClient c = new MyTcpClient(client);

                        // add it to the view
                        //mut.WaitOne();
                        view.AddClient(c);
                      //  mut.ReleaseMutex();
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

        /// <summary>
        /// Create the thread which reads client's requests while letting the main thread to run.
        /// </summary>
        private void CreateRequestReaderThread()
        {
            // create a thread that will get clients requests and handle them
            Thread thread = new Thread(() => {
                //Thread.Sleep(100); // wait a little until a client will connect
                while (true)
                {
                    try
                    {
                        //Console.WriteLine("Get client requests.");
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

        /// <summary>
        /// Stop the communication of the server.
        /// </summary>
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
