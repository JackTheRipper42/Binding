using Assets.Scripts.Serialization.Internal;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Serialization.Internal
{
    [TestFixture]
    internal class BinarySerializerTest
    {
        [Test]
        public void Serialize_NotNull_ByteArray()
        {
            var data = new TestData
            {
                Value = 42
            };
            var subject = new BinarySerializer();

            var result = subject.Serialize(data);

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void SerializeDeserialize_NotNull_EqualResult()
        {
            const double value = 42;
            var data = new TestData
            {
                Value = value
            };
            var subject = new BinarySerializer();

            var serialized = subject.Serialize(data);
            var result = subject.Deserialize(serialized);

            Assert.IsInstanceOf<TestData>(result);
            Assert.AreEqual(value, ((TestData) result).Value);
        }

        [Test]
        public void Deserialize_Null_Throws()
        {
            var subject = new BinarySerializer();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Deserialize(null));
        }

        [Serializable]
        private class TestData
        {
            public double Value { get; set; }
        }
    }
}
