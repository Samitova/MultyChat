namespace MMChatEngine
{
    public class NewUserRegisteredEventHandlerArgs
    {
        public NewUserRegisteredEventHandlerArgs(string login, UserInfo userInfo)
        {
            Login = login;
            UserInfo = userInfo;
        }

        public string Login { get; }
        public UserInfo UserInfo { get; }
    }
}