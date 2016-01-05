using Assets.Scripts.Binding;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class PropertyBindingTest
    {

        private Mock<INotifyingObject<int>> _notifyingObjectMock;
        private Mock<IDependencyProperty<float>> _dependencyPropertyMock;
        private Mock<ValueConverter<int, float>> _converterMock;

        [SetUp]
        public void SetUp()
        {
            _notifyingObjectMock = new Mock<INotifyingObject<int>>();
            _dependencyPropertyMock = new Mock<IDependencyProperty<float>>();
            _converterMock = new Mock<ValueConverter<int, float>>();
        }

        [Test]
        public void Ctor_BindingTypeNotSupported_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<NotSupportedException>(() => new PropertyBinding<int, float>(
                (BindingType)int.MaxValue,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object));
        }

        [Test]
        public void Ctor_DependencyPropertyNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new PropertyBinding<int, float>(
                BindingType.TwoWay,
                null,
                _notifyingObjectMock.Object,
                _converterMock.Object));
        }

        [Test]
        public void Ctor_NotifyingObjectNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                null,
                _converterMock.Object));
        }

        [Test]
        public void Ctor_ConverterNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                null));
        }

        [Test]
        public void Ctor_OneWayBindingAndConverterCanConvert_TargetUpdatedOnSourceChange()
        {
            const float converted = 42.3f;

            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.OneWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _dependencyPropertyMock.Verify(mock => mock.SetValue(converted), Times.Exactly(1));

                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 13));
            }

            _dependencyPropertyMock.Verify(mock => mock.SetValue(converted), Times.Exactly(2));
        }

        [Test]
        public void Ctor_OneWayBindingAndConverterCanNotConvert_TargetNotUpdatedOnSourceChange()
        {
            const float converted = 42.3f;

            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.OneWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 13));
            }

            _dependencyPropertyMock.Verify(mock => mock.SetValue(converted), Times.Never());
        }

        [Test]
        public void Ctor_OneWayBindingSourceChanged_CanConvertCalled()
        {
            const int source = 52;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvert(source, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.OneWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, source));
            }

            _converterMock.Verify(mock => mock.CanConvert(source, culture), Times.Once());
        }

        [Test]
        public void Ctor_OneWayBindingAndCanConvert_ConvertCalled()
        {
            const int source = 52;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.Convert(source, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.OneWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, source));
            }

            _converterMock.Verify(mock => mock.Convert(source, culture), Times.Once());
        }

        [Test]
        public void Ctor_OneWayBindingAndCanNotConvert_ConvertNotCalled()
        {
            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>()));

            using (new PropertyBinding<int, float>(
                BindingType.OneWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 34));
            }

            _converterMock.Verify(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>()), Times.Never());
        }

        [Test]
        public void Ctor_TwoWayBindingAndConverterCanConvert_TargetUpdatedOnSourceChange()
        {
            const float converted = 42.3f;

            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _dependencyPropertyMock.Verify(mock => mock.SetValue(converted), Times.Exactly(1));

                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 13));
            }

            _dependencyPropertyMock.Verify(mock => mock.SetValue(converted), Times.Exactly(2));
        }

        [Test]
        public void Ctor_TwoWayBindingAndConverterCanNotConvert_TargetNotUpdatedOnSourceChange()
        {
            const float converted = 42.3f;

            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 13));
            }

            _dependencyPropertyMock.Verify(mock => mock.SetValue(converted), Times.Never());
        }

        [Test]
        public void Ctor_TwoWayBindingSourceChanged_CanConvertCalled()
        {
            const int source = 52;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvert(source, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, source));
            }

            _converterMock.Verify(mock => mock.CanConvert(source, culture), Times.Once());
        }

        [Test]
        public void Ctor_TwoWayBindingAndCanConvert_ConvertCalled()
        {
            const int source = 52;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.Convert(source, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, source));
            }

            _converterMock.Verify(mock => mock.Convert(source, culture), Times.Once());
        }

        [Test]
        public void Ctor_TwoWayBindingAndCanNotConvert_ConvertNotCalled()
        {
            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>()));

            using (new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 34));
            }

            _converterMock.Verify(mock => mock.Convert(It.IsAny<int>(), It.IsAny<CultureInfo>()), Times.Never());
        }

        [Test]
        public void Ctor_TwoWayBindingAndConverterCanConvertBack_SourceUpdatedOnTargetChange()
        {
            const int converted = 42;

            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _notifyingObjectMock.Verify(mock => mock.SetValue(converted), Times.Exactly(1));

                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, 34f));
            }

            _notifyingObjectMock.Verify(mock => mock.SetValue(converted), Times.Exactly(2));
        }

        [Test]
        public void Ctor_TwoWayBindingAndConverterCanNotConvertBack_SourceNotUpdatedOnTargetChange()
        {
            const int converted = 42;

            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, 34f));
            }

            _notifyingObjectMock.Verify(mock => mock.SetValue(converted), Times.Never());
        }

        [Test]
        public void Ctor_TwoWayBindingTargetChanged_CanConvertBackCalled()
        {
            const float target = 52.53f;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvertBack(target, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, target));
            }

            _converterMock.Verify(mock => mock.CanConvertBack(target, culture), Times.Once());
        }

        [Test]
        public void Ctor_TwoWayBindingAndCanConvertBack_ConvertBackCalled()
        {
            const float target = 52.4f;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.ConvertBack(target, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, target));
            }

            _converterMock.Verify(mock => mock.ConvertBack(target, culture), Times.Once());
        }

        [Test]
        public void Ctor_TwoWayBindingAndCanNotConvertBack_ConvertBackNotCalled()
        {
            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>()));

            using (new PropertyBinding<int, float>(
                BindingType.TwoWay,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, 34f));
            }

            _converterMock.Verify(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>()), Times.Never());
        }

        [Test]
        public void Ctor_OneWayToSourceBindingAndConverterCanConvertBack_SourceUpdatedOnTargetChange()
        {
            const int converted = 42;

            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _notifyingObjectMock.Verify(mock => mock.SetValue(converted), Times.Exactly(1));

                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, 34f));
            }

            _notifyingObjectMock.Verify(mock => mock.SetValue(converted), Times.Exactly(2));
        }

        [Test]
        public void Ctor_OneWayToSourceBindingAndConverterCanNotConvertBack_SourceNotUpdatedOnTargetChange()
        {
            const int converted = 42;

            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(converted);
            _dependencyPropertyMock.Setup(mock => mock.SetValue(converted));

            using (new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, 34f));
            }

            _notifyingObjectMock.Verify(mock => mock.SetValue(converted), Times.Never());
        }

        [Test]
        public void Ctor_OneWayToSourceBindingTargetChanged_CanConvertBackCalled()
        {
            const float target = 52.53f;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvertBack(target, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, target));
            }

            _converterMock.Verify(mock => mock.CanConvertBack(target, culture), Times.Once());
        }

        [Test]
        public void Ctor_OneWayToSourceBindingAndCanConvertBack_ConvertBackCalled()
        {
            const float target = 52.4f;
            var culture = CultureInfo.InvariantCulture;

            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(true);
            _converterMock.Setup(mock => mock.ConvertBack(target, culture));

            using (var subject = new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                subject.Culture = culture;
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, target));
            }

            _converterMock.Verify(mock => mock.ConvertBack(target, culture), Times.Once());
        }

        [Test]
        public void Ctor_OneWayToSourceBindingAndCanNotConvertBack_ConvertBackNotCalled()
        {
            _converterMock.Setup(mock => mock.CanConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>())).Returns(false);
            _converterMock.Setup(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>()));

            using (new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object))
            {
                _dependencyPropertyMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<float>(0, 34f));
            }

            _converterMock.Verify(mock => mock.ConvertBack(It.IsAny<float>(), It.IsAny<CultureInfo>()), Times.Never());
        }

        [Test]
        public void Dispose_MultipleCalls_DoesNotThrow()
        {
            var subject = new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object);
            subject.Dispose();
            Assert.DoesNotThrow(() => subject.Dispose());
        }

        [Test]
        public void Dispose_SourceUpdated_TargetNotUpdated()
        {
            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>()));
            var subject = new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object);

            subject.Dispose();
            _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 1));

            _converterMock.Verify(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>()), Times.Never);
        }

        [Test]
        public void Close_MultipleCalls_DoesNotThrow()
        {
            var subject = new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object);
            subject.Close();
            Assert.DoesNotThrow(() => subject.Close());
        }

        [Test]
        public void Close_SourceUpdated_TargetNotUpdated()
        {
            _converterMock.Setup(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>()));
            var subject = new PropertyBinding<int, float>(
                BindingType.OneWayToSource,
                _dependencyPropertyMock.Object,
                _notifyingObjectMock.Object,
                _converterMock.Object);

            subject.Close();
            _notifyingObjectMock.Raise(mock => mock.PropertyChanged += null, new PropertyChangedEventArgs<int>(0, 1));

            _converterMock.Verify(mock => mock.CanConvert(It.IsAny<int>(), It.IsAny<CultureInfo>()), Times.Never);
        }

    }
}
