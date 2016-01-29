using Assets.Scripts.Serialization.Internal;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Serialization.Internal
{
    [TestFixture]
    internal class ContentTest
    {
        [Test]
        public void Ctor_NameNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new Content(null));
        }

        [Test]
        public void Ctor_Name_NameSet()
        {
            const string name = "458902";

            var subject = new Content(name);

            Assert.AreEqual(name, subject.Name);
        }

        [Test]
        public void Value_Set()
        {
            var subject = new Content("42");
            var value = new SerializedValue("42");

            subject.Value = value;
            var result = subject.Value;

            Assert.IsTrue(ReferenceEquals(value, result));
        }
    }
}
