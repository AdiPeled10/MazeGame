namespace ClientForServer
{
    public interface IClient : ICanbeNotified
    {

        bool HasARequest();

        string RecvARequest();  // every request will be separated by a special char, maybe 0b1111 11111

        void SendResponse(string res);

        // Removes allocated resources relating the client communication.
        void Disconnect();

        //// to promise the clients will have good hash 
        //int GetHashCode();
    }
}
