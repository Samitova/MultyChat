using System.IO;

namespace MMChatEngine.Packets
{
    public abstract class PacketBase
    {
        protected readonly PacketType _packetType;
        protected readonly BinaryReader _streamReader;
        protected BinaryWriter _streamWriter;

        protected PacketBase(PacketType packetType, Stream stream)
        {
            _packetType = packetType;
            _streamReader = new BinaryReader(stream);
            _streamWriter = new BinaryWriter(stream);
        }

        public abstract void Receive();

        public virtual void Send(Stream stream = null)
        {
            if (stream != null)
            {
                _streamWriter = new BinaryWriter(stream);
            }

            _streamWriter.Write((int)_packetType);
        }
    }
}