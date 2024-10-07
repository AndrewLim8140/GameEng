namespace Unity.TestProtocol.Messages
{
    public static class ErrorMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(ErrorMessage));
        public static Message Create(string message)
        {
            var result = new Message(MessageType);
            result["message"] = message;
            return result;
        }
    }

    public static class ErrorPartialMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(ErrorPartialMessage));

        public static Message Create(string message)
        {
            var result = new Message(MessageType);
            result["message"] = message;
            return result;
        }
    }
}
