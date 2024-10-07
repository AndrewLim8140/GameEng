using System;

namespace Unity.TestProtocol.Messages
{
    public static class TestStatusMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(TestStatusMessage));

        public static Message CreateTestStartMesssage(string name)
        {
            var result = new Message(MessageType, Message.BeginPhase);
            result["name"] = name;
            result["state"] = (int)TestStateEnum.Inconclusive;
            return result;
        }

        public static Message CreateTestEndMesssage(TestInfo info)
        {
            var result = new Message(MessageType, Message.EndPhase);
            result["name"] = info.name;
            result["state"] = (int)info.state;
            result["message"] = info.message;
            result["duration"] = Convert.ToInt64(info.durationMicroseconds / 1000);
            result["durationMicroseconds"] = info.durationMicroseconds;
            result["stack-trace"] = info.stackTrace;
            result["classname"] = info.classname;
            result["stackTrace"] = info.stackTrace;
            return result;
        }

        public static TestInfo ToTestInfo(Message msg)
        {
            var duration = msg.GetLong("durationMicroseconds");
            if (duration == 0)
            {
                duration = msg.GetLong("duration") * 1000;
            }

            var result = new TestInfo(
                msg.GetString("name"),
                (TestStateEnum)msg.GetInt("state"),
                msg.GetString("message"),
                duration,
                msg.GetString("stackTrace"),
                msg.GetString("className")
                ) { errors = msg.GetErrors() };
            return result;
        }
    }
}
