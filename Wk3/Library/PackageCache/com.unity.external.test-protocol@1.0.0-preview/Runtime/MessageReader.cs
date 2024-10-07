using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.TestProtocol
{
    public static class MessageReader
    {
        public static IEnumerable<Message> Read(TextReader reader, Predicate<Message> predicate)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("##utp:"))
                {
                    var message = UnityTestProtocolMessageBuilder.Deserialize(line);
                    if (predicate(message))
                    {
                        yield return message;
                    }
                }
            }
        }

        public static IEnumerable<Message> Read(TextReader reader)
        {
            return Read(reader, m => true);
        }
    }
}
