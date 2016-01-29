using Assets.Scripts.Serialization;
using Assets.Scripts.Serialization.Internal;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Test.Scripts.Serialization.Internal
{
    [TestFixture]
    internal class FormatSelectorTest
    {
        [TestCaseSource(typeof(GetFormatTestCases))]
        public SerializationFormat GetFormat(Type type, object data)
        {
            var subject = new FormatSelector();
            return subject.GetFormat(type, data);
        }

        [Test]
        public void GetFormat_TypeNull_Throws()
        {
            var subject = new FormatSelector();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.GetFormat(null, null));
        }

        private class GetFormatTestCases : IEnumerable<TestCaseData>
        {
            public IEnumerator<TestCaseData> GetEnumerator()
            {
                yield return new TestCaseData(typeof(BinarySerializable), null)
                    .Returns(SerializationFormat.Null)
                    .SetName("GetFormat_BinarySerializableNull_Null");
                yield return new TestCaseData(typeof(BinarySerializable), new BinarySerializable())
                    .Returns(SerializationFormat.BinaryFormatter)
                    .SetName("GetFormat_BinarySerializableNotNull_BinaryFormatter");
                yield return new TestCaseData(typeof(WithoutNamespace), null)
                    .Returns(SerializationFormat.Null)
                    .SetName("GetFormat_WithoutNamespaceNull_Null");
                yield return new TestCaseData(typeof(WithoutNamespace), new WithoutNamespace())
                    .Returns(SerializationFormat.BinaryFormatter)
                    .SetName("GetFormat_WithoutNamespaceNotNull_BinaryFormatter");
                yield return new TestCaseData(typeof(JsonSerializable), null)
                    .Returns(SerializationFormat.Null)
                    .SetName("GetFormat_JsonSerializableNull_Null");
                yield return new TestCaseData(typeof(JsonSerializable), new JsonSerializable())
                    .Returns(SerializationFormat.UnityJson)
                    .SetName("GetFormat_JsonSerializableNotNull_UnityJson");
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }

    [Serializable]
    internal class BinarySerializable
    {

    }
}

namespace UnityEngine
{
    [Serializable]
    internal class JsonSerializable
    {

    }
}

internal class WithoutNamespace
{

}
