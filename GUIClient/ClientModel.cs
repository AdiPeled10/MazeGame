using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using ViewModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Threading;
using System.IO;

namespace GUIClient
{
    public delegate void GotString(string str);

    public delegate void Locations(Location start, Location end);

    public delegate void MazeSize(int rows, int cols);

    public delegate void Name(string name);

    public delegate void MazeSolution(string solution,string name);

    public delegate void GotList(List<string> games);

    public delegate void GotDirection(string direction);

    public delegate void OpponentLeft();


    public class ClientModel
    {
        TcpClient client;

        public event OpponentLeft Disconnection;

        public event Parameterless WhereIsServer;

        //Event to notify listeners that we have the solution to the maze.
        public event MazeSolution GotSolution;
        
        public event GotString GeneratedMaze;

        public event Locations MazeLoc;

        public event MazeSize MazeRowsCols;

        public event Name NotifyName;

        public event GotList NotifyList;

        private List<string> validDirections;

        /// <summary>
        /// Event to notify of opponent movement.
        /// </summary>
        public event GotDirection NotifyDirection;

        private bool stop = false;

        public bool Stop
        {
            set { stop = value; }
            get { return stop; }
        }

        public ClientModel()
        {
            client = new TcpClient();
           
        }

        /// <summary>
        /// Connect to server.
        /// </summary>
        public void Connect()
        {
            //Create EndPoint based on default settings.
            string adr = GUIClient.Properties.Settings.Default.ServerIP == "" ?
                        "127.0.0.1" : GUIClient.Properties.Settings.Default.ServerIP;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(adr),
               GUIClient.Properties.Settings.Default.ServerPort);
            try
            {
                client.Connect(ep);
            }
            catch (Exception)
            {
                //Can't connect to server.
                WhereIsServer();
            }
            //Ended initiation of the client.
            client.ReceiveBufferSize *= 2;
        }

        /// <summary>
        /// We get command name because generate and start commands are practically the same.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void GenerateMaze(string command,string name, int rows, int cols)
        {
            //Send the request.
            string request = command + " " + name + " " + rows.ToString() + " " + cols.ToString();
            SendMessage(request);

            System.Threading.Thread.Sleep(200);
            string response = GetResponse();
            MazeFromJSON(response);
        }

        /// <summary>
        /// Different from generate because we will get the response only when the
        /// opponent connects.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void StartGame(string name,int rows,int cols)
        {
            //Send the request.
            string request = "start " + name + " " + rows.ToString() + " " + cols.ToString();
            SendMessage(request);

            //Loop until the json parsing worked and we got full string.
            while (!stop)
            {

                string response = GetResponse();
                MazeFromJSON(response);
            }
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
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                string response = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                Console.WriteLine("Got response " + response);
                return response;
            } catch (Exception ) { return ""; }
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
            try
            {
                JObject obj = JObject.Parse(json);
                stop = true;
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
                //Case of start where response isn't full until opponent connects.
            }
        }

        public void ListGames()
        {
            //Ask server for list of joinable games.
            SendMessage("list");

            //Get server's response.
            string response = GetResponse();

            //Analyze server response to build list.
            List<string> games = JsonConvert.DeserializeObject<List<string>>(response);
            //Notify this list of games.
            NotifyList(games);

        }

        /// <summary>
        /// TODO - Maybe save some code later.
        /// </summary>
        /// <param name="name"></param>
        public void JoinGame(string name)
        {
            string message = "join " + name;
            SendMessage(message);

            //Receive response.
            string response = GetResponse();
            MazeFromJSON(response);
        }

        public void PlayMove(string direction)
        {
            //Build string for command and send it.
            string message = "play " + direction;
            SendMessage(message);
        }

        /// <summary>
        /// Listen to moves of opponent. We will get response for every play that he
        /// sends to the server.
        /// </summary>
        public void GetOpponentMoves()
        {
            stop = false;
            string response;
            JObject obj = null;
            validDirections = new List<string>();
            validDirections.Add("left");
            validDirections.Add("right");
            validDirections.Add("down");
            validDirections.Add("up");
            //Loop until we got a valid move and notify.
            while(!stop)
            {
                Console.WriteLine("LOOP");
                response = GetResponse();
                if (response.ToLower() == "exit\r\n" || response.ToLower().Equals("exit"))
                {
                    //Opponent left the game.
                    Disconnection();
                    break;
                }
                    try
                    {
                    //Task myTask = new Task(() =>
                    //{
                        obj = JObject.Parse(response);
                        //Notify of direction.
                        if (validDirections.Contains((string)obj["Direction"].ToString().ToLower()))
                            NotifyDirection((string)obj["Direction"]);

                    //});
                    //myTask.Start();
                }
                    catch (Exception)
                    {
                        //Exception during parsing of json.
                    }
                //Deserialize to jobject.
                
            }
        }


    }
}
