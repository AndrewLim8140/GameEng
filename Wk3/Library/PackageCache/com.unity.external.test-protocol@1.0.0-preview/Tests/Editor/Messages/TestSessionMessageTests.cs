using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class TestSessionMessageTests
    {
        [Test]
        public void CanCreateTestSessionStartedMessage()
        {
            var msg = TestSessionMessage.CreateTestSessionStartedMessage(new[] {"--suite=suiteName"});

            Assert.That(msg.Is(TestSessionMessage.MessageType));
            Assert.That(msg.phase, Is.EqualTo(Message.BeginPhase));
            Assert.That(msg["minimalCommandLine"], Is.EqualTo(new[] {"--suite=suiteName"}));
        }

        [Test]
        public void CanCreateTestSessionCompletedMessage()
        {
            var msg = TestSessionMessage.CreateTestSessionCompletedMessage();

            Assert.That(msg.Is(TestSessionMessage.MessageType));
            Assert.That(msg.phase, Is.EqualTo(Message.EndPhase));
            Assert.That(msg.HasErrors(), Is.False);
        }
    }
}
