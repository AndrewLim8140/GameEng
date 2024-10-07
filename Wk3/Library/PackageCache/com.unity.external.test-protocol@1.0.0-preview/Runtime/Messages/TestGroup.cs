namespace Unity.TestProtocol.Messages
{
    public static class TestGroupMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(TestGroupMessage));

        public static Message CreateGroupStart(string name)
        {
            var result = new Message(MessageType, Message.BeginPhase);
            result["name"] = name;
            return result;
        }

        public static Message CreateGroupEnd(string name)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["name"] = name;
            return result;
        }
    }
}
