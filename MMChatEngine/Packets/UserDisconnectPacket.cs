using System.IO;

namespace MMChatEngine.Packets
{
    public class UserDisconnectPacket : PacketBase
    {
        public string UserLogin { get; private set; }

        public UserDisconnectPacket(Stream stream, string userLogin = "") : base(PacketType.UserDisconnect, stream)
        {
            UserLogin = userLogin;
        }

        public override void Receive()
        {
            UserLogin = _streamReader.ReadString();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(UserLogin);
        }
    }
}