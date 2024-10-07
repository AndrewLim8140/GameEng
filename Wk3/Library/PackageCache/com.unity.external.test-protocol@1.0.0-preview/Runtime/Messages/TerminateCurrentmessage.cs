namespace Unity.TestProtocol.Messages
{
    /// <summary>
    /// This messages used forcibly terminate another message of any type.
    /// Used to handle timeout and other type of error
    /// </summary>
    public static class TerminateCurrentMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(TerminateCurrentMessage));

        public static Message Create()
        {
            var message = new Message(MessageType, Message.EndPhase);
            return message;
        }
    }
}
