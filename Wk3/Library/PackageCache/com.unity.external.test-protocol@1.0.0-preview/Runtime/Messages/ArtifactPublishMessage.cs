namespace Unity.TestProtocol.Messages
{
    public static class ArtifactPublishMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(ArtifactPublishMessage));

        public static Message Create(string desination)
        {
            var result = new Message(MessageType);
            result["destination"] = desination;
            return result;
        }
    }
}
