using System.Collections.Generic;

namespace Unity.TestProtocol.Messages
{
    public static class TestPlanMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(TestPlanMessage));

        public static Message Create(IEnumerable<string> tests)
        {
            var result = new Message(MessageType);
            if (tests != null)
            {
                result["tests"] = new List<string>(tests);
            }
            else
            {
                result["tests"] = new List<string>();
            }

            return result;
        }
    }
}
