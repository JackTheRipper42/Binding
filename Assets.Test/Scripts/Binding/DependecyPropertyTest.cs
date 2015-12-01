using Assets.Scripts.Binding;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class DependecyPropertyTest
    {
        [Test]
        public void Ctor_ValueType_Initialized()
        {
            const int value = 42;
            var subject = new DependecyProperty<int>(value, null, null);
            Assert.AreEqual(value, subject.GetValue());
        }

        [Test]
        public void Ctor_RefernceType_Initialized()
        {
            var value = new List();
            var subject = new DependecyProperty<List>(value, null, null);
            Assert.AreEqual(value, subject.GetValue());
        }

        [Test]
        public void ParamlessCtor_ValueType_DefaultValue()
        {
            var subject = new DependecyProperty<TimeSpan>();
            Assert.AreEqual(default(TimeSpan), subject.GetValue());
        }

        [Test]
        public void ParamlessCtor_RefernceType_Null()
        {
            var subject = new DependecyProperty<List>();
            Assert.IsNull(subject.GetValue());
        }

        [Test]
        public void SetValue_ValueType_PropertyChangedRaised()
        {
            const int oldValue = default(int);
            const int newValue = 64;
            var eventRaised = false;
            var subject = new DependecyProperty<int>();
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
            var subject = new DependecyProperty<List>();
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

        [Test]
        public void SetValue_ValueChangedAndPropertyChangedCallbackNotNull_CallbackCalled()
        {
            const int newValue = 42;
            const int oldValue = 0;
            var called = false;
            var subject = new DependecyProperty<int>(
                oldValue,
                (oldInt, newInt) =>
                {
                    Assert.IsFalse(called);
                    Assert.AreEqual(oldValue, oldInt);
                    Assert.AreEqual(newValue, newInt);
                    called = true;
                },
                null);

            subject.SetValue(newValue);
            Assert.IsTrue(called);
        }

        [Test]
        public void SetValue_ValueNotChangedAndPropertyChangedCallbackNotNull_CallbackNotCalled()
        {
            const int newValue = 42;
            var called = false;
            var subject = new DependecyProperty<int>(
                newValue,
                (oldInt, newInt) => { called = true; },
                null);

            subject.SetValue(newValue);

            Assert.IsFalse(called);
        }

        [Test]
        public void SetValue_SetDifferentvalueAndCoerceCallbackDoesNotChangeTheValue_SetValueNotChanged()
        {
            const int newValue = 42;
            const int oldValue = 0;
            var called = false;
            var propertyChangeRaised = false;
            var subject = new DependecyProperty<int>(
                oldValue,
                null,
                value =>
                {
                    Assert.IsFalse(called);
                    Assert.AreEqual(newValue, value);
                    called = true;
                    return value;
                });
            subject.PropertyChanged += (o, e) =>
            {
                Assert.IsFalse(propertyChangeRaised);
                propertyChangeRaised = true;
            };

            subject.SetValue(newValue);

            Assert.AreEqual(newValue, subject.GetValue());
            Assert.IsTrue(called);
            Assert.IsTrue(propertyChangeRaised);
        }

        [Test]
        public void SetValue_SetDifferntValueAndCoerceCallbackDoesNotChangeTheValue_SetValueChanged()
        {
            const int newValue = 42;
            const int oldValue = 0;
            const int coercevalue = 23;
            var called = false;
            var propertyChangeRaised = false;
            var subject = new DependecyProperty<int>(
                oldValue,
                null,
                value =>
                {
                    Assert.IsFalse(called);
                    Assert.AreEqual(newValue, value);
                    called = true;
                    return coercevalue;
                });
            subject.PropertyChanged += (o, e) =>
            {
                Assert.IsFalse(propertyChangeRaised);
                propertyChangeRaised = true;
            };

            subject.SetValue(newValue);

            Assert.AreEqual(coercevalue, subject.GetValue());
            Assert.IsTrue(called);
            Assert.IsTrue(propertyChangeRaised);
        }

        [Test]
        public void SetValue_SetSameValue_CoerceCallbackNotCalled()
        {
            const int oldValue = 0;
            const int coercevalue = 23;
            var called = false;
            var subject = new DependecyProperty<int>(
                oldValue,
                null,
                value =>
                {
                    called = true;
                    return coercevalue;
                });

            subject.SetValue(oldValue);

            Assert.AreEqual(oldValue, subject.GetValue());
            Assert.IsFalse(called);
        }

        [Test]
        public void SetValue_SetDifferentValueCoerceCallbackSetSameValue_ValueNotChanged()
        {
            const int newValue = 42;
            const int oldValue = 0;
            var propertyChangeRaised = false;
            var called = false;
            var subject = new DependecyProperty<int>(
                oldValue,
                null,
                value =>
                {
                    Assert.IsFalse(called);
                    Assert.AreEqual(newValue, value);
                    called = true;
                    return oldValue;
                });
            subject.PropertyChanged += (o, e) =>
            {
                propertyChangeRaised = true;
            };

            subject.SetValue(newValue);

            Assert.AreEqual(oldValue, subject.GetValue());
            Assert.IsTrue(called);
            Assert.IsFalse(propertyChangeRaised);
        }
    }
}
