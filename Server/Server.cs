using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using View;

namespace Server
{
    public class Server
    {
        private int port;
        private TcpListener listener;
        private IView view;

        public Server(int port, IView view)
        {
            this.port = port;
            this.view = view;
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine("Waiting for connections...");
            Task task = new Task(() => {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        MyTcpClient c = new MyTcpClient(client);
                        view.AddClient(c);
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            task.Start();
        }

        public void Stop()
        {
            listener.Stop();
        }
    }
}
