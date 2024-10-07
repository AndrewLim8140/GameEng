namespace Unity.TestProtocol.Messages
{
    public class TestContextMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(TestContextMessage));

        public static Message Create()
        {
            return new Message(MessageType);
        }
    }
}
