using System.Net.Sockets;
using System.IO;
using ClientForServer;
using System;

namespace Server
{

    /// <summary>
    /// Implementation of the IClient interface in a TcpClient that we will use
    /// to communicate with the server.
    /// </summary>
    public class MyTcpClient : IClient
    {
        /// <summary>
        /// The TcpClient that we will use.
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// The NetWorkStream of communication.
        /// </summary>
        private NetworkStream stream;

        /// <summary>
        /// The Reader which we are going to read messages from.
        /// </summary>
        private StreamReader reader;

        /// <summary>
        /// The writer which we are going to write messages to.
        /// </summary>
        private StreamWriter writer;

        /// <summary>
        /// The hashcode of this client.
        /// </summary>
        private int hashCode;

        /// <summary>
        /// Constructor of this client by getting the TcpClient.
        ///  TODO fix the hash value
        /// </summary>
        /// <param name="client">
        /// The TcpClient which represents this client.
        /// </param>
        public MyTcpClient(TcpClient client) 
        {
            this.client = client;
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            hashCode = client.GetHashCode();
            this.client.Client.Blocking = false;

            // set the writer to immediately flush
            writer.AutoFlush = true;
        }

        /// <summary>
        /// Tells us if the client has a request.
        /// </summary>
        /// <exception cref="IOException">
        /// Thrown when the client is disconnected.
        /// </exception>
        /// <returns>
        /// True if he does, false otherwise.
        /// </returns>
        public bool HasARequest()
        {
            if (client.Connected)
            {
                // reader has internal buffer and may contain many
                // requests while the stream won't.
                try
                {
                    return stream.DataAvailable || reader.Peek() > 0;
                }
                //catch (ObjectDisposedException)
                //{
                //    return false;
                //}
                catch (IOException)
                {
                    return false;
                }
                //catch (Exception)
                //{
                //    return false;
                //}
            }
            throw new IOException("Client is disconnected");
        }

        /// <summary>
        /// Receive a request from the server.
        /// </summary>
        /// <returns>
        /// The request from the server.
        /// </returns>
        public string RecvARequest()
        {
            return reader.ReadLine();
        }

        /// <summary>
        /// Send a response to the server. 
        /// </summary>
        /// <exception cref="IOException">
        /// The client is already closed,cant write.
        /// </exception>
        /// <param name="res">
        /// The response that was sent.
        /// </param>
        public void SendResponse(string res)
        {
            try
            {
                writer.WriteLine(res);
            } catch (IOException)
            {
                //The client is already closed.
            }
        }

        /// <summary>
        /// Notify the client with a message.(Because of ICanBeNotified)
        /// </summary>
        /// <exception cref="IOException">
        /// When we try to notify a client which is already closed.
        /// </exception>
        /// <param name="res">
        /// Message sent.
        /// </param>
        public void Notify(string res)
        {
            try
            {
                writer.WriteLine(res);
            } catch (IOException )
            {
                //In this case the notified client is already closed.
            }
        }

        /// <summary>
        /// Disconnect the client from the server.
        /// </summary>
        public void Disconnect()
        {
            writer.Close();
            // TODO check this succeeds and not fails because the writer was closed before it 
            // (and closed its stream)
            reader.Close();
            stream.Close();
            client.Close();
        }

        /// <summary>
        /// Override the GetHashCode of object class.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return hashCode;
        }
    }
}