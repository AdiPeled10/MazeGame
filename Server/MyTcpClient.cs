using System.Net.Sockets;
using System.IO;
using ClientForServer;

namespace Server
{
    public class MyTcpClient : IClient
    {
        private StreamReader reader;
        private StreamWriter writer;
        private int hashCode;

        public MyTcpClient(TcpClient client) // TODO fix the hash value
        {
            NetworkStream stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            hashCode = client.GetHashCode();
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

        public void Notify(string res)
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

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}