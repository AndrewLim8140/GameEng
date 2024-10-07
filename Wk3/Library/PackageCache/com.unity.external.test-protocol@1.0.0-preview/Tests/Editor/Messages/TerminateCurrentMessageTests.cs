using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class TerminateCurrentMessageTests
    {
        [Test]
        public void Create_NoDuration_CreatesExpectedMessage()
        {
            var msg = TerminateCurrentMessage.Create();

            Assert.That(msg.messageType, Is.EqualTo(TerminateCurrentMessage.MessageType));
            Assert.That(msg.phase, Is.EqualTo(Message.EndPhase));
        }
    }
}
