using System;

namespace Unity.TestProtocol.Messages
{
    public static class ActionMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(ActionMessage));

        public static Message CreateActionStartedMessage(string name, string description)
        {
            var result = new Message(MessageType, Message.BeginPhase);
            result["name"] = name;
            result["description"] = description;
            return result;
        }

        public static Message CreateActionCompletedMessage(string name, string description, long duration)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["name"] = name;
            result["description"] = description;
            result["duration"] = duration;
            return result;
        }

        public static Message CreateActionFailedMessage(string name, string description, Exception ex, long duration)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["name"] = name;
            result["description"] = description;
            result.AddError(ex);
            result["duration"] = duration;
            return result;
        }
    }
}
