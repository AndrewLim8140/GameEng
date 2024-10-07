using System;

namespace Unity.TestProtocol.Messages
{
    public static class TestSuiteMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(TestSuiteMessage));

        public static Message CreateTestSuiteStartedMessage(string name, string platform, string scope, string[] minimalCommandLine, bool supportsFilters)
        {
            var result = new Message(MessageType, Message.BeginPhase);
            result["name"] = name;
            result["scope"] = scope;
            result["platform"] = platform;
            result["supportsFilters"] = supportsFilters;
            result["minimalCommandLine"] = minimalCommandLine;

            return result;
        }

        public static Message CreateTestSuiteStartedMessage(string name, string platform, string scope, string[] minimalCommandLine, bool supportsFilters, bool isRerun)
        {
            var message = CreateTestSuiteStartedMessage(name, platform, scope, minimalCommandLine, supportsFilters);
            message["isRerun"] = isRerun;

            return message;
        }

        public static Message CreateTestSuiteCompletedMessage(string name)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["name"] = name;
            return result;
        }

        public static Message CreateTestSuiteCompletedMessage(string name, string scope,
            string platform, long duration, string[] minimalCommandLine)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["name"] = name;
            result["duration"] = duration;
            result["scope"] = scope;
            result["platform"] = platform;
            result["minimalCommandLine"] = minimalCommandLine;
            return result;
        }

        public static Message CreateTestSuiteCompletedMessage(string name,  string scope, string platform, long duration, string[] minimalCommandLine, Exception ex)
        {
            var result = CreateTestSuiteCompletedMessage(name, scope, platform, duration, minimalCommandLine);
            result.AddError(ex.Message);
            return result;
        }
    }
}
