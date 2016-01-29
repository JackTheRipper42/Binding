using Assets.Scripts.Serialization;
using Assets.Scripts.Serialization.Internal;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Assets.Test.Scripts.Serialization
{
    [TestFixture]
    internal class SerializationInfoTest
    {
        private Mock<IFormatter> _formatterMock;

        [SetUp]
        public void SetUp()
        {
            _formatterMock = new Mock<IFormatter>();
        }

        [Test]
        public void Ctor_DefaultCtor_DoesNotThrow()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new SerializationInfo());
        }

        [Test]
        public void Ctor_Null_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SerializationInfo(null));
        }

        [Test]
        public void SetValueGeneric_NameNull_Throws()
        {
            var subject = new SerializationInfo(_formatterMock.Object);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.SetValue(null, new object()));
        }

        [Test]
        public void SetValueGeneric_NameNotNull_FormatterSerializeCalled()
        {
            _formatterMock.Setup(mock => mock.Serialize(It.IsAny<TestData>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);

            subject.SetValue("43", new TestData());

            _formatterMock.Verify(mock => mock.Serialize(It.IsAny<TestData>()));
        }

        [Test]
        public void SetValue_NameNull_Throws()
        {
            var subject = new SerializationInfo(_formatterMock.Object);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.SetValue(null, typeof(object), new object()));
        }

        [Test]
        public void SetValue_TypeNull_Throws()
        {
            var subject = new SerializationInfo(_formatterMock.Object);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.SetValue("234", null, new object()));
        }

        [Test]
        public void SetValue_NameAndTypeNotNull_FormatterSerializeCalled()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);

            subject.SetValue("43", typeof(TestData), new TestData());

            _formatterMock.Verify(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()));
        }

        [Test]
        public void GetValue_NameDoesNotExist_Throws()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);
            subject.SetValue("1", new TestData());
            subject.SetValue("2", new TestData());
            subject.SetValue("3", new TestData());
            subject.SetValue("4", new TestData());

            Assert.Throws<KeyNotFoundException>(() => subject.GetValue<TestData>("not existing"));
        }

        [Test]
        public void GetValue_ValueSet_Value()
        {
            const string name = "345";

            var value = new TestData();
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            _formatterMock.Setup(mock => mock.Deserialize<TestData>(It.IsAny<SerializedValue>()))
                .Returns(value);
            var subject = new SerializationInfo(_formatterMock.Object);
            subject.SetValue(name, value);

            var result = subject.GetValue<TestData>(name);

            Assert.IsTrue(ReferenceEquals(value, result));
        }

        [Test]
        public void TryGetValue_NameDoesNotExist_False()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);
            subject.SetValue("1", new TestData());
            subject.SetValue("2", new TestData());
            subject.SetValue("3", new TestData());
            subject.SetValue("4", new TestData());
            object value;

            var result = subject.TryGetValue("jje", out value);

            Assert.IsFalse(result);
        }

        [Test]
        public void TryGetValue_ValueSet_True()
        {
            const string name = "345";

            var expectedValue = new TestData();
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            _formatterMock.Setup(mock => mock.Deserialize(It.IsAny<SerializedValue>()))
                .Returns(expectedValue);
            var subject = new SerializationInfo(_formatterMock.Object);
            subject.SetValue(name, expectedValue);
            object value;

            var result = subject.TryGetValue(name, out value);

            Assert.IsTrue(result);
            Assert.IsTrue(ReferenceEquals(expectedValue, value));
        }

        [Test]
        public void SetValueGeneric_NameAlreadyExists_DoesNotThrow()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);
            const string name = "42523";

            Assert.DoesNotThrow(() => subject.SetValue(name, new TestData()));
            Assert.DoesNotThrow(() => subject.SetValue(name, new TestData()));
            Assert.DoesNotThrow(() => subject.SetValue(name, new TestData()));
            Assert.DoesNotThrow(() => subject.SetValue(name, new TestData()));
        }

        [Test]
        public void SetValue_NameAlreadyExists_DoesNotThrow()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);
            const string name = "42523";

            Assert.DoesNotThrow(() => subject.SetValue(name, typeof(TestData), new TestData()));
            Assert.DoesNotThrow(() => subject.SetValue(name, typeof(TestData), new TestData()));
            Assert.DoesNotThrow(() => subject.SetValue(name, typeof(TestData), new TestData()));
            Assert.DoesNotThrow(() => subject.SetValue(name, typeof(TestData), new TestData()));
        }

        [Test]
        public void SetValueGeneric_MutlipleCallWithDifferentNames_DoesNotThrow()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);

            for (int i = 0; i < 120; i++)
            {
                Assert.DoesNotThrow(() => subject.SetValue(i.ToString(), new TestData()));
            }
        }

        [Test]
        public void SetValue_MutlipleCallWithDifferentNames_DoesNotThrow()
        {
            _formatterMock.Setup(mock => mock.Serialize(typeof(TestData), It.IsAny<object>()))
                .Returns(new SerializedValue("2442"));
            var subject = new SerializationInfo(_formatterMock.Object);

            for (var i = 0; i < 120; i++)
            {
                Assert.DoesNotThrow(() => subject.SetValue(i.ToString(), typeof(TestData), new TestData()));
            }
        }
        
        private class TestData
        {             
        }
    }
}
