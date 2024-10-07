using NUnit.Framework;
using Unity.TestProtocol.Messages;

namespace Unity.TestProtocol.UnitTests
{
    [TestFixture]
    public class MergeMessageTests
    {
        [Test]
        public void Merge_BeginEndMessages_ReturnsExpectedResult()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            var endMessage = new Message("messageType", Message.EndPhase);

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.messageType, Is.EqualTo("messageType"));
            Assert.That(result.phase, Is.EqualTo(Message.CompletePhase));
        }

        [Test]
        public void Merge_BeginMessageHasCutsomFields_ReturnsExpectedResult()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            beginMessage["foo"] = "bar";
            var endMessage = new Message("messageType", Message.EndPhase);

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.messageType, Is.EqualTo("messageType"));
            Assert.That(result.phase, Is.EqualTo(Message.CompletePhase));
            Assert.That(result.GetString("foo"), Is.EqualTo("bar"));
        }

        [Test]
        public void Merge_EndMessageHasCutsomFields_ReturnsExpectedResult()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            var endMessage = new Message("messageType", Message.EndPhase);
            endMessage["foo"] = "bar";

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.GetString("foo"), Is.EqualTo("bar"));
        }

        [Test]
        public void Merge_BothMesssageHaveCustomFields_ReturnsExpectedResult()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            beginMessage["field1"] = "field1Value";
            beginMessage["field2"] = "field2Value";

            var endMessage = new Message("messageType", Message.EndPhase);
            endMessage["field1"] = "field1OverridenValue";
            endMessage["field3"] = "field3Value";

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.GetString("field1"), Is.EqualTo("field1OverridenValue"));
            Assert.That(result.GetString("field2"), Is.EqualTo("field2Value"));
            Assert.That(result.GetString("field3"), Is.EqualTo("field3Value"));
        }

        [Test]
        public void Merge_BeginEndMessagesDurationIsNotProvided_CalulatesCorrectDuration()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            beginMessage["time"] = 150;
            var endMessage = new Message("messageType", Message.EndPhase);
            endMessage["time"] = 250;

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.GetLong("duration"), Is.EqualTo(100));
        }

        [Test]
        public void Merge_BeginDurationIsGivenInEndMessage_UsesGivenDuration()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            var endMessage = new Message("messageType", Message.EndPhase);
            endMessage["duration"] = 123;

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.GetLong("duration"), Is.EqualTo(123));
        }

        [Test]
        public void Merge_ThrowsException_IfMessageTypesDoNotMatch()
        {
            var beginMessage = new Message("messageType1", Message.BeginPhase);
            var endMessage = new Message("messageType2", Message.EndPhase);

            Assert.That(() => beginMessage.MergeWith(endMessage), Throws.Exception.TypeOf<UtpMessageMergeException>());
        }

        [Test]
        public void Merge_TerminateMessages_EnclosesMessageCorrectly()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            var terminateMessage = TerminateCurrentMessage.Create();
            terminateMessage.AddError("Timeout");

            var result = beginMessage.MergeWith(terminateMessage);

            Assert.That(result.messageType, Is.EqualTo("messageType"));
            Assert.That(result.phase, Is.EqualTo(Message.CompletePhase));
            Assert.That(result.GetErrors(), Is.EqualTo(new[] { "Timeout" }));
        }

        [Test]
        public void Merge_MessagesHaveTheSameTypeButDifferentNames_ThrowsAnException()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            beginMessage["name"] = "foo";
            var endMessage = new Message("messageType", Message.EndPhase);
            endMessage["name"] = "bar";

            Assert.That(() => beginMessage.MergeWith(endMessage), Throws.Exception.TypeOf<UtpMessageMergeException>());
        }

        [Test]
        public void Merge_BeginMessageHasErrors_AddsErrorsToTheResultMessage()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            beginMessage.AddError("errorText");
            var endMessage = new Message("messageType", Message.EndPhase);

            var result = beginMessage.MergeWith(endMessage);

            Assert.That(result.GetErrors(), Is.EqualTo(new[] { "errorText" }));
        }

        [Test]
        public void IsEndMessageFor_CompatibleMessages_ReturnsTrue()
        {
            var beginMessage = new Message("messageType", Message.BeginPhase);
            var endMessage = new Message("messageType", Message.EndPhase);
            Assert.That(beginMessage.IsEndMessageFor(endMessage));
        }

        [Test]
        public void IsEndMessageFor_IncompatibleTypes_ReturnsFalse()
        {
            var beginMessage = new Message("t1", Message.BeginPhase);
            var endMessage = new Message("t2", Message.EndPhase);

            Assert.IsFalse(beginMessage.IsEndMessageFor(endMessage));
        }

        [Test]
        public void IsEndMessageFor_IncompatiblePhases_ReturnsFalse()
        {
            var beginMessage = new Message("t1", Message.BeginPhase);
            var endMessage = new Message("t1", Message.BeginPhase);

            Assert.IsFalse(beginMessage.IsEndMessageFor(endMessage));
        }
    }
}
