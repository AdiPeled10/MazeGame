using System;
using System.Net.Sockets;
using System.Net;
using System.IO;

using System.Threading;
using System.Collections.Generic;

namespace Client
{
    public class Program
    {
        //static void OutputServerResponses(StreamReader reader, Thread creator)
        //{
        //    // list for reading the respond
        //    List<string> lines = new List<string>(16);
        //    string ans;
        //    while (true)
        //    {
        //        // read the server responds
        //        lines.Add(reader.ReadLine());
        //        while (reader.Peek() > 0)
        //        {
        //            lines.Add(reader.ReadLine());
        //        }
        //        ans = string.Join(System.Environment.NewLine, lines);
        //        lines.Clear();

        //        // if the creator thread is waitng for input, then stop him   reading or writing
        //        creator.ThreadState
        //        Console.WriteLine(ans);
        //    }
        //}

        static void Main(string[] args)
        {
            Console.ReadKey(); // to stop it untill the server is ready.

            //connect to the server
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            client.Connect(ep);
            Console.WriteLine("You are connected");

            // buffer for the responds
            int n;
            char[] buffer = new char[client.ReceiveBufferSize];

            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                // set the writer to immediately flush
                writer.AutoFlush = true;

                // get requests and send them to the server
                string req = "";
                while (true) 
                {
                    // Send data to server
                    Console.Write("Please enter a command:  ");
                    req = Console.ReadLine();
                    writer.WriteLine(req);

                    Thread.Sleep(20);// a human won't feel this but it helps the server responds to look faster.

                    /*
                     * for now it works because the server works in "question-answer"
                     * and not "questions-answers"(can't ask another before the answer
                     * to the previous. Also, if along other he will get notification
                     * that others have moved, its still okay.
                     */
                    if (stream.DataAvailable || (!req.StartsWith("play") && !req.StartsWith("close")))
                    {
                        do
                        {
                            // block untill at least one byte can be read
                            buffer[0] = (char)stream.ReadByte();
                            // keep reading as long as there is data available
                            for (n = 1; stream.DataAvailable && n < client.ReceiveBufferSize; ++n)
                            {
                                buffer[n] = (char)stream.ReadByte();
                            }
                            // this also uses a third buffer. but C# is limited "pointer-wise" and
                            // that's the best I can do
                            Console.Write(buffer, 0, n);
                        } while (stream.DataAvailable) ;
                    }
                }
            }

            //if (client.Connected)
            //{
            //    Console.WriteLine("client was still open!");
            //}

            //client.Close();
        }

        static void ReadOperationThatWorksBadlyAndWhy(NetworkStream stream, StreamReader reader, string req)
        {
            /**
             * The arguments and the variables are necessary for this to work but aren't the reason it's bad.
             * The function can be easy be turned to "not a function" and then they won't be recreated every
             * time.
             * The reason it's so bad is the that the reader has internal buffer and the many operation
             * needed to read the data and then restore it.
             */

            // list for reading the respond
            List<string> lines = new List<string>(16);
            string ans;

            /*
             * for now(while the client is simple and only sends requests and then
             * prints the answers) it works because the server works in "question-answer"
             * and not "questions-answers"(can't ask another before the answer
             * to the previous. Also, if along other he will get notification
             * that others have moved, its still okay.
             * 
             * Why I'm using "stream.DataAvailable" outside but "Peek()" inside? Answer:
             * (the explanation is based on my observations and knowledge only)
             * reader has internal buffer, which means it is possible that "stream.DataAvailable"
             * will be true while reader.Peek() will be -1. Actually, the fact that every time we
             * have data we read all causes to Peek() to be -1 untill next "Read()" (which only then
             * the reader checks the stream for new data) so we need to check for new data using
             * "stream.DataAvailable". Then, after the first read the buffer of the stream is drained
             * by the reader so "strem.DataAvailable" will be false but "reader.Peek()" will be
             * positive, so we need to use it to drain the reader buffer.
             */
            if (stream.DataAvailable || (!req.StartsWith("play") && !req.StartsWith("close")))
            // if the server sended data read it. Or, if we excpect an answer, wait for one.
            {
                lines.Add(reader.ReadLine());
                while (reader.Peek() > 0)
                {
                    lines.Add(reader.ReadLine());
                }
                ans = string.Join(System.Environment.NewLine, lines);
                lines.Clear();
                Console.WriteLine(ans);
            }
        }
    }
}
