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
        private OneWayToSourceValueConverter<int, double> _subject;

        [SetUp]
        public void SetUp()
        {
            var subjectMock = new Mock<OneWayToSourceValueConverter<int, double>>
            {
                CallBase = true
            };
            _subject = subjectMock.Object;
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
    }
}
