namespace MMChatEngine
{
    public class UserDisconnectedEventHandlerArgs
    {
        public UserDisconnectedEventHandlerArgs(string login)
        {
            Login = login;
        }

        public string Login { get; }
    }
}