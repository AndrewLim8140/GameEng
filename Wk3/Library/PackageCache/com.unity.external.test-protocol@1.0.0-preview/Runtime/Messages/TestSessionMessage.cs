using System.Collections.Generic;

namespace Unity.TestProtocol.Messages
{
    public static class TestSessionMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(TestSessionMessage));

        public static Message CreateTestSessionStartedMessage(IEnumerable<string> minimalCommandLine)
        {
            var result = new Message(MessageType, Message.BeginPhase);
            result["minimalCommandLine"] = minimalCommandLine;
            return result;
        }

        public static Message CreateTestSessionCompletedMessage()
        {
            var result = new Message(MessageType, Message.EndPhase);
            return result;
        }
    }
}
