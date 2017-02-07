using System;
using System.IO;

namespace MMChatEngine.Packets
{
    public class UserConnectPacket : PacketBase
    {
        public string UserLogin { get; private set; }
        public UserInfo UserInfo { get; private set; }

        public UserConnectPacket(Stream stream, string userLogin = "", UserInfo userInfo = null) : base(PacketType.UserConnect, stream)
        {
            UserLogin = userLogin;
            UserInfo = userInfo;
        }

        public override void Receive()
        {
            UserLogin = _streamReader.ReadString();
            string nick = _streamReader.ReadString();
            Sex sex = (Sex)_streamReader.ReadInt32();
            DateTime birthdate = DateTime.FromBinary(_streamReader.ReadInt64());
            UserInfo = new UserInfo(nick, sex, birthdate);
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(UserLogin);
            _streamWriter.Write(UserInfo.Nick);
            _streamWriter.Write((int)UserInfo.Sex);
            _streamWriter.Write(UserInfo.Birthdate.ToBinary());
        }
    }
}