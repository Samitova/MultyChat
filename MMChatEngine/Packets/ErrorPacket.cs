using System.IO;

namespace MMChatEngine.Packets
{
    public class ErrorPacket : PacketBase
    {
        public string ErrorMessage { get; private set; }
        public ErrorType ErrorType { get; private set; }

        public ErrorPacket(Stream stream, string errorMessage = "", ErrorType errorType = ErrorType.None) : base(PacketType.Error, stream)
        {
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public override void Receive()
        {
            ErrorType = (ErrorType) _streamReader.ReadInt32();
            ErrorMessage = _streamReader.ReadString();
        }

        public override void Send(Stream stream = null)
        {
            base.Send(stream);
            _streamWriter.Write((int) ErrorType);
            _streamWriter.Write(ErrorMessage);
            _streamWriter.Flush();
        }
    }
}