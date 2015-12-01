using Assets.Scripts.Binding;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class OnWayToSourceConverterTest
    {
        private IValueConverter _subject;
        private Mock<OneWayToSourceConverter<int, double>> _subjectMock;

        [SetUp]
        public void SetUp()
        {
            _subjectMock = new Mock<OneWayToSourceConverter<int, double>>
            {
                CallBase = true
            };
            _subject = _subjectMock.Object;
        }

        [Test]
        public void Convert_Throws()
        {
            Assert.Throws<NotSupportedException>(() => _subject.Convert(42, CultureInfo.InvariantCulture));
        }

        [Test]
        public void CanConvert_False()
        {
            Assert.IsFalse(_subject.CanConvert(42, CultureInfo.InvariantCulture));
        }

        [Test]
        public void ConvertBack_GenericConvertBackCalled()
        {
            var culture = CultureInfo.InvariantCulture;
            const double value = 42.42;
            const int convertedValue = 34;
            _subjectMock.Setup(mock => mock.ConvertBack(value, culture)).Returns(convertedValue);

            var result = _subject.ConvertBack(value, culture);

            _subjectMock.Verify(mock => mock.ConvertBack(value, culture));
            Assert.AreEqual(convertedValue, result);
        }

        [Test]
        public void CanConvertBack_GenericCanConvertBackCalled()
        {
            var culture = CultureInfo.InvariantCulture;
            const double value = 42.42;
            const bool canConvert = true;
            _subjectMock.Setup(mock => mock.CanConvertBack(value, culture)).Returns(canConvert);

            var result = _subject.CanConvertBack(value, culture);

            _subjectMock.Verify(mock => mock.CanConvertBack(value, culture));
            Assert.AreEqual(canConvert, result);
        }
    }
}
