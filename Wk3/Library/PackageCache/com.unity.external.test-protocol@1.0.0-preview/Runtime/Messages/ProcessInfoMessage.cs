using System.Collections.Generic;

namespace Unity.TestProtocol.Messages
{
    public static class ProcessInfoMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(ProcessInfoMessage));

        public static Message CreateProcessBeginMessage(int id, string path, string processArguments, IEnumerable<string> logs)
        {
            var result = new Message(MessageType, Message.BeginPhase);
            return Create(id, path, processArguments, logs, result);
        }

        public static Message CreateProcessEnd(int id)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["id"] = id;
            return result;
        }

        public static Message CreateProcessEndMessage(int id, string path, string processArguments, IEnumerable<string> logs)
        {
            var result = new Message(MessageType, Message.EndPhase);
            return Create(id, path, processArguments, logs, result);
        }

        static Message Create(int id, string path, string processArguments, IEnumerable<string> logs, Message result)
        {
            result["id"] = id;
            result["path"] = path;
            if (!string.IsNullOrEmpty(processArguments))
            {
                result["arguments"] = processArguments;
            }
            result["logs"] = new List<string>(logs);
            return result;
        }
    }
}
