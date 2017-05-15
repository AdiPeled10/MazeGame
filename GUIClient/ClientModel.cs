using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ViewModel;
using Newtonsoft.Json.Linq;
using System.IO;

namespace GUIClient
{
    public delegate void GotString(string str);

    public delegate void Locations(Location start, Location end);

    public delegate void MazeSize(int rows, int cols);

    public delegate void Name(string name);

    public delegate void MazeSolution(string solution,string name);

    public class ClientModel
    {
        TcpClient client;

        //Event to notify listeners that we have the solution to the maze.
        public event MazeSolution GotSolution;
        
        public event GotString GeneratedMaze;

        public event Locations MazeLoc;

        public event MazeSize MazeRowsCols;

        public event Name NotifyName;

        public ClientModel()
        {
            //Create EndPoint based on default settings.
            string adr = GUIClient.Properties.Settings.Default.ServerIP == "" ?
                        "127.0.0.1" : GUIClient.Properties.Settings.Default.ServerIP;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(adr),
               GUIClient.Properties.Settings.Default.ServerPort);
            client = new TcpClient();
            //Connect our TcpClient to the EndPoint.
            client.Connect(ep);
            //Ended initiation of the client.
        }

        public void GenerateMaze(string name, int rows, int cols)
        {
            //Send the request.
            string request = "generate " + name + " " + rows.ToString() + " " + cols.ToString();
            SendMessage(request);

            //Get the response.
            string response = GetResponse();
            MazeFromJSON(response);
        }

        /// <summary>
        /// Ask server for solution for given maze,take search algorithm by default
        /// from settings.
        /// </summary>
        /// <param name="name"></param>
        public void GetMazeSolution(string name)
        {
            //int algorithm = GUIClient.Properties.Settings.Default.SearchAlgorithm;
            //TODO- fix save of search algorithm in settings.
            int algorithm = 1;
            string request = "solve" + " " + name + " " + algorithm.ToString();
            //Send the request.
            SendMessage(request);

            //Wait for response.
            string response = GetResponse();
            SolutionFromJson(response);

        }

        public void SendMessage(string message)
        {
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            //Convert message to bytes.
            writer.WriteLine(message);
        }

        public string GetResponse()
        {
            NetworkStream stream = client.GetStream();
            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            string response = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
            return response;
        }

        /// <summary>
        /// Close the connection with the server.
        /// </summary>
        public void CloseClient()
        {
            client.Close();
        }

        public void SolutionFromJson(string json)
        {
            JObject obj = JObject.Parse(json);
            try
            {
                //Extract maze solution.
                string solution = (string)obj["Solution"];
                string name = (string)obj["Name"];
                GotSolution(solution,name);
            } catch( Exception)
            {
                //Case in which this field doesn't appear.
            }
        }

        public void MazeFromJSON(string json)
        {
            JObject obj = JObject.Parse(json);
            try
            {
                Location start = new Location((int)obj["Start"]["Row"],
                                                (int)obj["Start"]["Col"]);
                Location end = new Location((int)obj["End"]["Row"],
                                                (int)obj["End"]["Col"]);
                //Call event for locations.
                MazeLoc(start, end);
                //Get string,starting position and end position.
                int rows = (int)obj["Rows"];
                int cols = (int)obj["Cols"];
                MazeRowsCols(rows, cols);

                string maze = (string)obj["Maze"];
                //Call generated event.
                string name = (string)obj["Name"];
                //Activate events.
                NotifyName(name);
                GeneratedMaze(maze);
            } catch (Exception)
            {
                //Case where a field in jobject doesn't appear.
            }
        }
        public void GetSolutionToMaze(string name, int algorithm) { }


    }
}
