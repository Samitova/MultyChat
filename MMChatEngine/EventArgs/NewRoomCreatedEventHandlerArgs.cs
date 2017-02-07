namespace MMChatEngine
{
    public class NewRoomCreatedEventHandlerArgs
    {
        public NewRoomCreatedEventHandlerArgs(Room room)
        {
            Room = room;
        }

        public Room Room { get; }
    }
}