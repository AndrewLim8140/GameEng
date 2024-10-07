using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace Unity.TestProtocol.UnitTests
{
    public class MessageTests
    {
        [Test]
        public void Ctor_ThrowsExceptionIfTypeIsNotSet()
        {
            var data = new OrderedDictionary();

            Assert.That(() => new Message(data), Throws.Exception);
            Assert.That(() => new Message(""), Throws.Exception);
        }

        [Test]
        public void Ctor_MessageHasAllFieldsSetCorrectly()
        {
            var message = new Message("typeName");

            Assert.That(message.messageType, Is.EqualTo("typeName"));
            Assert.That(message.phase, Is.EqualTo(Message.ImmediatePhase));
            Assert.That(message.GetInt("processId"), Is.EqualTo(Process.GetCurrentProcess().Id));
            Assert.That(message.GetInt("version"), Is.EqualTo(Message.kProtocolVersion));
            Assert.That(message["errors"], Is.EqualTo(new string[] {}));
        }

        [Test]
        public void Indexer_SetsPropertyValue()
        {
            var message = new Message("typeName");

            message["field1"] = 1;

            Assert.That(message["field1"], Is.EqualTo(1));
        }

        [Test]
        public void GetInt_ReturnsExpectedValue()
        {
            var message = new Message("typeName");

            message["field"] = 1;

            Assert.That(message.GetInt("field"), Is.EqualTo(1));
        }

        [Test]
        public void GetString_ReturnsExpectedValue()
        {
            var message = new Message("typeName");

            message["field"] = "stringValue";

            Assert.That(message.GetString("field"), Is.EqualTo("stringValue"));
        }

        [Test]
        public void GetLong_ReturnsExpectedValue()
        {
            var message = new Message("typeName");

            message["field"] = long.MaxValue;

            Assert.That(message.GetLong("field"), Is.EqualTo(long.MaxValue));
        }

        [Test]
        public void AddError_AddsAnErrorToErrorsField()
        {
            var message = new Message("messageType", Message.EndPhase);

            message.AddError("errorText");

            Assert.That(message["errors"], Is.EqualTo(new[] { "errorText" }));
        }

        [Test]
        public void Ctor_NoErrorsAreGivenInData_AssignsAnEmptyList()
        {
            IDictionary data = new Dictionary<string, object>();
            data["type"] = "typeName";

            var message = new Message(data);

            Assert.That(!message.GetErrors().Any());
        }

        [Test]
        public void Ctor_ErrorsHasAnUnexpectedType_ThrowsAnException()
        {
            var msg = new Message("Dummy");
            msg["errors"] = "stringError";

            Assert.That(() => new Message(msg.data), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void End_ReturnsEndPhaseMessagePair_ForBeginMessage()
        {
            var beginMessage = new Message("foo", Message.BeginPhase) { ["time"] = 0 };

            var endMessage = beginMessage.End();

            Assert.That(endMessage, Is.Not.SameAs(beginMessage));
            Assert.That(endMessage["type"], Is.EqualTo(beginMessage["type"]));
            Assert.That(endMessage["phase"].ToString(), Is.EqualTo(Message.EndPhase));
            Assert.That(endMessage["time"], Is.TypeOf<long>());
            Assert.That(endMessage["time"], Is.GreaterThan(0));
            Assert.That(endMessage["duration"], Is.TypeOf<long>());
            Assert.That(endMessage["duration"], Is.GreaterThan(0));
        }

        [Test]
        public void End_ReturnsEndPhaseMessagePairWithErrorAdded_IfSpecified()
        {
            var beginMessage = new Message("foo", Message.BeginPhase);
            var exception = new Exception("bar");

            var endMessage = beginMessage.End(exception);

            Assert.That(endMessage.GetErrors(), Is.Not.Empty);
            Assert.That(endMessage.GetErrors().First(), Is.EqualTo(exception.ToString()));
            Assert.That(endMessage["phase"].ToString(), Is.EqualTo(Message.EndPhase));
        }

        [Test]
        public void End_ThrowsException_IfCalledOnImmediateMessage()
        {
            var beginMessage = new Message("foo", Message.ImmediatePhase);

            Assert.That(() => beginMessage.End(), Throws.Exception.TypeOf<InvalidOperationException>());
        }
    }
}
