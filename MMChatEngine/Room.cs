using System;
using System.Collections.Generic;

namespace MMChatEngine
{
    public class Room
    {
        public Guid Id { get; }
        public string Name { get; }
        public HashSet<string> Members { get; }

        public static readonly Guid MainRoomId = new Guid("{9A1500F5-5EFF-4EB9-8A01-2643F2C19B68}");
        public static readonly string MainRoomName = "Main";

        public Room(Guid id, string name, HashSet<string> members)
        {
            Id = id;
            Name = name;
            Members = members;
        }     
    }
}