using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MMChatEngine.Packets;
using System.Threading.Tasks;

namespace MMChatEngine
{
    public class Client
    {
        private readonly SynchronizationContext _context = SynchronizationContext.Current;
        private readonly Dictionary<string, UserInfo> _users = new Dictionary<string, UserInfo>();
        private readonly Dictionary<Guid, Room> _rooms = new Dictionary<Guid, Room>();
        public string Login { get; set; }
        private BinaryWriter _streamWriter;

        private TcpClient _server;
        private readonly ManualResetEvent[] _loginEvents = { new ManualResetEvent(false), new ManualResetEvent(false) };
        private readonly ManualResetEvent[] _registerEvents = { new ManualResetEvent(false), new ManualResetEvent(false) };

        public delegate void UserUpdatedEventHandler(object sender, EventArgs args);
        public delegate void RoomClosedEventHandler(object sender, RoomClosedEventHandlerArgs args);
        public delegate void RoomsUpdatedEventHandler(object sender, RoomsUpdatedEventHandlerArgs args);
        public delegate void NegotiationCompletedEventHandler(object sender, EventArgs args);
        public delegate void NewRoomCreatedEventHandler(object sender, NewRoomCreatedEventHandlerArgs args);
        public delegate void UserDisconnectedEventHandler(object sender, UserDisconnectedEventHandlerArgs args);
        public delegate void UserConnectedEventHandler(object sender, UserConnectedEventHandlerArgs args);
        public delegate void SimpleMessageReceivedEventHandler(object sender, SimpleMessageRecivedEventHandlerArgs args);
        public delegate void ErrorOccurredEventHandler(object sender, ErrorOccurredEventHandlerArgs args);

        public event ErrorOccurredEventHandler ErrorOccurred;
        public event SimpleMessageReceivedEventHandler SimpleMessageReceived;
        public event UserConnectedEventHandler UserConnected;
        public event UserDisconnectedEventHandler UserDisconnected;
        public event NewRoomCreatedEventHandler NewRoomCreated;
        public event NegotiationCompletedEventHandler NegotiationCompleted;
        public event RoomsUpdatedEventHandler RoomsUpdated;
        public event RoomClosedEventHandler RoomClosed;
        public event UserUpdatedEventHandler UserUpdated;

        public Client()
        {
            _server = new TcpClient();
            UserDisconnected += OnUserDisconnected;
            ErrorOccurred += OnErrorOccurred;
        }

        private void OnErrorOccurred(object sender, ErrorOccurredEventHandlerArgs args)
        {
            if (args.ErrorType == ErrorType.IncorrectLoginOrPassword || args.ErrorType == ErrorType.UserAlreadyLogin)
            {
                _loginEvents[1].Set();
            }
            if (args.ErrorType == ErrorType.UserAlreadyExist)
            {
                _registerEvents[1].Set();
            }
        }

        public bool IsConnected
        {
            get { return _server.Connected; }
        }

        private void OnUserDisconnected(object sender, UserDisconnectedEventHandlerArgs args)
        {
            if (Login == args.Login)
            {
                new DisconnectPacket(_streamWriter.BaseStream).Send();
                Disconnect();
                Login = string.Empty;
            }
        }

        private void Disconnect()
        {
            _server.GetStream().Close();
            _server.Close();
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                _server = new TcpClient();
            }
            try
            {
                _server.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
                _streamWriter = new BinaryWriter(_server.GetStream());
            }
            catch (Exception)
            {
                return;
            }
            Thread t = new Thread(ClientStreamReadData);
            t.Start();
        }

        /// <summary>
        /// Read and prosess data from stream
        /// </summary>
        private void ClientStreamReadData()
        {
            try
            {
                using (Stream stream = _server.GetStream())
                {
                    BinaryReader streamReader = new BinaryReader(stream);
                    while (true)
                    {
                        PacketType packetType = (PacketType) streamReader.ReadInt32();

                        switch (packetType)
                        {
                            case PacketType.Negotiation:
                                NegotiationPacket negotiationPacket = new NegotiationPacket(stream);
                                negotiationPacket.Receive();
                                Login = negotiationPacket.Login;
                                _loginEvents[0].Set();
                                _registerEvents[0].Set();
                                lock (_rooms)
                                {
                                    Room mainRoom = new Room(Room.MainRoomId, Room.MainRoomName, new HashSet<string>());
                                    _rooms.Add(Room.MainRoomId, mainRoom);
                                }
                                _context.Post(p => NegotiationCompleted?.Invoke(this, EventArgs.Empty), null);
                                break;                           
                            case PacketType.SimpleMessage:
                                SimpleMessagePacket simpleMessagePacket = new SimpleMessagePacket(stream);
                                simpleMessagePacket.Receive();
                                OnSimpleMessageReceived(simpleMessagePacket);
                                break;
                            case PacketType.Error:
                                ErrorPacket errorPacket = new ErrorPacket(stream);
                                errorPacket.Receive();
                                _context.Post(p => ErrorOccurred?.Invoke(this, new ErrorOccurredEventHandlerArgs(errorPacket.ErrorMessage, errorPacket.ErrorType)), null);
                                break;
                            case PacketType.UserConnect:
                                UserConnectPacket userConnectPacket = new UserConnectPacket(stream);
                                userConnectPacket.Receive();
                                lock (_users)
                                {
                                    _users[userConnectPacket.UserLogin] = userConnectPacket.UserInfo;
                                }
                                _context.Post(p => UserConnected?.Invoke(this, new UserConnectedEventHandlerArgs(userConnectPacket.UserLogin, userConnectPacket.UserInfo)), null);
                                break;
                            case PacketType.UserDisconnect:
                                UserDisconnectPacket userDisconnectPacket = new UserDisconnectPacket(stream);
                                userDisconnectPacket.Receive();
                                lock (_users)
                                {
                                    _users.Remove(userDisconnectPacket.UserLogin);
                                }
                                _context.Post(p => UserDisconnected?.Invoke(this, new UserDisconnectedEventHandlerArgs(userDisconnectPacket.UserLogin)), null);
                                break;
                            case PacketType.CreateNewRoom:
                                CreateNewRoomPacket createNewRoomPacket = new CreateNewRoomPacket(stream);
                                createNewRoomPacket.Receive();
                                lock (_rooms)
                                {
                                    _rooms.Add(createNewRoomPacket.Room.Id, createNewRoomPacket.Room);
                                }
                                _context.Post(p => NewRoomCreated?.Invoke(this, new NewRoomCreatedEventHandlerArgs(createNewRoomPacket.Room)), null);
                                break;
                            case PacketType.AddUserToRoom:
                                AddUsersToRoomPacket addUsersToRoomPacket = new AddUsersToRoomPacket(stream);
                                addUsersToRoomPacket.Receive();
                                lock (_rooms)
                                {
                                    foreach (string user in addUsersToRoomPacket.Users)
                                    {
                                        _rooms[addUsersToRoomPacket.RoomId].Members.Add(user);
                                    }
                                }
                                _context.Post(p => RoomsUpdated?.Invoke(this, new RoomsUpdatedEventHandlerArgs()), null);
                                break;
                            case PacketType.RemoveUserFromRoom:
                                RemoveUserFromRoomPacket removeUserFromRoomPacket = new RemoveUserFromRoomPacket(stream);
                                removeUserFromRoomPacket.Receive();
                                lock (_rooms)
                                {
                                    Room room;
                                    if (_rooms.TryGetValue(removeUserFromRoomPacket.RoomId, out room))
                                    {
                                        room.Members.Remove(removeUserFromRoomPacket.User);
                                    }
                                }
                                _context.Post(p => RoomsUpdated?.Invoke(this, new RoomsUpdatedEventHandlerArgs()), null);
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                _server.Close();
                _server.Client.Dispose();                
            }
        }

        public UserInfo GetUserInfo(string login)
        {
            lock(_users)
            {
                return _users[login];
            }
        }

        private void OnSimpleMessageReceived(SimpleMessagePacket simpleMessagePacket)
        {
            _context.Post(p => SimpleMessageReceived?.Invoke(this, 
                new SimpleMessageRecivedEventHandlerArgs(simpleMessagePacket.Room, simpleMessagePacket.UserLogin, simpleMessagePacket.DateTime, simpleMessagePacket.Message)), null);
        }

        public Task<RequestToServerResult> CreateMultiChat(string name, IReadOnlyList<string> users)
        {
            return Task<RequestToServerResult>.Factory.StartNew(() =>
            {
                var members = new HashSet<string>();
                foreach (string user in users)
                {
                    members.Add(user);
                }
                new CreateNewRoomPacket(_streamWriter.BaseStream, new Room(Guid.NewGuid(), name, members)).Send();
                return RequestToServerResult.OK;
            });
        }

        public Task<RequestToServerResult> UpdateUser(string login, UserInfoWithPrivateInfo userInfoWithPrivateInfo)
        {
            return Task<RequestToServerResult>.Factory.StartNew(() =>
            {
                new UpdateUserPacket(_streamWriter.BaseStream, login, userInfoWithPrivateInfo).Send();
                lock (_users)
                {
                    _users[Login] = userInfoWithPrivateInfo;
                }
                _context.Post(p => UserUpdated?.Invoke(this, EventArgs.Empty), null);
                return RequestToServerResult.OK;
            });
        }

        public Task<RequestToServerResult> RegisterNewUser(string login, UserInfoWithPrivateInfo userInfoWithPrivateInfo)
        {
            return Task<RequestToServerResult>.Factory.StartNew(() =>
            {
                if (IsConnected)
                {
                    _registerEvents[0].Reset();
                    _registerEvents[1].Reset();
                    new RegistryPacket(_streamWriter.BaseStream, login, userInfoWithPrivateInfo).Send();
                    var result = WaitHandle.WaitAny(_registerEvents, 60000);
                    if (result == 0)
                    {
                        return RequestToServerResult.OK;
                    }
                    else
                    {
                        return RequestToServerResult.Failed;
                    }
                }
                return RequestToServerResult.Disconnect;
            });
        }

       
        public Task<RequestToServerResult> SendNegotiation(string login, string password)
        {
            return Task<RequestToServerResult>.Factory.StartNew(() =>
            {
                if (IsConnected)
                {
                    _loginEvents[0].Reset();
                    _loginEvents[1].Reset();
                    new LoginPacket(_streamWriter.BaseStream, login, password).Send();
                    var result = WaitHandle.WaitAny(_loginEvents, 60000);
                    if (result == 0)
                    {
                        return RequestToServerResult.OK;
                    }
                    else
                    {
                        return RequestToServerResult.Failed;
                    }
                }
                return RequestToServerResult.Disconnect;
            });
        }

        public void SendSimpleMessage(Guid room, string message)
        {
            new SimpleMessagePacket(_streamWriter.BaseStream, room, Login, DateTime.Now, message).Send();
        }

        public IReadOnlyDictionary<string, UserInfo> Users
        {
            get
            {
                lock (_users)
                {
                    return  new ReadOnlyDictionary<string, UserInfo>(_users);
                }
            }
        }

        public IReadOnlyDictionary<Guid, Room> Rooms
        {
            get
            {
                lock (_rooms)
                {
                    return new ReadOnlyDictionary<Guid, Room>(_rooms);
                }
            }
        }

        public void UserDisconnect()
        {
            if (!string.IsNullOrWhiteSpace(Login))
            {
                new UserDisconnectPacket(_streamWriter.BaseStream, Login).Send();
            }
            else
            {
                Disconnect();
            }
        }

        public void CreatePrivateChat(string secondMember)
        {
            string name = $"Private: {Login} and {secondMember}";
            var members = new HashSet<string> {Login, secondMember};
            new CreateNewRoomPacket(_streamWriter.BaseStream, new Room(Guid.NewGuid(), name, members)).Send();
        }

        public void AddUsersToRoom(Guid roomId, List<string> users)
        {
            new AddUsersToRoomPacket(_streamWriter.BaseStream, roomId, users).Send();
        }

        public void CloseRoom(Guid roomId)
        {
            new CloseRoomPacket(_streamWriter.BaseStream, roomId, Login).Send();
            _context.Post(p => RoomClosed?.Invoke(this, new RoomClosedEventHandlerArgs(roomId)), null);
        }

        public List<KeyValuePair<string, string>> GetRoomUsers(Guid roomId)
        {
            return Rooms.Where(r => r.Key == roomId).
                SelectMany(user => Users, (room, user) => new { room.Value.Members, user }).
                Where(roomAndUser => roomAndUser.Members.Contains(roomAndUser.user.Key)).
                Select(roomAndUser => new KeyValuePair<string, string>(roomAndUser.user.Key, $"{roomAndUser.user.Value.Nick}({roomAndUser.user.Key})")).ToList();
        }
    }      
}
