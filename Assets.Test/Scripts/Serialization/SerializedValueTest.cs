using System;
using Assets.Scripts.Serialization;
using NUnit.Framework;

namespace Assets.Test.Scripts.Serialization
{
    [TestFixture]
    internal class SerializedValueTest
    {
        [Test]
        public void Ctor_String_Json()
        {
            const string assemblyQualifiedName = "some assembly qualified name";
            const string data = "some string data";

            var subject = new SerializedValue(assemblyQualifiedName, data);

            Assert.AreEqual(assemblyQualifiedName, subject.AssemblyQualifiedName);
            Assert.AreEqual(data, subject.JsonData);
            Assert.AreEqual(SerializationFormat.UnityJson, subject.Format);
        }

        [Test]
        public void Ctor_ByteArray_Binary()
        {
            const string assemblyQualifiedName = "some assembly qualified name";
            var data = new byte[] {2, 3, 4, 54};

            var subject = new SerializedValue(assemblyQualifiedName, data);

            Assert.AreEqual(assemblyQualifiedName, subject.AssemblyQualifiedName);
            Assert.AreEqual(data, subject.BinaryData);
            Assert.AreEqual(SerializationFormat.BinaryFormatter, subject.Format);
        }

        [Test]
        public void Ctor_OnlyAssemblyQualifiedName_Null()
        {
            const string assemblyQualifiedName = "some assembly qualified name";

            var subject = new SerializedValue(assemblyQualifiedName);

            Assert.AreEqual(assemblyQualifiedName, subject.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.Null, subject.Format);
        }

        [Test]
        public void Ctor_AssemblyQualifiedNameNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SerializedValue(null));
        }

        [Test]
        public void Ctor_BinaryAssemblyQualifiedNameNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SerializedValue(null, new byte[0]));
        }

        [Test]
        public void Ctor_JsonAssemblyQualifiedNameNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SerializedValue(null, string.Empty));
        }

        [Test]
        public void Ctor_BinaryDataNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SerializedValue("assembly qualified name", (byte[])null));
        }

        [Test]
        public void Ctor_JsonDataNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SerializedValue("assembly qualified name", (string) null));
        }
    }
}
