namespace Unity.TestProtocol.Messages
{
    public static class WarningMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(WarningMessage));

        public static Message Create(string message)
        {
            var result = new Message(MessageType);
            result["message"] = message;
            return result;
        }
    }
}
