using Assets.Scripts.Binding;
using NUnit.Framework;
using System;
using System.Globalization;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    internal class OneWayDelegateValueConverterTest
    {
        [Test]
        public void Ctor_ConvertOnlyNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new OneWayDelegateValueConverter<int, int>(null));
        }

        [Test]
        public void Ctor_ConvertNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new OneWayDelegateValueConverter<int, int>(
                null,
                (source, culture) => true));
        }

        [Test]
        public void Ctor_CanConvertNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new OneWayDelegateValueConverter<int, int>(
                (source, culture) => source,
                null));
        }

        [Test]
        public void Convert_DelegateCalled()
        {
            const int input = 42;
            const int output = 24;
            var culture = CultureInfo.InvariantCulture;

            var called = false;
            var subject = new OneWayDelegateValueConverter<int, int>(
                (i, c) =>
                {
                    called = true;
                    Assert.AreEqual(input, i);
                    Assert.AreEqual(culture, c);
                    return output;
                });

            var result = subject.Convert(input, culture);

            Assert.AreEqual(output, result);
            Assert.IsTrue(called);
        }

        [Test]
        public void CanConvert_DelegateCalled()
        {
            const int input = 42;
            const bool output = true;
            var culture = CultureInfo.InvariantCulture;

            var called = false;
            var subject = new OneWayDelegateValueConverter<int, int>(
                (i, c) => i,
                (i, c) =>
                {
                    called = true;
                    Assert.AreEqual(input, i);
                    Assert.AreEqual(culture, c);
                    return output;
                });

            var result = subject.CanConvert(input, culture);

            Assert.AreEqual(output, result);
            Assert.IsTrue(called);
        }

        [Test]
        public void CanConvert_CtorWithoutCanConvert_True()
        {
            var subject = new OneWayDelegateValueConverter<int, int>((i, c) => i);

            var result = subject.CanConvert(42, CultureInfo.InvariantCulture);

            Assert.IsTrue(result);
        }
    }
}
