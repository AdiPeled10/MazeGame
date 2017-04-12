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

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string name = "Adi Peled";
            DFSMazeGenerator gen = new DFSMazeGenerator();
            Maze maze = gen.Generate(10, 10);

            JObject mazeObj = new JObject();
            mazeObj["Name"] = name;
            mazeObj["Maze"] = maze.ToJSON(); // TODO verify it's the same as ToString
            mazeObj["Rows"] = maze.Rows;
            mazeObj["Cols"] = maze.Cols;
            Position start = maze.InitialPos;
            JObject startObj = new JObject();
            startObj["Row"] = start.Row;
            startObj["Col"] = start.Col;
            mazeObj["Start"] = startObj;
            Position end = maze.GoalPos;
            JObject endObj = new JObject();
            endObj["Row"] = end.Row;
            endObj["Col"] = end.Col;
            mazeObj["End"] = endObj;

            name = mazeObj.ToString(); ;
            Console.WriteLine(name);
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
