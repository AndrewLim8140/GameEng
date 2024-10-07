namespace Unity.TestProtocol.Messages
{
    public static class CompilerMessage
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(CompilerMessage));

        public static Message Create(string severity, string message, string stackTrace, int line, string file)
        {
            var result = new Message(MessageType)
            {
                ["severity"] = severity,
                ["message"] = message,
                ["stacktrace"] = stackTrace,
                ["line"] = line,
                ["file"] = file
            };
            return result;
        }
    }
}
