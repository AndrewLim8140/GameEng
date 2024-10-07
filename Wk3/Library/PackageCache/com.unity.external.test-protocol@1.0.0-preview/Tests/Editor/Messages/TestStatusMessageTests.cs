using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class TestSatusMessageTests
    {
        [Test]
        public void CanCreateTestStartMessage()
        {
            var msg = TestStatusMessage.CreateTestStartMesssage("Test1");

            Assert.That(msg.messageType, Is.EqualTo(TestStatusMessage.MessageType));
            Assert.That(msg["name"], Is.EqualTo("Test1"));
            Assert.That(msg["state"], Is.EqualTo((int)TestStateEnum.Inconclusive));
            Assert.That(msg.phase, Is.EqualTo(Message.BeginPhase));
        }

        [Test]
        public void CanCreateTestEndMessage()
        {
            const string name = "Test1";
            const TestStateEnum state = TestStateEnum.Failure;
            const string message = "epic failure";
            const int durationMicroseconds = 123456;
            const string stackTrace = "f1\\nf2";
            const string className = "SomeClazz";
            var testInfo = new TestInfo(
                name,
                state,
                message,
                durationMicroseconds,
                stackTrace,
                className
            );

            var msg = TestStatusMessage.CreateTestEndMesssage(testInfo);

            Assert.That(msg.Is(TestStatusMessage.MessageType));
            Assert.That(msg["name"], Is.EqualTo(name));
            Assert.That(msg["phase"], Is.EqualTo(Message.EndPhase));
            Assert.That(msg["state"], Is.EqualTo((int)TestStateEnum.Failure));
            Assert.That(msg["duration"], Is.EqualTo(durationMicroseconds / 1000));
            Assert.That(msg["durationMicroseconds"], Is.EqualTo(durationMicroseconds));
            Assert.That(msg["stackTrace"], Is.EqualTo(stackTrace));
            Assert.That(msg["classname"], Is.EqualTo(className));
        }
    }
}
