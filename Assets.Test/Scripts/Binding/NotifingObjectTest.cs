using Assets.Scripts.Binding;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class NotifingObjectTest
    {
        [Test]
        public void Ctor_ValueType_Initialized()
        {
            const int value = 42;
            var subject = new NotifyingObject<int>(value);
            Assert.AreEqual(value, subject.GetValue());
        }

        [Test]
        public void Ctor_RefernceType_Initialized()
        {
            var value = new List();
            var subject = new NotifyingObject<List>(value);
            Assert.AreEqual(value, subject.GetValue());
        }

        [Test]
        public void ParamlessCtor_ValueType_DefaultValue()
        {
            var subject = new NotifyingObject<TimeSpan>();
            Assert.AreEqual(default(TimeSpan), subject.GetValue());
        }

        [Test]
        public void ParamlessCtor_RefernceType_Null()
        {
            var subject = new NotifyingObject<List>();
            Assert.IsNull(subject.GetValue());
        }

        [Test]
        public void SetValue_ValueType_PropertyChangedRaised()
        {
            const int oldValue = default(int);
            const int newValue = 64;
            var eventRaised = false;
            var subject = new NotifyingObject<int>();
            subject.PropertyChanged += (sender, args) =>
            {
                Assert.IsFalse(eventRaised);
                Assert.AreEqual(oldValue, args.OldValue);
                Assert.AreEqual(newValue, args.NewValue);
                eventRaised = true;
            };

            subject.SetValue(newValue);

            Assert.IsTrue(eventRaised);
        }

        [Test]
        public void SetValue_ReferenceType_PropertyChangedRaised()
        {
            var newValue = new List();
            var eventRaised = false;
            var subject = new NotifyingObject<List>();
            subject.PropertyChanged += (sender, args) =>
            {
                Assert.IsFalse(eventRaised);
                Assert.IsNull(args.OldValue);
                Assert.AreEqual(newValue, args.NewValue);
                eventRaised = true;
            };

            subject.SetValue(newValue);

            Assert.IsTrue(eventRaised);
        }
    }
}
