namespace MMChatEngine
{
    public class ErrorOccurredEventHandlerArgs
    {
        public ErrorOccurredEventHandlerArgs(string errorMessage, ErrorType errorType)
        {
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public string ErrorMessage { get; }
        public ErrorType ErrorType { get; }
    }
}