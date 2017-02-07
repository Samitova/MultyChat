using System.IO;

namespace MMChatEngine.Packets
{
    public class LoginPacket : PacketBase 
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public LoginPacket(Stream stream, string login = "", string password ="") : base(PacketType.Login, stream)
        {
            Login = login;
            Password = password;
        }

        public override void Receive()
        {
            Login = _streamReader.ReadString();
            Password = _streamReader.ReadString();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(Login);
            _streamWriter.Write(Password);
            _streamWriter.Flush();
        }
    }
}