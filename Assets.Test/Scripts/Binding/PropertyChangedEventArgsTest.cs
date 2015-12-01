using Assets.Scripts.Binding;
using NUnit.Framework;
using System.Collections.Generic;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    internal class PropertyChangedEventArgsTest
    {
        [Test]
        public void Ctor_ValueType_Initialized()
        {
            const int oldValue = 42;
            const int newValue = 64;
            var subject = new PropertyChangedEventArgs<int>(oldValue, newValue);
            Assert.AreEqual(oldValue, subject.OldValue);
            Assert.AreEqual(newValue, subject.NewValue);
        }

        [Test]
        public void Ctor_ReferenceType_Initialized()
        {
            var oldValue = new List<double> {42.4};
            var newValue = new List<double>();
            var subject = new PropertyChangedEventArgs<List<double>>(oldValue, newValue);
            Assert.AreEqual(oldValue, subject.OldValue);
            Assert.AreEqual(newValue, subject.NewValue);
        }

        [Test]
        public void Ctor_Null_Initialized()
        {
            var oldValue = new List();
            var subject = new PropertyChangedEventArgs<List>(oldValue, null);
            Assert.AreEqual(oldValue, subject.OldValue);
            Assert.IsNull(subject.NewValue);
        }
    }
}
