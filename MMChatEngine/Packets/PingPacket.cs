using System.IO;

namespace MMChatEngine.Packets
{
    public class PingPacket : PacketBase
    {
        public PingPacket(Stream stream) : base(PacketType.Ping, stream)
        {
        }

        public override void Receive()
        {
            throw new System.NotImplementedException();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
        }
    }
}