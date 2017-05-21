using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using ViewModel;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace GUIClient
{
    /// <summary>
    /// A delegate that notify listeners everytimes a certain string has changed.
    /// </summary>
    /// <param name="str">
    /// The new string.
    /// </param>
    public delegate void GotString(string str);

    /// <summary>
    /// A delegate that pass the listeners start location and end location.
    /// </summary>
    /// <param name="start">
    /// Location class type object.
    /// </param>
    /// <param name="end">
    /// Location class type object.
    /// </param>
    public delegate void Locations(Location start, Location end);

    /// <summary>
    /// A delegate that pass the listeners the measurment of a maze.
    /// </summary>
    /// <param name="rows">
    /// An int type, the number of rows in the maze.
    /// </param>
    /// <param name="cols">
    /// An int type, the number of cols in the maze.
    /// </param>
    public delegate void MazeSize(int rows, int cols);

    /// <summary>
    /// A delegate that notify listeners everytimes a certain string has changed.
    /// </summary>
    /// <param name="name">
    /// The new string.
    /// </param>
    public delegate void Name(string name);

    /// <summary>
    /// A delegate that give listeners a string representation a maze solution and that maze name.
    /// </summary>
    /// <param name="solution">
    /// The serialized solution string.
    /// </param>
    /// <param name="name">
    /// The maze name.
    /// </param>
    public delegate void MazeSolution(string solution, string name);

    /// <summary>
    /// A delegate that give listeners a list of strings(games names).
    /// </summary>
    /// <param name="games">
    /// The list of strings.
    /// </param>
    public delegate void GotList(List<string> games);

    /// <summary>
    /// A delegate that notify listeners everytimes owner gets a new string "direction".
    /// </summary>
    /// <param name="direction">
    /// The new string.
    /// </param>
    public delegate void GotDirection(string direction);

    /// <summary>
    /// A delegate that notify listeners when something spesific has changed and
    /// its value doesn't matter or was known (like bollean value the was initially
    /// knowon and no has changed).
    /// </summary>
    public delegate void OpponentLeft();

    /// <summary>
    /// A local model in the client side. Incharge of direct communication with
    /// the server and notify handlers on the server responds.
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// A refrence to a TCP socket that will be used to communication with the server.
        /// </summary>
        TcpClient client;

        /// <summary>
        /// A delegate that notify listeners when the some other player who was part of the game
        /// has disconnected while the game is still active.
        /// </summary>
        public event OpponentLeft Disconnection;

        /// <summary>
        /// A delegate that notify listeners if the ClientModel failed to connect to the server.
        /// </summary>
        public event Parameterless WhereIsServer;

        /// <summary>
        /// Event to notify listeners that we have the solution to the maze.
        /// </summary>
        public event MazeSolution GotSolution;

        /// <summary>
        /// Event to notify listeners that we have a string of a new maze(a sequance of 0,1
        /// where 1 represents a wall and 0 reprenets a free pass).
        /// </summary>
        public event GotString GeneratedMaze;

        /// <summary>
        /// Event to give the listeners the start and end locations of a new maze that has arrived.
        /// </summary>
        public event Locations MazeLoc;

        /// <summary>
        /// Event that passes the listeners the measurment of a new maze.
        /// </summary>
        public event MazeSize MazeRowsCols;

        /// <summary>
        /// Event that passes the listeners the name of a new maze.
        /// </summary>
        public event Name NotifyName;

        /// <summary>
        /// Event that passes the listeners a names list of joinable multiplayer games.
        /// </summary>
        public event GotList NotifyList;

        /// <summary>
        /// A list valid moves in the game.
        /// </summary>
        private List<string> validDirections;

        /// <summary>
        /// Event to notify of opponent movement.
        /// </summary>
        public event GotDirection NotifyDirection;

        /// <summary>
        /// A boolean value that is false when the class need to stop getting other player movements,
        /// and is true otherwise.
        /// </summary>
        private bool stop = false;

        /// <summary>
        /// Stop property.
        /// </summary>
        /// <value>
        /// True or False. 
        /// False when the class need to stop getting other player movements,
        /// and is true otherwise.
        /// </value>
        public bool Stop
        {
            set { stop = value; }
            get { return stop; }
        }

        /// <summary>
        /// The class constructor.
        /// Opens a new TCP socket.
        /// </summary>
        public ClientModel()
        {
            client = new TcpClient();
        }

        /// <summary>
        /// Connect to the server.
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
        /// Asking the server to generate a new Maze  and send it to us. Then, calling MazeFromJSON.
        /// We get command name because generate and start commands are practically the same.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        public void GenerateMaze(string command, string name, int rows, int cols)
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
            string response = "";
            while (!stop)
            {
                System.Threading.Thread.Sleep(200);
                response += GetResponse();
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
            int algorithm = Properties.Settings.Default.SearchAlgorithm;
            string request = "solve " + name + " " + algorithm.ToString();
            //Send the request.
            SendMessage(request);

            //Wait for response.
            string response = "";
            do
            {
                response += GetResponse();
            } while (!response.EndsWith("}\r\n"));
            SolutionFromJson(response);
        }

        /// <summary>
        /// Sends a message to the server.
        /// </summary>
        /// <param name="message"> The message to be sent to the server </param>
        public void SendMessage(string message)
        {
            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            //Convert message to bytes.
            writer.WriteLine(message);
        }

        /// <summary>
        /// Gets a responed from the server.
        /// </summary>
        /// <returns> The server respond </returns>
        public string GetResponse()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                string response = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                return response;
            } catch (Exception ) { return ""; }
        }

        /// <summary>
        /// Close the connection with the server.
        /// </summary>
        public void CloseClient(string name)
        {
            SendMessage("close " + name);
            client.Close();
        }

        /// <summary>
        /// analize the JSON and calls "GotSolution" event with the solution string
        /// and the name of the maze it solves.
        /// </summary>
        /// <param name="json"> A JSON of a solution. </param>
        public void SolutionFromJson(string json)
        {
            JObject obj = JObject.Parse(json);
            try
            {
                //Extract maze solution.
                string solution = (string)obj["Solution"];
                string name = (string)obj["Name"];
                GotSolution(solution, name);
            } catch( Exception)
            {
                //Case in which this field doesn't appear.
            }
        }

        /// <summary>
        /// Creates a maze from the giving JSON of a maze.
        /// It also calls all maze associated events(except "GotSolution").
        /// </summary>
        /// <param name="json"> A JSON of a maze. </param>
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

        /// <summary>
        /// Gets a list of names of joinable multiplayer games and pass it to "GotList" listeners.
        /// </summary>
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
        /// Joins the user to a multiplayer game named as the value of "name".
        /// </summary>
        /// <param name="name"> Name of a joinable multiplayer game. </param>
        public void JoinGame(string name)
        {
            string message = "join " + name;
            SendMessage(message);

            //Receive response.
            System.Threading.Thread.Sleep(500);
            string response = GetResponse();
            MazeFromJSON(response);
        }

        /// <summary>
        /// Tells the server that the user moved in direction "direction".
        /// </summary>
        /// <param name="direction"> The direction that the user moved. </param>
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
