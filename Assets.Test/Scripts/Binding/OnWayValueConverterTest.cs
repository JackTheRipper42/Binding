using Assets.Scripts.Binding;
using Moq;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    class OnWayConverterTest
    {
        private OneWayValueConverter<int, double> _subject;

        [SetUp]
        public void SetUp()
        {
            var subjectMock = new Mock<OneWayValueConverter<int, double>>
            {
                CallBase = true
            };
            _subject = subjectMock.Object;
        }

        [Test]
        public void ConvertBack_Throws()
        {
            Assert.Throws<NotSupportedException>(() => _subject.ConvertBack(42.3, CultureInfo.InvariantCulture));
        }

        [Test]
        public void CanConvertBack_False()
        {
            Assert.IsFalse(_subject.CanConvertBack(42.3, CultureInfo.InvariantCulture));
        }
    }
}
