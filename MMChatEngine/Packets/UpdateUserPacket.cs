using System;
using System.IO;

namespace MMChatEngine.Packets
{
    class UpdateUserPacket : PacketBase
    {
        public string Login { get; private set; }
        public UserInfoWithPrivateInfo UserInfoWithPrivateInfo { get; private set; }

        public UpdateUserPacket(Stream stream, string login = "", UserInfoWithPrivateInfo userInfoWithPrivateInfo = null) : base(PacketType.UpdateUser, stream)
        {
            Login = login;
            UserInfoWithPrivateInfo = userInfoWithPrivateInfo;
        }

        public override void Receive()
        {
            Login = _streamReader.ReadString();
            string password = _streamReader.ReadString();
            string nick = _streamReader.ReadString();
            Sex sex = (Sex)_streamReader.ReadInt32();
            DateTime birthdate = DateTime.FromBinary(_streamReader.ReadInt64());
            UserInfoWithPrivateInfo = new UserInfoWithPrivateInfo(password, nick, sex, birthdate);
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write(Login);
            _streamWriter.Write(UserInfoWithPrivateInfo.Password);
            _streamWriter.Write(UserInfoWithPrivateInfo.Nick);
            _streamWriter.Write((int)UserInfoWithPrivateInfo.Sex);
            _streamWriter.Write(UserInfoWithPrivateInfo.Birthdate.ToBinary());
        }
    }
}
