using System;
using System.Collections.Generic;
using System.IO;

namespace MMChatEngine.Packets
{
    public class AddUsersToRoomPacket : PacketBase
    {
        public Guid RoomId { get; private set; }
        public List<string> Users { get; private set; }

        public AddUsersToRoomPacket(Stream stream, Guid roomId = default(Guid), List<string> users = null) : base(PacketType.AddUserToRoom, stream)
        {
            RoomId = roomId;
            Users = users;
        }

        public override void Receive()
        {
            RoomId = new Guid(_streamReader.ReadBytes(16));
            int userCount = _streamReader.ReadInt32();
            Users = new List<string>(userCount);
            for (int i = 0; i < userCount; i++)
            {
                Users.Add(_streamReader.ReadString());
            }
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(RoomId.ToByteArray());
            _streamWriter.Write(Users.Count);
            foreach (string user in Users)
            {
                _streamWriter.Write(user);
            }
            _streamWriter.Flush();
        }
    }
}