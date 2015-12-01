using Assets.Scripts.Binding;
using Moq;
using NUnit.Framework;
using System.Globalization;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class TwoConverterTest
    {
        private IValueConverter _subject;
        private Mock<TwoWayConverter<int, double>> _subjectMock;

        [SetUp]
        public void SetUp()
        {
            _subjectMock = new Mock<TwoWayConverter<int, double>>
            {
                CallBase = true
            };
            _subject = _subjectMock.Object;
        }

        [Test]
        public void Convert_GenericConvertCalled()
        {
            var culture = CultureInfo.InvariantCulture;
            const int value = 42;
            const double convertedValue = 34.3;
            _subjectMock.Setup(mock => mock.Convert(value, culture)).Returns(convertedValue);

            var result = _subject.Convert(value, culture);

            _subjectMock.Verify(mock => mock.Convert(value, culture));
            Assert.AreEqual(convertedValue, result);
        }

        [Test]
        public void CanConvert_GenericCanConvertCalled()
        {
            var culture = CultureInfo.InvariantCulture;
            const int value = 42;
            const bool canConvert = true;
            _subjectMock.Setup(mock => mock.CanConvert(value, culture)).Returns(canConvert);

            var result = _subject.CanConvert(value, culture);

            _subjectMock.Verify(mock => mock.CanConvert(value, culture));
            Assert.AreEqual(canConvert, result);
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
