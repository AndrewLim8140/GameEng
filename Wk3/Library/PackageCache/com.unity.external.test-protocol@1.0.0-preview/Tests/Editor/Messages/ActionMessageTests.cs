using System;
using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class ActionMessageTests
    {
        [Test]
        public void ActionStartedMessageInitializedCorrectly()
        {
            var msg = ActionMessage.CreateActionStartedMessage("name", "description");

            Assert.That(msg.Is(ActionMessage.MessageType));
            Assert.That(msg["name"], Is.EqualTo("name"));
            Assert.That(msg["description"], Is.EqualTo("description"));
            Assert.That(msg["phase"], Is.EqualTo(Message.BeginPhase));
        }

        [Test]
        public void ActionCompletedMessageInitializedCorrectly()
        {
            var msg = ActionMessage.CreateActionCompletedMessage("name", "description", 123);

            Assert.That(msg.Is(ActionMessage.MessageType));
            Assert.That(msg["name"], Is.EqualTo("name"));
            Assert.That(msg["description"], Is.EqualTo("description"));
            Assert.That(msg["phase"], Is.EqualTo(Message.EndPhase));
            Assert.That(msg["duration"], Is.EqualTo(123));
        }

        [Test]
        public void ActionFailedMessageInitializedCorrectly()
        {
            var ex = new Exception("exceptionText");
            var msg = ActionMessage.CreateActionFailedMessage("name", "description", ex, 123);

            Assert.That(msg.Is(ActionMessage.MessageType));
            Assert.That(msg["name"], Is.EqualTo("name"));
            Assert.That(msg["description"], Is.EqualTo("description"));
            Assert.That(msg["phase"], Is.EqualTo(Message.EndPhase));
            Assert.That(msg["duration"], Is.EqualTo(123));
            Assert.That(msg.HasErrors());
        }
    }
}
