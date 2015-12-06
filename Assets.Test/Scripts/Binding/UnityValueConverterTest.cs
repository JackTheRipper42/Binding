using Assets.Scripts.Binding;
using NUnit.Framework;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    internal class UnityValueConverterTest
    {
        [Test]
        public void CanConvert_True()
        {
            var subject = new UnityValueConverter<int>();

            Assert.IsTrue(subject.CanConvert(default(int), null));
        }

        [Test]
        public void CanConvertBack_True()
        {
            var subject = new UnityValueConverter<int>();

            Assert.IsTrue(subject.CanConvertBack(default(int), null));
        }

        [Test]
        public void Convert_ReferenceType_SameInstance()
        {
            var subject = new UnityValueConverter<byte[]>();
            var value = new byte[5];

            var result = subject.Convert(value, null);

            Assert.IsTrue(ReferenceEquals(value, result));
        }

        [Test]
        public void Convert_ValueType_EqualValue()
        {
            var subject = new UnityValueConverter<int>();
            var value = 5789;

            var result = subject.Convert(value, null);

            Assert.AreEqual(value, result);
        }

        [Test]
        public void ConvertBack_ReferenceType_SameInstance()
        {
            var subject = new UnityValueConverter<byte[]>();
            var value = new byte[5];

            var result = subject.ConvertBack(value, null);

            Assert.IsTrue(ReferenceEquals(value, result));
        }

        [Test]
        public void ConvertBack_ValueType_EqualValue()
        {
            var subject = new UnityValueConverter<int>();
            var value = 5789;

            var result = subject.ConvertBack(value, null);

            Assert.AreEqual(value, result);
        }


        [Test]
        public void Instance_MultipleCalls_SameInstance()
        {
            var result = UnityValueConverter<int>.Instance;
            var result2 = UnityValueConverter<int>.Instance;

            Assert.IsTrue(ReferenceEquals(result, result2));
        }
    }
}
