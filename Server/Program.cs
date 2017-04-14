using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SearchAlgorithmsLib;
using MazeLib;
using Newtonsoft.Json.Linq;
using MazeGeneratorLib;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource tokenSource2 = new CancellationTokenSource();
            Task t = new Task(() => Console.WriteLine("First")), tcopy;
            tcopy = t;
            //tcopy.Start();
            Thread.Sleep(1000);
            t = t.ContinueWith(dummy => Console.WriteLine("Second"), tokenSource2.Token);
            t = t.ContinueWith(dummy => Console.WriteLine("Third"), tokenSource2.Token);
            t = t.ContinueWith(dummy => Console.WriteLine("Fourth"), tokenSource2.Token);
            t = t.ContinueWith(dummy => Console.WriteLine("Fifth"));
            t = t.ContinueWith(dummy => Console.WriteLine("Sixth"), tokenSource2.Token);
            //return;

            tcopy.Start();
            tokenSource2.Cancel();

            Task<string> strTask = new Task<string>(() => "First "), strTaskCopy;
            strTaskCopy = strTask;
            strTask = strTask.ContinueWith(str => str.Result + "Second ");
            strTask = strTask.ContinueWith(str => str.Result + "Third ");
            strTask = strTask.ContinueWith(str => str.Result + "Fourth ");
            strTask = strTask.ContinueWith(str => str.Result + "Fifth ");
            strTask = strTask.ContinueWith(str => str.Result + "Sixth");
            //tcopy.Start();
            strTaskCopy.Start();
            Console.WriteLine(strTask.Result);
            Console.ReadKey();
            return;


            // just some code from the slides of Tirgul 3
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpListener listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Waiting for client connections...");
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected");
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                Console.WriteLine("Waiting for a number");
                int num = reader.ReadInt32();
                Console.WriteLine("Number accepted");
                num *= 2;
                writer.Write(num);
            }

            client.Close();
            listener.Stop();
        }
    }
}
