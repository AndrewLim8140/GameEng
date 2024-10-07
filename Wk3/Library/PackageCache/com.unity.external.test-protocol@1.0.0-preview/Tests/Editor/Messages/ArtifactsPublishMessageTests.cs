using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class ArtifactsPublishMessageTests
    {
        [Test]
        public void CanCreateArtifactsPublishMessage()
        {
            var destination = "/dev/null";

            var msg = ArtifactPublishMessage.Create(destination);

            Assert.That(msg.messageType, Is.EqualTo(ArtifactPublishMessage.MessageType));
            Assert.That(msg.phase, Is.EqualTo(Message.ImmediatePhase));
            Assert.That(msg["destination"], Is.EqualTo(destination));
        }
    }
}
