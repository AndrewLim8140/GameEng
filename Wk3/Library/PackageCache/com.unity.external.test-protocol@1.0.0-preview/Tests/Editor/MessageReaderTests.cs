using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests
{
    public class MessageReaderTests
    {
        StringBuilder m_Input;

        [SetUp]
        public void SetUp()
        {
            m_Input = new StringBuilder();
        }

        [Test]
        public void Read_WhenNoInput_ReturnsEmptyResult()
        {
            var sr = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(m_Input.ToString())));
            var result = MessageReader.Read(sr, m => m.Is("nonExitingType"));

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Read_WhenInputContainsASingleMessageWithRequestedType_ReturnsResultsContainingMessage()
        {
            m_Input.AppendLine("##utp:{\"type\":\"Info\",\"time\":1,\"version\":1,\"message\":\"messageText\"}");
            var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(m_Input.ToString())));

            var result = MessageReader.Read(reader, m => m.Is(InfoMessage.MessageType)).ToArray();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Is("Info"));
        }

        [Test]
        public void Read_WhenInputContainsASinglemessageThatDoesNotMatchARequestedType_ReturnsResultWithoutAMessage()
        {
            m_Input.AppendLine("##utp:{\"type\":\"Info\",\"time\":1,\"version\":1,\"message\":\"messageText\"}");
            var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(m_Input.ToString())));

            var result = MessageReader.Read(reader, m => m.Is(ErrorMessage.MessageType)).ToArray();

            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void Read_WhenInputContainsTwoElementsMatchingFilterAndOneNot_ReturnsResultWithTwoMatchedElements()
        {
            const string text1 = "##utp:{\"type\":\"Info\",\"time\":1,\"version\":1,\"message\":\"messageText\"}";
            const string text2 = "##utp:{\"type\":\"Info\",\"time\":1,\"version\":1,\"message\":\"messageText\"}";
            const string text3 = "##utp:{\"type\":\"Error\",\"time\":1,\"version\":1,\"message\":\"errorMessage\"}";
            m_Input.AppendLine(text1);
            m_Input.AppendLine(text2);
            m_Input.AppendLine(text3);
            var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(m_Input.ToString())));

            var result = MessageReader.Read(reader, m => m.Is(InfoMessage.MessageType)).ToArray();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Is("Info"));
            Assert.That(result[1].Is("Info"));
        }
    }
}
