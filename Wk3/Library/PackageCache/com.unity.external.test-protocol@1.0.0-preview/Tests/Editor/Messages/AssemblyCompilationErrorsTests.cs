using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests.Messages
{
    [TestFixture]
    public class AssemblyCompilationErrorsTests
    {
        [Test]
        public void Create_InitializesMessageCorrectly()
        {
            var errors = new[] { "errorText1", "errorText2" };

            var msg = AssemblyCompilationErrors.Create("assemblyName", errors);

            Assert.That(msg.Is(AssemblyCompilationErrors.MessageType));
            Assert.That(msg.GetString("assembly"), Is.EqualTo("assemblyName"));
            Assert.That(msg["errors"], Is.EqualTo(errors));
        }
    }
}
