using System;

namespace MMChatEngine
{
    public enum Sex
    {
        Unknown,
        Male,
        Female
    }

    public class UserInfo
    {
        public string Nick { get; }
        public Sex Sex { get; }
        public DateTime Birthdate { get; }

        public UserInfo(string nick, Sex sex, DateTime birthdate)
        {
            Nick = nick;
            Sex = sex;
            Birthdate = birthdate;
        }       
    }

    public class UserInfoWithPrivateInfo : UserInfo
    {
        public string Password { get; }

        public UserInfoWithPrivateInfo(string password, string nick, Sex sex, DateTime birthdate) : base(nick, sex, birthdate)
        {
            Password = password;
        }       
    }
}
