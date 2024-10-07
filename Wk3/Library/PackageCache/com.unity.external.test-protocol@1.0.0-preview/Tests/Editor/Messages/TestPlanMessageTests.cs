using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class TestPlanMessageTests
    {
        [Test]
        public void TestPlanMessageInitializedCorrectly()
        {
            var tests = new[] { "test1", "test2" };

            var msg = TestPlanMessage.Create(tests);

            Assert.That(msg.Is(TestPlanMessage.MessageType));
            Assert.That(msg["tests"], Is.EqualTo(tests));
        }
    }
}
