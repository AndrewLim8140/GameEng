using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol
{
    public static class Check
    {
        public static bool IsBeginMessage(this Message msg)
        {
            return msg.phase == Message.BeginPhase;
        }

        public static bool IsEndMessage(this Message msg)
        {
            return msg.phase == Message.EndPhase;
        }

        public static bool IsCompleteMessage(this Message msg)
        {
            return msg.phase == Message.CompletePhase;
        }

        public static bool IsImmediate(this Message msg)
        {
            return msg.phase == Message.ImmediatePhase;
        }

        public static bool IsActionMessage(this Message msg)
        {
            return msg.Is(ActionMessage.MessageType);
        }

        public static bool IsActionBeginMessage(this Message msg)
        {
            return IsActionMessage(msg) && msg.IsBeginMessage();
        }

        private static bool IsActionMessage(this Message msg, string name, string phase)
        {
            return IsActionMessage(msg) && msg.GetString("name") == name && msg.phase == phase;
        }

        public static bool IsActionBeginMessage(this Message msg, string name)
        {
            return IsActionBeginMessage(msg) && msg.GetString("name") == name;
        }

        public static bool IsActionEndMessage(this Message msg)
        {
            return IsActionMessage(msg) && msg.phase == Message.EndPhase;
        }

        public static bool IsActionEndMessage(this Message msg, string name)
        {
            return IsActionMessage(msg, name, Message.EndPhase);
        }

        public static bool IsActionCompletedWithError(this Message msg)
        {
            return IsActionMessage(msg) && msg.phase == Message.EndPhase && msg.HasErrors();
        }

        public static bool IsTestStart(this Message msg)
        {
            return msg.Is(TestStatusMessage.MessageType) && msg.IsBeginMessage();
        }

        public static bool IsTestFinish(this Message msg)
        {
            return msg.Is(TestStatusMessage.MessageType) && msg.IsEndMessage();
        }

        public static bool IsTestSuiteEnd(this Message msg)
        {
            return msg.Is(TestSuiteMessage.MessageType) && msg.IsEndMessage();
        }

        public static bool IsTestFinish(this Message msg, TestStateEnum state)
        {
            return msg.IsTestFinish() && msg.GetInt("state") == (int)state;
        }

        public static bool IsTestSuiteStart(this Message msg)
        {
            return msg.Is(TestSuiteMessage.MessageType) && msg.IsBeginMessage();
        }

        public static bool IsTestFailedMessage(this Message msg)
        {
            if (!msg.Is(TestStatusMessage.MessageType) || msg.IsBeginMessage())
            {
                return false;
            }

            var state = (TestStateEnum)int.Parse(msg["state"].ToString());
            return (state == TestStateEnum.Cancelled ||
                state == TestStateEnum.Error ||
                state == TestStateEnum.Failure ||
                state == TestStateEnum.Inconclusive);
        }

        public static bool IsSuiteFailedMessage(this Message msg)
        {
            return msg.Is(TestSuiteMessage.MessageType) && msg.HasErrors();
        }

        public static bool IsDurationEqualTo(this Message msg, long expected)
        {
            var actual = long.Parse(msg.data["duration"].ToString());
            return actual == expected;
        }
    }
}
