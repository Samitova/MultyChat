using System.IO;

namespace MMChatEngine.Packets
{
    class NegotiationPacket : PacketBase
    {
        public string Login { get; private set; }

        public NegotiationPacket(Stream stream, string login = "") : base(PacketType.Negotiation, stream)
        {
            Login = login;
        }

        public override void Receive()
        {
            Login = _streamReader.ReadString();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(Login);
            _streamWriter.Flush();
        }
    }
}