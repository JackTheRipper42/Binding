using Assets.Scripts.Serialization;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Serialization
{
    [TestFixture]
    internal class SerializedMemberTest
    {
        [Test]
        public void Ctor_NameNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SerializedMember(
                null,
                MemberType.Field,
                new SerializedValue("df")));
        }

        [Test]
        public void Ctor_SerializedValueNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SerializedMember(
                "ge",
                MemberType.Field,
                null));
        }

        [Test]
        public void Ctor_ParameterNotNull_PropertiesInitialized()
        {
            const string name = "memberName";
            const MemberType memberType = MemberType.Property;
            var serializedValue = new SerializedValue("fef");

            var subject = new SerializedMember(name, memberType, serializedValue);

            Assert.AreEqual(name, subject.Name);
            Assert.AreEqual(memberType, subject.MemberType);
            Assert.IsTrue(ReferenceEquals(serializedValue, subject.SerializedValue));
        }
    }
}
