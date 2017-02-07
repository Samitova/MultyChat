using System.Net;
using System.Net.Sockets;

namespace MMChatEngine
{
    public class ConnectedEventHandlerArgs
    {
        private TcpClient _client;

        public ConnectedEventHandlerArgs(TcpClient client)
        {
            _client = client;
        }

        public IPAddress Ip
        {
            get { return ((IPEndPoint) _client.Client.RemoteEndPoint).Address; }
        }
    }
}