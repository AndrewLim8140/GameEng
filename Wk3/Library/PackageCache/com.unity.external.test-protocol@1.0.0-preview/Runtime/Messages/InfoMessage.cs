namespace Unity.TestProtocol.Messages
{
    public static class InfoMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(InfoMessage));

        public static Message Create(string message)
        {
            var result = new Message(MessageType);
            result["message"] = message;
            return result;
        }
    }

    public static class InfoPartialMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(InfoPartialMessage));

        public static Message Create(string message)
        {
            var result = new Message(MessageType);
            result["message"] = message;
            return result;
        }
    }
}
