using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MMChatEngine.Packets;

namespace MMChatEngine
{
    public class Server
    {
        private readonly SynchronizationContext _context = SynchronizationContext.Current;
        private readonly TcpListener _listener;
        private readonly Dictionary<Guid, Room> _rooms = new Dictionary<Guid, Room>();
        private readonly Dictionary<string, TcpClient> _connectedUsers = new Dictionary<string, TcpClient>();
        private readonly Timer _pingTimer;

        public delegate void ErrorOccurredEventHandler(object sender, ErrorOccurredEventHandlerArgs args);
        public delegate void UserLoginEventHandler(object sender, UserLoginEventHandlerArgs args);
        public delegate void NewUserRegisteredEventHandler(object sender, NewUserRegisteredEventHandlerArgs args);
        public delegate void ConnectedEventHandler(object sender, ConnectedEventHandlerArgs args);
        public delegate void DisconnectedEventHandler(object sender, DisconnectedEventHandlerArgs args);

        public event ErrorOccurredEventHandler ErrorOccurred;
        public event ConnectedEventHandler ClientConnected;
        public event DisconnectedEventHandler ClientDisconnected;
        public event NewUserRegisteredEventHandler NewUserRegistred;
        public event UserLoginEventHandler UserLogin;

        public Server(int port)
        {
            UserManager.Instance.Load();

            _pingTimer = new Timer(PingClient, this, 1000, 5000);

            Room mainRoom = new Room(Room.MainRoomId, Room.MainRoomName, new HashSet<string>());
            _rooms.Add(Room.MainRoomId, mainRoom);

            IPHostEntry localMachineInfo =
            Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint myEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            _listener = new TcpListener(myEndpoint);
            _listener.Start();
        }

        /// <summary>
        /// Ping client to indicate disconnected users
        /// </summary>
        /// <param name="state"></param>
        private void PingClient(object state)
        {
            lock (_connectedUsers)
            {
                List<string> disconnectedUsers = new List<string>();
                foreach (KeyValuePair<string, TcpClient> kp in _connectedUsers)
                {
                    if (!kp.Value.Connected)
                    {
                        disconnectedUsers.Add(kp.Key);
                    }
                }
                foreach (string disconnectedUser in disconnectedUsers)
                {
                    _connectedUsers.Remove(disconnectedUser);
                    _context.Post(p => ClientDisconnected?
                                    .Invoke(this, new DisconnectedEventHandlerArgs(null, disconnectedUser)), null);
                }

                foreach (TcpClient connectedUsersValue in _connectedUsers.Values)
                {
                    foreach (string disconnectedUser in disconnectedUsers)
                    {
                        new UserDisconnectPacket(connectedUsersValue.GetStream(), disconnectedUser).Send();
                    }
                }
            }
        }

        public async void Start()
        {
            _listener.Start();
            while (true)
            {
                var client = await _listener.AcceptTcpClientAsync();
                _context.Post(p => ClientConnected?.Invoke(this, new ConnectedEventHandlerArgs(client)), null);
                Thread t = new Thread(() => Listener(client));
                t.Start();
            }
        }

        private void Listener(TcpClient tcpClient)
        {
            try
            {
                using (Stream stream = tcpClient.GetStream())
                {
                    while (true)
                    {
                        BinaryReader streamReader = new BinaryReader(stream);
                        PacketType packetType = (PacketType)streamReader.ReadInt32();
                        switch (packetType)
                        {
                            case PacketType.Registry:
                                RegistryPacket registryPacket = new RegistryPacket(stream);
                                registryPacket.Receive();
                                RegistryNewUser(tcpClient, registryPacket);
                                break;
                            case PacketType.Login:
                                LoginPacket loginPacket = new LoginPacket(stream);
                                loginPacket.Receive();

                                if (!LoginUser(tcpClient, loginPacket))
                                    return;
                                break;
                            case PacketType.SimpleMessage:
                                SimpleMessagePacket simpleMessagePacket = new SimpleMessagePacket(stream);
                                simpleMessagePacket.Receive();
                                lock (_connectedUsers)
                                {
                                    foreach (TcpClient client in _connectedUsers.Values)
                                    {
                                        simpleMessagePacket.Send(client.GetStream());
                                    }
                                }
                                break;
                            case PacketType.Disconnect:
                                lock (_connectedUsers)
                                {
                                    var item = _connectedUsers.First(u => u.Value == tcpClient);
                                    _connectedUsers.Remove(item.Key);
                                }
                                return;
                            case PacketType.UserDisconnect:
                                UserDisconnectPacket userDisconnectPacket = new UserDisconnectPacket(stream);
                                userDisconnectPacket.Receive();
                                lock (_connectedUsers)
                                {
                                    foreach (TcpClient client in _connectedUsers.Values)
                                    {
                                        userDisconnectPacket.Send(client.GetStream());
                                    }
                                }
                                lock (_connectedUsers)
                                {
                                    _connectedUsers.Remove(userDisconnectPacket.UserLogin);
                                    _context.Send(p => ClientDisconnected?.Invoke(this, new DisconnectedEventHandlerArgs(tcpClient, userDisconnectPacket.UserLogin)), null);
                                }
                                return;
                            case PacketType.CreateNewRoom:
                                lock (_rooms)
                                {
                                    CreateNewRoomPacket createNewRoomPacket = new CreateNewRoomPacket(stream);
                                    createNewRoomPacket.Receive();

                                    _rooms.Add(createNewRoomPacket.Room.Id, createNewRoomPacket.Room);

                                    lock (_connectedUsers)
                                    {
                                        foreach (var connectedUser in _connectedUsers.Where(cu => 
                                                     createNewRoomPacket.Room.Members.Contains(cu.Key)))
                                        {
                                            createNewRoomPacket.Send(connectedUser.Value.GetStream());
                                        }
                                    }
                                }
                                break;
                            case PacketType.CloseRoom:
                                CloseRoomPacket closeRoomPacket = new CloseRoomPacket(stream);
                                closeRoomPacket.Receive();
                                lock (_rooms)
                                {
                                    Room room;
                                    if (_rooms.TryGetValue(closeRoomPacket.RoomId, out room))
                                    {
                                        lock (_connectedUsers)
                                        {
                                            foreach (TcpClient client in _connectedUsers.Where(u => 
                                                               room.Members.Contains(u.Key)).Select(u => u.Value))
                                            {
                                                new RemoveUserFromRoomPacket(client.GetStream(), closeRoomPacket.RoomId, closeRoomPacket.User).Send();
                                            }
                                        }
                                    }
                                }
                                break;
                            case PacketType.UpdateUser:
                                UpdateUserPacket updateUserPacket = new UpdateUserPacket(stream);
                                updateUserPacket.Receive();
                                UserManager.Instance.Update(updateUserPacket.Login, updateUserPacket.UserInfoWithPrivateInfo);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _context.Post(p => ErrorOccurred?.Invoke(this, new ErrorOccurredEventHandlerArgs(ex.Message, ErrorType.ServerListener)), null);
            }
        }

        private bool LoginUser(TcpClient client, LoginPacket loginPacket)
        {
            lock (_connectedUsers)
            {
                if (_connectedUsers.ContainsKey(loginPacket.Login))
                {
                    new ErrorPacket(client.GetStream(), $"User with login {loginPacket.Login} has been already connected.", ErrorType.UserAlreadyLogin).Send();
                    return false;
                }
            }
            UserInfoWithPrivateInfo userInfoWithPrivateInfo;
            if (UserManager.Instance.TryGetUserInfoWithPrivateInfo(loginPacket.Login, out userInfoWithPrivateInfo) && userInfoWithPrivateInfo.Password == loginPacket.Password)
            {
                new NegotiationPacket(client.GetStream(), loginPacket.Login).Send();
                _context.Post(p => UserLogin?.Invoke(this, new UserLoginEventHandlerArgs(loginPacket.Login, loginPacket.Password)), null);
                OnClientAuthorized(client, loginPacket.Login, userInfoWithPrivateInfo);
                return true;
            }

            new ErrorPacket(client.GetStream(), "Incorrect user or password.", ErrorType.IncorrectLoginOrPassword).Send();
            return false;
        }

        private void RegistryNewUser(TcpClient client, RegistryPacket registryPacket)
        {
            if (!UserManager.Instance.ContainsLogin(registryPacket.Login))
            {
                UserManager.Instance.Add(registryPacket.Login, registryPacket.UserInfoWithPrivateInfo);
                new NegotiationPacket(client.GetStream(), registryPacket.Login).Send();
                _context.Post(p => NewUserRegistred?.Invoke(this, new NewUserRegisteredEventHandlerArgs(registryPacket.Login, registryPacket.UserInfoWithPrivateInfo)), null);
                OnClientAuthorized(client, registryPacket.Login, registryPacket.UserInfoWithPrivateInfo);
            }
            else
            {
                new ErrorPacket(client.GetStream(), "User already exist.", ErrorType.UserAlreadyExist).Send();
            }
        }

        private void OnClientAuthorized(TcpClient client, string userLogin, UserInfo userInfo)
        {
            lock (_connectedUsers)
            {
                foreach (KeyValuePair<string, TcpClient> connectedUser in _connectedUsers)
                {
                    new UserConnectPacket(client.GetStream(), connectedUser.Key, UserManager.Instance.GetUserInfo(connectedUser.Key)).Send();
                    new AddUsersToRoomPacket(client.GetStream(), Room.MainRoomId, new List<string> { connectedUser.Key }).Send();
                }
                _connectedUsers.Add(userLogin, client);
                AddUsersToRoomPacket addUsersToRoomPacket = new AddUsersToRoomPacket(client.GetStream(), Room.MainRoomId, new List<string> { userLogin });
                UserConnectPacket userConnectPacket = new UserConnectPacket(client.GetStream(), userLogin, userInfo);
                foreach (TcpClient cl in _connectedUsers.Values)
                {
                    userConnectPacket.Send(cl.GetStream());
                    addUsersToRoomPacket.Send(cl.GetStream());
                }
                lock (_rooms)
                {
                    _rooms[Room.MainRoomId].Members.Add(userLogin);
                }
            }
        }
    }    
}
