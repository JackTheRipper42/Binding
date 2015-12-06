using Assets.Scripts.Binding;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class DependencyPropertyTest
    {
        private Mock<IBindingFactory> _bindingFactoryMock;

        [SetUp]
        public void SetUp()
        {
            _bindingFactoryMock = new Mock<IBindingFactory>();
        }

        [Test]
        public void Ctor_ValueType_Initialized()
        {
            const int value = 42;
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object, value, null, null);
            Assert.AreEqual(value, subject.GetValue());
        }

        [Test]
        public void Ctor_ReferenceType_Initialized()
        {
            var value = new List();
            var subject = new DependencyProperty<List>(_bindingFactoryMock.Object, value, null, null);
            Assert.AreEqual(value, subject.GetValue());
        }

        [Test]
        public void Ctor_BindingFactoryNull_Throws()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new DependencyProperty<int>(null, 42, null, null));
        }

        [Test]
        public void ParamlessCtor_ValueType_DefaultValue()
        {
            var subject = new DependencyProperty<TimeSpan>(_bindingFactoryMock.Object);
            Assert.AreEqual(default(TimeSpan), subject.GetValue());
        }

        [Test]
        public void ParamlessCtor_ReferenceType_Null()
        {
            var subject = new DependencyProperty<List>(_bindingFactoryMock.Object);
            Assert.IsNull(subject.GetValue());
        }

        [Test]
        public void SetValue_ValueType_PropertyChangedRaised()
        {
            const int oldValue = default(int);
            const int newValue = 64;
            var eventRaised = false;
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
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
            var subject = new DependencyProperty<List>(_bindingFactoryMock.Object);
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
            var subject = new DependencyProperty<int>(
                _bindingFactoryMock.Object,
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
            var subject = new DependencyProperty<int>(
                _bindingFactoryMock.Object,
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
            var subject = new DependencyProperty<int>(
                _bindingFactoryMock.Object,
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
        public void SetValue_SetDifferentValueAndCoerceCallbackDoesNotChangeTheValue_SetValueChanged()
        {
            const int newValue = 42;
            const int oldValue = 0;
            const int coercevalue = 23;
            var called = false;
            var propertyChangeRaised = false;
            var subject = new DependencyProperty<int>(
                _bindingFactoryMock.Object,
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
            var subject = new DependencyProperty<int>(
                _bindingFactoryMock.Object,
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
            var subject = new DependencyProperty<int>(
                _bindingFactoryMock.Object,
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

        [Test]
        public void Dispose_NotBound_DoesNotThrow()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            Assert.DoesNotThrow(subject.Dispose);
        }

        [Test]
        public void Dispose_MultipleCalls_DoesNotThrow()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Dispose();
            Assert.DoesNotThrow(subject.Dispose);
        }

        [Test]
        public void Dispose_Bind_BindingDisposed()
        {
            var bindingMock = new Mock<IBinding>();
            bindingMock.Setup(mock => mock.Close());
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(BindingType.TwoWay, Mock.Of<INotifyingObject<int>>());
            subject.Dispose();
            bindingMock.Verify(mock => mock.Close());
        }

        [Test]
        public void ClearBinding_NotBound_DoesNotThrow()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            Assert.DoesNotThrow(subject.ClearBinding);
        }

        [Test]
        public void ClearBinding_MultipleCalls_DoesNotThrow()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.ClearBinding();
            Assert.DoesNotThrow(subject.Dispose);
        }

        [Test]
        public void ClearBinding_Bind_BindingDisposed()
        {
            var bindingMock = new Mock<IBinding>();
            bindingMock.Setup(mock => mock.Close());
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(BindingType.TwoWay, Mock.Of<INotifyingObject<int>>());
            subject.ClearBinding();
            bindingMock.Verify(mock => mock.Close());
        }

        [Test]
        public void Bind_WithoutConverterSourceNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(BindingType.OneWay, null));
        }

        [Test]
        public void Bind_WithoutConverter_CultureSet()
        {
            var culture = CultureInfo.InvariantCulture;
            var bindingMock = new Mock<IBinding>();
            bindingMock.SetupSet(mock => mock.Culture = culture);
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object) { Culture = culture };

            subject.Bind(BindingType.TwoWay, Mock.Of<INotifyingObject<int>>());
            bindingMock.VerifySet(mock => mock.Culture = culture);
        }

        [Test]
        public void Bind_WithoutConverterMultipleTimes_Throws()
        {
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(Mock.Of<IBinding>());
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(BindingType.OneWay, Mock.Of<INotifyingObject<int>>());
            Assert.Throws<InvalidOperationException>(
                () => subject.Bind(BindingType.OneWay, Mock.Of<INotifyingObject<int>>()));
        }

        [Test]
        public void Bind_OneWayValueConverterNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(
                Mock.Of<INotifyingObject<int>>(),
                (OneWayValueConverter<int, int>)null));
        }

        [Test]
        public void Bind_OneWayValueConverterSourceNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(
                null,
                Mock.Of<OneWayValueConverter<int, int>>()));
        }

        [Test]
        public void Bind_OneWayValueConverter_CultureSet()
        {
            var culture = CultureInfo.InvariantCulture;
            var bindingMock = new Mock<IBinding>();
            bindingMock.SetupSet(mock => mock.Culture = culture);
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object) { Culture = culture };

            subject.Bind(Mock.Of<INotifyingObject<int>>(), Mock.Of<OneWayValueConverter<int, int>>());

            bindingMock.VerifySet(mock => mock.Culture = culture);
        }

        [Test]
        public void Bind_OneWayValueConverterMultipleTimes_Throws()
        {
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(Mock.Of<IBinding>());
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<OneWayValueConverter<int, int>>());
            Assert.Throws<InvalidOperationException>(() => subject.Bind(
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<OneWayValueConverter<int, int>>()));
        }

        [Test]
        public void Bind_TwoWayValueConverterSourceNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(
                BindingType.OneWay, 
                null,
                Mock.Of<TwoWayValueConverter<int, int>>()));
        }

        [Test]
        public void Bind_TwoWayValueConverter_CultureSet()
        {
            var culture = CultureInfo.InvariantCulture;
            var bindingMock = new Mock<IBinding>();
            bindingMock.SetupSet(mock => mock.Culture = culture);
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object) { Culture = culture };

            subject.Bind(BindingType.TwoWay, Mock.Of<INotifyingObject<int>>(), Mock.Of<TwoWayValueConverter<int, int>>());

            bindingMock.VerifySet(mock => mock.Culture = culture);
        }

        [Test]
        public void Bind_TwoWayvalueConverterNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(
                BindingType.OneWay,
                Mock.Of<INotifyingObject<int>>(),
                null));
        }

        [Test]
        public void Bind_TwoWayValueConverterMultipleTimes_Throws()
        {
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(Mock.Of<IBinding>());
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(
                BindingType.OneWay, 
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<TwoWayValueConverter<int, int>>());
            Assert.Throws<InvalidOperationException>(() => subject.Bind(
                BindingType.OneWay, 
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<TwoWayValueConverter<int, int>>()));
        }

        [Test]
        public void Bind_OneWayToSourceValueConverterSourceNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(
                null,
                Mock.Of<OneWayToSourceValueConverter<int, int>>()));
        }

        [Test]
        public void Bind_OneWayToSourceValueConverter_CultureSet()
        {
            var culture = CultureInfo.InvariantCulture;
            var bindingMock = new Mock<IBinding>();
            bindingMock.SetupSet(mock => mock.Culture = culture);
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object) {Culture = culture};

            subject.Bind(Mock.Of<INotifyingObject<int>>(), Mock.Of<OneWayToSourceValueConverter<int, int>>());

            bindingMock.VerifySet(mock => mock.Culture = culture);
        }

        [Test]
        public void Bind_OneWayToSourceValueConverterNull_Throws()
        {
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.Bind(
                Mock.Of<INotifyingObject<int>>(),
                (OneWayToSourceValueConverter<int, int>) null));
        }

        [Test]
        public void Bind_OneWayToSourceValueConverterMultipleTimes_Throws()
        {
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(Mock.Of<IBinding>());
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<OneWayToSourceValueConverter<int, int>>());
            Assert.Throws<InvalidOperationException>(() => subject.Bind(
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<OneWayToSourceValueConverter<int, int>>()));
        }

        [Test]
        public void Culture_NotBound_CultureSet()
        {
            var culture = CultureInfo.InvariantCulture;
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);

            Assert.DoesNotThrow(() => subject.Culture = culture);
            Assert.AreEqual(culture, subject.Culture);
        }

        [Test]
        public void Culture_Bound_BindingCultureSet()
        {
            var culture = CultureInfo.InvariantCulture;
            var bindingMock = new Mock<IBinding>();
            bindingMock.SetupSet(mock => mock.Culture = culture);
            _bindingFactoryMock.Setup(mock => mock.CreatePropertyBinding(
                It.IsAny<BindingType>(),
                It.IsAny<IDependencyProperty<int>>(),
                It.IsAny<INotifyingObject<int>>(),
                It.IsAny<ValueConverter<int, int>>()))
                .Returns(bindingMock.Object);
            var subject = new DependencyProperty<int>(_bindingFactoryMock.Object);
            subject.Bind(BindingType.OneWay, Mock.Of<INotifyingObject<int>>());
            subject.Culture = culture;

            bindingMock.VerifySet(mock => mock.Culture = culture);
            Assert.AreEqual(culture, subject.Culture);
        }
    }
}
