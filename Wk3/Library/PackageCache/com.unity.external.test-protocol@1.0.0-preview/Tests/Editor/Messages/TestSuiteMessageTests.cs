using System;
using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class TestSuiteMessageTests
    {
        [Test]
        public void CanCreateTestSuiteStartedMessage()
        {
            var msg = TestSuiteMessage.CreateTestSuiteStartedMessage("suiteName", "platformName", "suiteScope", new[] {"test"}, true);

            Assert.That(msg.messageType, Is.EqualTo(TestSuiteMessage.MessageType));
            Assert.That(msg.phase, Is.EqualTo(Message.BeginPhase));
            Assert.That(msg["name"], Is.EqualTo("suiteName"));
            Assert.That(msg["scope"], Is.EqualTo("suiteScope"));
            Assert.That(msg["platform"], Is.EqualTo("platformName"));
            Assert.That(msg["minimalCommandLine"], Does.Contain("test"));
            Assert.That(msg["supportsFilters"], Is.EqualTo(true));
        }

        [Test]
        public void CanCreateTestSuiteCompletedMessage()
        {
            var msg = TestSuiteMessage.CreateTestSuiteCompletedMessage("suiteName", "suiteScope", "platformName", 123, new[] {"--foo=bar"});

            Assert.That(msg.Is(TestSuiteMessage.MessageType));
            Assert.That(msg.phase, Is.EqualTo(Message.EndPhase));
            Assert.That(msg["name"], Is.EqualTo("suiteName"));
            Assert.That(msg["platform"], Is.EqualTo("platformName"));
            Assert.That(msg["scope"], Is.EqualTo("suiteScope"));
            Assert.That(msg["duration"], Is.EqualTo(123));
            Assert.That(msg["minimalCommandLine"], Is.EqualTo(new[] {"--foo=bar"}));
        }

        [Test]
        public void CanCreateTestSuiteCompletedMessageWithError()
        {
            var exception = new Exception("exceptionText");

            var msg = TestSuiteMessage.CreateTestSuiteCompletedMessage("suiteName", "scope",  "platformName", 123, new string[] {}, exception);

            Assert.That(msg.Is(TestSuiteMessage.MessageType));
            Assert.That(msg["name"], Is.EqualTo("suiteName"));
            Assert.That(msg["duration"], Is.EqualTo(123));
            Assert.That(msg["scope"], Is.EqualTo("scope"));
            Assert.That(msg["platform"], Is.EqualTo("platformName"));
            Assert.That(msg.HasErrors());
        }
    }
}
