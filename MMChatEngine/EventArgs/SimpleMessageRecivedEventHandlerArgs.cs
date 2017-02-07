using System;

namespace MMChatEngine
{
    public class SimpleMessageRecivedEventHandlerArgs
    {
        public SimpleMessageRecivedEventHandlerArgs(Guid room, string userLogin, DateTime dateTime, string message)
        {
            Room = room;
            UserLogin = userLogin;
            DateTime = dateTime;
            Message = message;
        }

        public Guid Room { get; }
        public string UserLogin { get; }
        public DateTime DateTime { get; }
        public string Message { get; }
    }
}