namespace MMChatEngine
{
    public class UserConnectedEventHandlerArgs
    {
        public UserConnectedEventHandlerArgs(string login, UserInfo userInfo)
        {
            Login = login;
            UserInfo = userInfo;
        }

        public string Login { get; }
        public UserInfo UserInfo { get; }
    }
}