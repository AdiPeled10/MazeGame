using System;
using MazeGeneratorLib;
using Views;
using Models;
using Controllers;
using SearchGames;
using System.Configuration;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // create the server "building blocks"
            MazeGameGenerator generator = new MazeGameGenerator(new DFSMazeGenerator());
            RequestsQueue queue = new RequestsQueue();
            Model model = new Model(generator, MazeGame.FromJSON);
            Controller controller = new Controller(model, queue);
            View view = new View(controller);
            Server server = new Server(int.Parse(ConfigurationManager.AppSettings["Port"]), view);

            // start getting clients
            server.Start();

            // close the server
            Console.WriteLine("Press any button to close the server.");
            Console.ReadKey();
            server.Stop();
            //CancellationTokenSource tokenSource2 = new CancellationTokenSource();
            //Task t = new Task(() => Console.WriteLine("First")), tcopy;
            //tcopy = t;
            ////tcopy.Start();
            //Thread.Sleep(1000);
            //t = t.ContinueWith(dummy => Console.WriteLine("Second"), tokenSource2.Token);
            //t = t.ContinueWith(dummy => Console.WriteLine("Third"), tokenSource2.Token);
            //t = t.ContinueWith(dummy => Console.WriteLine("Fourth"), tokenSource2.Token);
            //t = t.ContinueWith(dummy => Console.WriteLine("Fifth"));
            //t = t.ContinueWith(dummy => Console.WriteLine("Sixth"), tokenSource2.Token);
            ////return;

            //tcopy.Start();
            //tokenSource2.Cancel();

            //Task<string> strTask = new Task<string>(() => "First "), strTaskCopy;
            //strTaskCopy = strTask;
            //strTask = strTask.ContinueWith(str => str.Result + "Second ");
            //strTask = strTask.ContinueWith(str => str.Result + "Third ");
            //strTask = strTask.ContinueWith(str => str.Result + "Fourth ");
            //strTask = strTask.ContinueWith(str => str.Result + "Fifth ");
            //strTask = strTask.ContinueWith(str => str.Result + "Sixth");
            ////tcopy.Start();
            //strTaskCopy.Start();
            //Console.WriteLine(strTask.Result);
        }
    }
}
