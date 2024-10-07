namespace Unity.TestProtocol.Messages
{
    public static class HintMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(HintMessage));

        public static Message Create(string message)
        {
            var result = new Message(MessageType);
            result["message"] = message;
            return result;
        }
    }
}
