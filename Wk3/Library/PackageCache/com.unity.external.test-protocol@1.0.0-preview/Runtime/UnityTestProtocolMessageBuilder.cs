using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace Unity.TestProtocol
{
    public class UnityTestProtocolMessageBuilder
    {
        public static string Serialize(Message message)
        {
            var fields = new OrderedDictionary();
            fields["type"] = message.messageType;

            foreach (var entry in message.data)
            {
                var e = (DictionaryEntry)entry;
                fields[e.Key] = e.Value;
            }

            return BuildMessage(fields);
        }

        public static Message Deserialize(string str)
        {
            str = str.Trim();
            str = str.Remove(0, "##utp:".Length);
            var payloadData = JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            return new Message(payloadData);
        }

        static string BuildMessage(OrderedDictionary fields)
        {
            return "##utp:" + JsonConvert.SerializeObject(fields, Formatting.None);
        }
    }

    public class TestInfo
    {
        public TestInfo(string name,
                        TestStateEnum state,
                        string message,
                        long durationMicroseconds,
                        string stackTrace,
                        string className)
        {
            this.name = name;
            this.state = state;
            this.message = message;
            this.durationMicroseconds = durationMicroseconds;
            this.stackTrace = stackTrace;
            this.classname = className;
            artifacts = new List<string>();
            errors = new List<string>();
        }

        public string name { get; private set; }
        public TestStateEnum state { get; private set; }
        public string message { get; private set; }
        public long durationMicroseconds { get; private set; }
        public string stackTrace { get; private set; }
        public string classname { get; private set; }
        public IEnumerable<string> artifacts { get; set; }
        public IEnumerable<string> errors { get; set; }
    }

    public enum TestStateEnum
    {
        Inconclusive = 0,
        NotRunnable = 1,
        Skipped = 2,
        Ignored = 3,
        Success = 4,
        Failure = 5,
        Error = 6,
        Cancelled = 7
    }
}
