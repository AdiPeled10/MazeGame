using System.Net.Sockets;
using System.IO;
using ClientForServer;

namespace Server
{
    //internal delegate void SendAlias(string message);

    public class MyTcpClient : IClient
    {
        private TcpClient client;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private int hashCode;

        public MyTcpClient(TcpClient client) // TODO fix the hash value
        {
            this.client = client;
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            hashCode = client.GetHashCode();

            // set the writer to immediately flush
            writer.AutoFlush = true;
        }

        public bool HasARequest()
        {
            if (client.Connected)
            {
                // stream has internal buffer and may contain many
                // requests while the stream won't.
                return stream.DataAvailable || reader.Peek() > 0;
            }
            throw new IOException("Client is disconnected");
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
            writer.WriteLine(res);// + System.Environment.NewLine);
        }

        public void Notify(string res)// = this.SendResponse;
        {
            writer.WriteLine(res);
        }

        public void Disconnect()
        {
            writer.Close();
            // TODO check this succeeds and not fails because the writer was closed before it 
            // (and closed its stream)
            reader.Close();
            stream.Close();
            client.Close();
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}