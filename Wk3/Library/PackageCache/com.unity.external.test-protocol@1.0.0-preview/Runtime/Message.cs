using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol
{
    [DebuggerDisplay("{" + nameof(DebugValue) + "}")]
    public sealed class Message
    {
        public const int kProtocolVersion = 2;

        public const string BeginPhase = "Begin";
        public const string EndPhase = "End";
        public const string CompletePhase = "Complete";
        public const string ImmediatePhase = "Immediate";

        public Message(string type, string phase = ImmediatePhase)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new Exception("'type' must be specified when constructing Unity Test Protocol Messages");
            }
            data = new OrderedDictionary();
            data["type"] = type;
            data["time"] = GetUtcNowMs();
            data["phase"] = phase;
            data["version"] = kProtocolVersion;
            data["processId"] = Process.GetCurrentProcess().Id;
            data["errors"] = new List<string>();
        }

        public Message(IDictionary messageData)
        {
            if (messageData["type"] == null || messageData["type"].ToString() == string.Empty)
            {
                throw new Exception("'type' must be specified when constructing Unity Test Protocol Messages");
            }
            data = new OrderedDictionary();
            data["type"] = messageData["type"];
            data["time"] = GetUtcNowMs();
            data["version"] = kProtocolVersion;
            foreach (var entry in messageData)
            {
                var e = (DictionaryEntry)entry;
                if (e.Key.ToString() == "errors" && e.Value != null && e.Value is IEnumerable<object>)
                {
                    var errors = new List<string>();
                    foreach (var error in e.Value as IEnumerable<object>)
                    {
                        errors.Add(error.ToString());
                    }
                    data[e.Key] = errors;
                }
                else
                {
                    data[e.Key] = e.Value;
                }
            }

            if (!data.Contains("errors"))
            {
                this["errors"] = new List<string>();
            }
            else if (!(this["errors"] is List<string>))
            {
                throw new InvalidOperationException("Unexpected value in errors field. " +
                    $"Expected List<string>, but actual type was '{this["errors"].GetType().Name}'" +
                    $"Check the code responsible for serialization of message with timestamp: {data["time"]} and type {data["type"]}"
                );
            }
        }

        public IOrderedDictionary data { get; }

        static readonly DateTime k_Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime();

        private static long GetUtcNowMs()
        {
            return Convert.ToInt64((DateTime.UtcNow - k_Epoch).TotalMilliseconds);
        }

        public string messageType { get { return data["type"].ToString(); } }

        public static string GetDefaultMessageTypeValue(Type type)
        {
            var typeName = type.Name;
            var messageSuffixIdx = typeName.LastIndexOf("Message");
            return messageSuffixIdx > 0 ? typeName.Remove(messageSuffixIdx, "Message".Length) : typeName;
        }

        public string phase { get { return (string)this["phase"]; } }

        public object this[string fieldName]
        {
            get { return data[fieldName]; }
            set { data[fieldName] = value; }
        }

        public bool IsEndMessageFor(Message other)
        {
            // ReSharper disable once UnusedVariable
            string reason;
            return IsEndMessageFor(other, out reason);
        }

        /// <summary>
        /// Should only be used with begin-end message types, such as:
        /// Action, TestSuite, TestSession, ProcessInfo, TestGroup.
        /// </summary>
        public Message End()
        {
            if (this["phase"].ToString() != BeginPhase)
                throw new InvalidOperationException($"Only a message with 'Begin' phase can have an 'End' message pair instantiated. Current {this["type"]} message has {this["phase"]} phase.");

            var endMessage = new Message(data);
            endMessage["phase"] = EndPhase;
            endMessage["time"] = GetUtcNowMs();
            endMessage["duration"] = endMessage.GetLong("time") - this.GetLong("time");

            return endMessage;
        }

        /// <summary>
        /// Should only be used with begin-end message types, such as:
        /// Action, TestSuite, TestSession, ProcessInfo, TestGroup.
        /// </summary>
        public Message End(Exception exception)
        {
            var endMessage = this.End();
            endMessage.AddError(exception);
            return endMessage;
        }

        private bool IsEndMessageFor(Message other, out string reason)
        {
            reason = null;

            if (other.Is(TerminateCurrentMessage.MessageType))
            {
                return true;
            }

            if (messageType != other.messageType)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Message of type '{other.messageType}' cannot enclose message of type '{messageType}'");
                sb.AppendLine($"This error indicates that test execution went wrong and expected message of type `{messageType}`  was not printed.");
                sb.AppendLine($"Most likely it is caused by a crash of process with id {GetInt("processId")}");
                sb.AppendLine("Diagnostics");
                sb.AppendLine($"Message1 details: {GetMessageDetails(other)}");
                sb.AppendLine($"Message2 details: {GetMessageDetails(this)}");
                reason = sb.ToString();
                return false;
            }

            if (HasField("name") && GetString("name") != other.GetString("name"))
            {
                var name = GetString("name");
                var otherName = other.GetString("name");
                reason = $"Message of type '{messageType}' with name '{name}' can not be merged with message of type '{other.messageType}' with name '{otherName}'";
                return false;
            }

            if (!other.IsEndMessage())
            {
                reason = $"Message with phase 'other.phase' can not enclose message with phase {phase}";
                return false;
            }

            return true;
        }

        public Message MergeWith(Message other)
        {
            string reason;
            if (!IsEndMessageFor(other, out reason))
            {
                throw new UtpMessageMergeException(reason);
            }

            return EncloseWith(other);
        }

        private Message EncloseWith(Message endMessage)
        {
            var resultMessage = new Message(messageType);

            foreach (var key in this.data.Keys)
            {
                resultMessage[key.ToString()] = this.data[key];
            }

            foreach (var key in endMessage.data.Keys.Cast<string>())
            {
                if (key == "errors")
                {
                    var errors = new List<string>();
                    if (resultMessage.HasErrors())
                    {
                        errors.AddRange(resultMessage.GetErrors());
                    }

                    if (endMessage.HasErrors())
                    {
                        errors.AddRange(endMessage.GetErrors());
                    }

                    resultMessage[key] = errors;
                }
                else
                {
                    resultMessage[key] = endMessage.data[key];
                }
            }

            var beginTime = this.GetLong("time");
            var endTime = endMessage.GetLong("time");

            if (endMessage.HasField("duration"))
            {
                resultMessage["duration"] = endMessage.GetLong("duration");
            }
            else
            {
                resultMessage["duration"] = endTime - beginTime;
            }

            resultMessage["type"] = messageType;
            resultMessage["phase"] = CompletePhase;

            return resultMessage;
        }

        public void AddError(Exception ex)
        {
            AddError(ex.ToString());
        }

        public void AddError(string error)
        {
            var errors = (List<string>) this["errors"];
            errors.Add(error);
        }

        public IEnumerable<string> GetErrors()
        {
            return (List<string>) this["errors"];
        }

        public bool HasErrors()
        {
            return GetErrors().Any();
        }

        string DebugValue
        {
            // ReSharper disable once UnusedMember.Local
            get
            {
                var sb = new StringBuilder();
                sb.Append($"Type: '{messageType}', phase: '{phase}'");
                if (HasField("name"))
                {
                    sb.Append($", name: {GetString("name")}");
                }

                if (HasField("description"))
                {
                    sb.Append($", description: '{GetString("description")}'");
                }

                return sb.ToString();
            }
        }

        public bool Is(string type)
        {
            return messageType == type;
        }

        public int GetInt(string fieldName)
        {
            return Convert.ToInt32(this[fieldName]);
        }

        public long GetLong(string fieldName)
        {
            return Convert.ToInt64(data[fieldName]);
        }

        public bool GetBool(string fieldName)
        {
            return Convert.ToBoolean(data[fieldName]);
        }

        public bool HasField(string fieldName)
        {
            return data.Contains(fieldName);
        }

        public string GetString(string fieldName)
        {
            var value = this[fieldName];
            return value?.ToString();
        }

        private static string GetMessageDetails(Message msg)
        {
            return $"{msg.DebugValue}, ts:'{msg.GetLong("time")}'";
        }
    }

    [Serializable]
    public class UtpMessageMergeException : Exception
    {
        public UtpMessageMergeException(string msg)
            : base(msg)
        {
        }
    }
}
