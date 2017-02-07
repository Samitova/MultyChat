using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace MMChatEngine
{
    public enum PacketType
    {
        Registry = 1,
        Login,
        Negotiation,
        Error,
        UserConnect,
        UserDisconnect,
        Connect,
        Disconnect,
        SimpleMessage,
        CreateNewRoom,
        AddUserToRoom,
        Ping,
        CloseRoom,
        RemoveUserFromRoom,
        UpdateUser
    }

    public enum ErrorType
    {
        None,
        IncorrectLoginOrPassword,
        UserAlreadyExist,
        UserAlreadyLogin,
        ClientRecieveMassege,
        ServerListener
    }

    public enum RequestToServerResult
    {
        OK,
        Failed,
        Disconnect
    }
}
