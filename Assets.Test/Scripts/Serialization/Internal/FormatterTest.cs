using System;
using System.Globalization;
using System.Reflection;
using Assets.Scripts.Serialization;
using Assets.Scripts.Serialization.Internal;
using Moq;
using NUnit.Framework;

namespace Assets.Test.Scripts.Serialization.Internal
{
    [TestFixture]
    internal class FormatterTest
    {
        private Mock<IBinarySerializer> _binarySerializerMock;
        private Mock<IUnityJsonSerializer> _unityJsonSerializerMock;
        private Mock<IFormatSelector> _formatSelectorMock;

        [SetUp]
        public void SetUp()
        {
            _binarySerializerMock = new Mock<IBinarySerializer>();
            _unityJsonSerializerMock = new Mock<IUnityJsonSerializer>();
            _formatSelectorMock = new Mock<IFormatSelector>();
        }

        [Test]
        public void Ctor_Default_DoesNotThrow()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new Formatter());
        }

        [Test]
        public void Ctor_BinarySerializerNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Formatter(
                null,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object));
        }

        [Test]
        public void Ctor_UnityJsonSerializerNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Formatter(
                _binarySerializerMock.Object,
                null,
                _formatSelectorMock.Object));
        }

        [Test]
        public void Ctor_FormatSelectorNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                null));
        }

        [Test]
        public void SerializeGeneric_SerializationFormatNull_NullFormatted()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns(SerializationFormat.Null);

            var result = subject.Serialize<TestData>(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TestData).AssemblyQualifiedName, result.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.Null, result.Format);
        }

        [Test]
        public void SerializeGeneric_SerializationFormatBinary_BinaryFormatted()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var serializedData = new byte[] { 1, 2, 3, 42, 2 };

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns(SerializationFormat.BinaryFormatter);
            _binarySerializerMock.Setup(mock => mock.Serialize(It.IsAny<object>()))
                .Returns(serializedData);

            var result = subject.Serialize<TestData>(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TestData).AssemblyQualifiedName, result.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.BinaryFormatter, result.Format);
            Assert.AreEqual(serializedData, result.BinaryData);
        }

        [Test]
        public void SerializeGeneric_SerializationFormatJson_JsonFormatted()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            const string serializedData = "12345";

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns(SerializationFormat.UnityJson);
            _unityJsonSerializerMock.Setup(mock => mock.Serialize(It.IsAny<object>()))
                .Returns(serializedData);

            var result = subject.Serialize<TestData>(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TestData).AssemblyQualifiedName, result.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.UnityJson, result.Format);
            Assert.AreEqual(serializedData, result.JsonData);
        }

        [Test]
        public void Serialize_SerializationFormatNull_NullFormatted()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns(SerializationFormat.Null);

            var result = subject.Serialize(typeof(TestData), null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TestData).AssemblyQualifiedName, result.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.Null, result.Format);
        }

        [Test]
        public void SerializeGerneric_NotSupportedFormat_Throws()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns((SerializationFormat) int.MaxValue);

            Assert.Throws<NotSupportedException>(() => subject.Serialize(new TestData()));
        }

        [Test]
        public void Serialize_SerializationFormatBinary_BinaryFormatted()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var serializedData = new byte[] { 1, 2, 3, 42, 2 };

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns(SerializationFormat.BinaryFormatter);
            _binarySerializerMock.Setup(mock => mock.Serialize(It.IsAny<object>()))
                .Returns(serializedData);

            var result = subject.Serialize(typeof(TestData), null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TestData).AssemblyQualifiedName, result.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.BinaryFormatter, result.Format);
            Assert.AreEqual(serializedData, result.BinaryData);
        }

        [Test]
        public void Serialize_SerializationFormatJson_JsonFormatted()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            const string serializedData = "12345";

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns(SerializationFormat.UnityJson);
            _unityJsonSerializerMock.Setup(mock => mock.Serialize(It.IsAny<object>()))
                .Returns(serializedData);

            var result = subject.Serialize(typeof(TestData), null);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(TestData).AssemblyQualifiedName, result.AssemblyQualifiedName);
            Assert.AreEqual(SerializationFormat.UnityJson, result.Format);
            Assert.AreEqual(serializedData, result.JsonData);
        }

        [Test]
        public void Serialize_TypeNull_Throws()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Serialize(null, new object()));
        }

        [Test]
        public void Serialize_TypeWithoutNull_Throws()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            var type = new TypeWithoutAssemblyQualifiedName();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => subject.Serialize(type, new object()));
        }

        [Test]
        public void Serialize_NotSupportedFormat_Throws()
        {
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            _formatSelectorMock.Setup(mock => mock.GetFormat(It.IsAny<Type>(), It.IsAny<object>()))
                .Returns((SerializationFormat)int.MaxValue);

            Assert.Throws<NotSupportedException>(() => subject.Serialize(typeof(TestData), new TestData()));
        }

        [Test]
        public void DeserializeGeneric_NullFormatted_Null()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName);
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            var result = subject.Deserialize<TestData>(serializationInfo);

            Assert.IsNull(result);
        }

        [Test]
        public void DeserializeGeneric_BinaryFormatted_Deserialized()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName, new byte[0]);
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var testData = new TestData();
            _binarySerializerMock.Setup(mock => mock.Deserialize(It.IsAny<byte[]>()))
                .Returns(testData);

            var result = subject.Deserialize<TestData>(serializationInfo);

            Assert.AreEqual(testData, result);
        }

        [Test]
        public void DeserializeGeneric_JsonFormatted_Deserialized()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName, "12345");
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var testData = new TestData();
            _unityJsonSerializerMock.Setup(mock => mock.Deserialize(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(testData);

            var result = subject.Deserialize<TestData>(serializationInfo);

            Assert.AreEqual(testData, result);
        }

        [Test]
        public void DeserializeGeneric_TypeNotResolved_Throws()
        {
            var serializationInfo = new SerializedValue("12345", "12345");
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var testData = new TestData();
            _unityJsonSerializerMock.Setup(mock => mock.Deserialize(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(testData);

            Assert.Throws<InvalidOperationException>(() => subject.Deserialize<TestData>(serializationInfo));
        }

        [Test]
        public void DeserializeGeneric_TypeNotAssignalbe_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName, "12345");
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            Assert.Throws<InvalidOperationException>(() => subject.Deserialize<int>(serializationInfo));
        }

        [Test]
        public void Deserialize_NullFormatted_Null()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName);
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);

            var result = subject.Deserialize(serializationInfo);

            Assert.IsNull(result);
        }

        [Test]
        public void Deserialize_BinaryFormatted_Deserialized()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName, new byte[0]);
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var testData = new TestData();
            _binarySerializerMock.Setup(mock => mock.Deserialize(It.IsAny<byte[]>()))
                .Returns(testData);

            var result = subject.Deserialize(serializationInfo);

            Assert.AreEqual(testData, result);
        }

        [Test]
        public void Deserialize_JsonFormatted_Deserialized()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            var serializationInfo = new SerializedValue(typeof(TestData).AssemblyQualifiedName, "12345");
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var testData = new TestData();
            _unityJsonSerializerMock.Setup(mock => mock.Deserialize(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(testData);

            var result = subject.Deserialize(serializationInfo);

            Assert.AreEqual(testData, result);
        }

        [Test]
        public void Deserialize_TypeNotResolved_Throws()
        {
            var serializationInfo = new SerializedValue("12345", "12345");
            var subject = new Formatter(
                _binarySerializerMock.Object,
                _unityJsonSerializerMock.Object,
                _formatSelectorMock.Object);
            var testData = new TestData();
            _unityJsonSerializerMock.Setup(mock => mock.Deserialize(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(testData);

            Assert.Throws<InvalidOperationException>(() => subject.Deserialize(serializationInfo));
        }

        private class TestData
        {             
        }

        private class TypeWithoutAssemblyQualifiedName : Type
        {
            public override object[] GetCustomAttributes(bool inherit)
            {
                throw new NotImplementedException();
            }

            public override bool IsDefined(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }

            public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override Type GetInterface(string name, bool ignoreCase)
            {
                throw new NotImplementedException();
            }

            public override Type[] GetInterfaces()
            {
                throw new NotImplementedException();
            }

            public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override EventInfo[] GetEvents(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override Type[] GetNestedTypes(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override Type GetNestedType(string name, BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override Type GetElementType()
            {
                throw new NotImplementedException();
            }

            protected override bool HasElementTypeImpl()
            {
                throw new NotImplementedException();
            }

            protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types,
                ParameterModifier[] modifiers)
            {
                throw new NotImplementedException();
            }

            public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
                Type[] types, ParameterModifier[] modifiers)
            {
                throw new NotImplementedException();
            }

            public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override FieldInfo GetField(string name, BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override FieldInfo[] GetFields(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
            {
                throw new NotImplementedException();
            }

            protected override TypeAttributes GetAttributeFlagsImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsArrayImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsByRefImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsPointerImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsPrimitiveImpl()
            {
                throw new NotImplementedException();
            }

            protected override bool IsCOMObjectImpl()
            {
                throw new NotImplementedException();
            }

            public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args,
                ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
            {
                throw new NotImplementedException();
            }

            public override Type UnderlyingSystemType
            {
                get { throw new NotImplementedException(); }
            }

            protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention,
                Type[] types, ParameterModifier[] modifiers)
            {
                throw new NotImplementedException();
            }

            public override string Name
            {
                get { throw new NotImplementedException(); }
            }

            public override Guid GUID
            {
                get { throw new NotImplementedException(); }
            }

            public override Module Module
            {
                get { throw new NotImplementedException(); }
            }

            public override Assembly Assembly
            {
                get { throw new NotImplementedException(); }
            }

            public override string FullName
            {
                get { throw new NotImplementedException(); }
            }

            public override string Namespace
            {
                get { throw new NotImplementedException(); }
            }

            public override string AssemblyQualifiedName
            {
                get { return null; }
            }

            public override Type BaseType
            {
                get { throw new NotImplementedException(); }
            }

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                throw new NotImplementedException();
            }
        }
    }
}
