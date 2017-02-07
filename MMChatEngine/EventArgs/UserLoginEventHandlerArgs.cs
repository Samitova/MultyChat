namespace MMChatEngine
{
    public class UserLoginEventHandlerArgs
    {
        public UserLoginEventHandlerArgs(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }
    }
}