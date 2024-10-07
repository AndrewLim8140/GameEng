using System.Collections.Generic;

namespace Unity.TestProtocol.Messages
{
    public class AssemblyCompilationErrors
    {
        public static string MessageType = Message.GetDefaultMessageTypeValue(typeof(AssemblyCompilationErrors));

        public static Message Create(string assembly, IEnumerable<string> errors)
        {
            var result = new Message(MessageType);
            result["assembly"] = assembly;
            result["errors"] = new List<string>(errors);
            return result;
        }
    }
}
