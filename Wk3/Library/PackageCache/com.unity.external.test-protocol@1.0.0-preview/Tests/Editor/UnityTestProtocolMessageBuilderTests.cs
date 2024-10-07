using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Unity.TestProtocol.UnitTests
{
    [TestFixture]
    public class UnityTestProtocolMessageBuilderTests
    {
        [Test]
        public void CanDeserializeMessage()
        {
            var msg = new Message("Dummy");
            msg["payload_key1"] = "1";
            msg["payload_key2"] = 2;

            var deserialized = GetMessage(UnityTestProtocolMessageBuilder.Serialize(msg));

            Assert.That(deserialized["type"], Is.EqualTo("Dummy"));
            Assert.That(deserialized["time"], Is.EqualTo(msg.GetLong("time")));
            Assert.That(deserialized["version"], Is.EqualTo(msg.GetInt("version")));
            Assert.That(deserialized["payload_key1"], Is.EqualTo("1"));
            Assert.That(deserialized["payload_key2"], Is.EqualTo(2));
        }

        [Test]
        public void FieldsSerializedInCertainOrder()
        {
            var msg = new Message("Dummy");
            msg["ykey"] = "y";
            msg["xkey"] = "x";

            var str = UnityTestProtocolMessageBuilder.Serialize(msg);
            var deserialized = UnityTestProtocolMessageBuilder.Deserialize(str);

            var keys = deserialized.data.Keys.Cast<string>();
            Assert.That(keys.Skip(keys.Count() - 2), Is.EqualTo(new[] { "ykey", "xkey" }));
        }

        [Test]
        public void CanDeserializeMessageStartsWithEndOfLine()
        {
            var msg = new Message("Dummy");
            msg["ykey"] = "y";
            msg["xkey"] = "x";

            var str = UnityTestProtocolMessageBuilder.Serialize(msg);
            var deserialized = UnityTestProtocolMessageBuilder.Deserialize($"\n{str}");

            var keys = deserialized.data.Keys.Cast<string>();
            Assert.That(keys.Skip(keys.Count() - 2), Is.EqualTo(new[] { "ykey", "xkey" }));
        }


        [Test]
        public void CanDeserializeErrorsField()
        {
            var msg = new Message("Dummy");
            msg.AddError("error1");
            msg.AddError("error2");

            var str = UnityTestProtocolMessageBuilder.Serialize(msg);
            var deserialized = UnityTestProtocolMessageBuilder.Deserialize(str);

            Assert.That(deserialized.GetErrors(), Is.EqualTo(new[] { "error1", "error2" }));
        }

        static IDictionary GetMessage(string line)
        {
            line = line.Remove(0, "##utp:".Length);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
        }
    }
}
