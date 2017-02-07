using System;
using System.Collections.Generic;
using System.IO;

namespace MMChatEngine.Packets
{
    public class CreateNewRoomPacket : PacketBase
    {
        public Room Room { get; private set; }

        public CreateNewRoomPacket(Stream stream, Room room = null) : base(PacketType.CreateNewRoom, stream)
        {
            Room = room;
        }

        public override void Receive()
        {
            Guid id = new Guid(_streamReader.ReadBytes(16));
            string name = _streamReader.ReadString();
            int memberCount = _streamReader.ReadInt32();
            HashSet<string> members = new HashSet<string>();
            for (int i = 0; i < memberCount; i++)
            {
                members.Add(_streamReader.ReadString());
            }
            Room = new Room(id, name, members);
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(Room.Id.ToByteArray());
            _streamWriter.Write(Room.Name);
            _streamWriter.Write(Room.Members.Count);
            foreach (string member in Room.Members)
            {
                _streamWriter.Write(member);
            }
            _streamWriter.Flush();
        }
    }
}