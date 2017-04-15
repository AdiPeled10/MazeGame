using ClientForServer;

namespace View
{
    public interface IView
    {
        void SendServerResponseTo(string res, IClient c);

        void HandleClientRequest(string res, IClient c);

        void GetClientsRequests();

        void AddClient(IClient c); // IClient might be using TCP or UDP
    }
}
