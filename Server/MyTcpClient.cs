using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class MyTcpClient : IClient
    {
        private StreamReader reader;
        private StreamWriter writer;
        //private List<string> requestCache;

        public MyTcpClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            //requestCache = new List<string>();
        }

        public bool HasARequest()
        {
            return !reader.EndOfStream; // requestCache.Count != 0 || !reader.EndOfStream;
        }

        public string RecvARequest()
        {
            //// TODO find out what happens here when TcpClient is closed
            //// TODO make sure thw second "ReadToEnd" will work
            //string req;
            //if (requestCache.Count !=0)
            //{
            //    req = requestCache[0];
            //    requestCache.RemoveAt(0);
            //}
            //else
            //{
            //    string[] reqs = reader.ReadToEnd().Split('\n');
            //    req = reqs[0];
            //    requestCache = reqs.Skip(1).ToList();
            //}
            return reader.ReadLine();
        }

        public void SendResponse(string res)
        {
            writer.Write(res);
        }

        public void Disconnect()
        {
            writer.Close();
            // TODO check this succeeds and not fails because the writer was closed before it 
            // (and closed its stream)
            reader.Close();
        }
    }
}