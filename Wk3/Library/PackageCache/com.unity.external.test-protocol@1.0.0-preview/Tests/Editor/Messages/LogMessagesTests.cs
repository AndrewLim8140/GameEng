using System.Collections.Generic;
using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class LogMessagesTests
    {
        [Test]
        public void CanCreateErrorMessage()
        {
            var message = ErrorMessage.Create("errorMessage");

            Assert.That(message["type"], Is.EqualTo(ErrorMessage.MessageType));
            Assert.That(message["message"], Is.EqualTo("errorMessage"));
        }

        [Test]
        public void CanCreateErrorPartialMessage()
        {
            var message = ErrorPartialMessage.Create("errorMessagePartial");

            Assert.That(message["type"], Is.EqualTo(ErrorPartialMessage.MessageType));
            Assert.That(message["message"], Is.EqualTo("errorMessagePartial"));
        }

        [Test]
        public void CanCreateInfoMessage()
        {
            var message = InfoMessage.Create("infoMessage");

            Assert.That(message["type"], Is.EqualTo(InfoMessage.MessageType));
            Assert.That(message["message"], Is.EqualTo("infoMessage"));
        }

        [Test]
        public void CanCreateInfoPartialMessage()
        {
            var message = InfoPartialMessage.Create("infoMessagePartial");

            Assert.That(message["type"], Is.EqualTo(message.messageType));
            Assert.That(message["message"], Is.EqualTo("infoMessagePartial"));
        }

        [Test]
        public void CanCreateWarningMessage()
        {
            var message = WarningMessage.Create("warningMessage");

            Assert.That(message["type"], Is.EqualTo(WarningMessage.MessageType));
            Assert.That(message["message"], Is.EqualTo("warningMessage"));
        }

        [Test]
        public void CanCreateMemoryLeaksMessage()
        {
            var message = MemoryLeaksMessage.Create(1234, new Dictionary<string, int> {{"lbl", 123}});

            Assert.That(message["type"], Is.EqualTo(MemoryLeaksMessage.MessageType));
            Assert.That(message["allocatedMemory"], Is.EqualTo(1234));
            Assert.That(message["memoryLabels"], Is.Not.Null);
            var memoryLabels = message["memoryLabels"] as Dictionary<string, int>;
            Assert.That(memoryLabels, Has.Count.EqualTo(1));
        }
    }
}
