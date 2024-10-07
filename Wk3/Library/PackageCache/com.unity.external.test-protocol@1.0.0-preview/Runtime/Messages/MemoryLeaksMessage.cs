using System.Collections.Generic;

namespace Unity.TestProtocol.Messages
{
    public static class MemoryLeaksMessage
    {
        public static readonly string MessageType = Message.GetDefaultMessageTypeValue(typeof(MemoryLeaksMessage));

        public static Message Create(long allocatedMemory, IDictionary<string, int> memoryLabels)
        {
            var result = new Message(MessageType);

            result["allocatedMemory"] = allocatedMemory;
            result["memoryLabels"] = memoryLabels;

            return result;
        }
    }
}
