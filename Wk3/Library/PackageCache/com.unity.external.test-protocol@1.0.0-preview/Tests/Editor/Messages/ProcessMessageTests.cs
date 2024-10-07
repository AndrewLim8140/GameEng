using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class ProcessMessageTests
    {
        const string k_ProcessPath = "path_to_executable";
        const string k_ProcessArguments = "--arg1 --args2";

        [Test]
        public void CreateProcessBeginMessage()
        {
            var processMessage = ProcessInfoMessage.CreateProcessBeginMessage(1, k_ProcessPath, k_ProcessArguments, new[] {"log1", "log2"});

            Assert.That(processMessage.IsBeginMessage());
            Assert.That(processMessage["id"], Is.EqualTo(1));
            Assert.That(processMessage.messageType, Is.EqualTo("ProcessInfo"));
            Assert.That(processMessage["path"], Is.EqualTo(k_ProcessPath));
            Assert.That(processMessage["arguments"], Is.EqualTo(k_ProcessArguments));
            Assert.That(processMessage["logs"], Is.EqualTo(new[] {"log1", "log2"}));
        }

        [Test]
        public void CreateProcessEndMessage()
        {
            var processMessage = ProcessInfoMessage.CreateProcessEndMessage(1, k_ProcessPath, k_ProcessArguments, new[] {"log1", "log2"});

            Assert.That(processMessage.IsEndMessage());
            Assert.That(processMessage["id"], Is.EqualTo(1));
            Assert.That(processMessage.messageType, Is.EqualTo("ProcessInfo"));
            Assert.That(processMessage["path"], Is.EqualTo(k_ProcessPath));
            Assert.That(processMessage["arguments"], Is.EqualTo(k_ProcessArguments));
            Assert.That(processMessage["logs"], Is.EqualTo(new[] {"log1", "log2"}));
        }
    }
}
