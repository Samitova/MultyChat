using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MMChatEngine
{
    public class DisconnectedEventHandlerArgs
    {
        private TcpClient _client;
        
        public string UserLogin { get; }

        public DisconnectedEventHandlerArgs(TcpClient client, string userLogin)
        {
            _client = client;
            UserLogin = userLogin;
        }

        public string Ip
        {
            get { return ((IPEndPoint) _client?.Client.RemoteEndPoint)?.Address.ToString() ?? "Unknown"; }
        }
    }
}
