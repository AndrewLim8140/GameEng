using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class CompilerMessageTests
    {
        [Test]
        public void CanCreateLogEntryMessage()
        {
            var message = CompilerMessage.Create("Error", "message", "stacktrace", 123, "fileName");

            Assert.That(message["type"], Is.EqualTo(CompilerMessage.MessageType));
            Assert.That(message["severity"], Is.EqualTo("Error"));
            Assert.That(message["stacktrace"], Is.EqualTo("stacktrace"));
            Assert.That(message["line"], Is.EqualTo(123));
            Assert.That(message["file"], Is.EqualTo("fileName"));
        }
    }
}
