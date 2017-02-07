using System;

namespace MMChatEngine
{
    public class RoomClosedEventHandlerArgs
    {
        public RoomClosedEventHandlerArgs(Guid roomId)
        {
            RoomId = roomId;
        }

        public Guid RoomId { get; }
    }
}