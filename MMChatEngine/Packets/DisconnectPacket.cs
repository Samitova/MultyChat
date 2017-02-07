using System.IO;

namespace MMChatEngine.Packets
{
    public class DisconnectPacket : PacketBase
    {
        public DisconnectPacket(Stream stream) : base(PacketType.Disconnect, stream)
        {
        }

        public override void Receive()
        {
            
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
        }
    }
}