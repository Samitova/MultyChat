using System;
using System.IO;

namespace MMChatEngine.Packets
{
    public class RemoveUserFromRoomPacket : PacketBase
    {
        public Guid RoomId { get; private set; }
        public string User { get; private set; }

        public RemoveUserFromRoomPacket(Stream stream, Guid roomId = default(Guid), string user = "") : base(PacketType.RemoveUserFromRoom, stream)
        {
            RoomId = roomId;
            User = user;
        }

        public override void Receive()
        {
            RoomId = new Guid(_streamReader.ReadBytes(16));
            User = _streamReader.ReadString();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(RoomId.ToByteArray());
            _streamWriter.Write(User);
        }
    }
}