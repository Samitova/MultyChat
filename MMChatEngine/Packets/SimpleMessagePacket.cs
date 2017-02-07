using System;
using System.IO;

namespace MMChatEngine.Packets
{
    public class SimpleMessagePacket : PacketBase
    {
        public Guid Room { get; private set; }
        public string UserLogin { get; private set; }
        public DateTime DateTime { get; private set; }
        public string Message { get; private set; }

        public SimpleMessagePacket(Stream stream, Guid room = default(Guid), string userLogin = "", DateTime dateTime = default(DateTime), string message = "") : base(PacketType.SimpleMessage, stream)
        {
            Room = room;
            UserLogin = userLogin;
            DateTime = dateTime;
            Message = message;
        }

        public override void Receive()
        {
            Room = new Guid(_streamReader.ReadBytes(16));
            UserLogin = _streamReader.ReadString();
            DateTime = DateTime.FromBinary(_streamReader.ReadInt64());
            Message = _streamReader.ReadString();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(Room.ToByteArray());
            _streamWriter.Write(UserLogin);
            _streamWriter.Write(DateTime.Now.ToBinary());
            _streamWriter.Write(Message);
            _streamWriter.Flush();
        }
    }
}