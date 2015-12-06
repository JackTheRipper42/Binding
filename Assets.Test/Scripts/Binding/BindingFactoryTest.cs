using Assets.Scripts.Binding;
using Moq;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    internal class BindingFactoryTest
    {
        [Test]
        public void CreatePropertyBinding_TargetNull_Throws()
        {
            var subject = new BindingFactory();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.CreatePropertyBinding(
                BindingType.TwoWay,
                null,
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<ValueConverter<int, int>>()));
        }

        [Test]
        public void CreatePropertyBinding_SourceNull_Throws()
        {
            var subject = new BindingFactory();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.CreatePropertyBinding(
                BindingType.TwoWay,
                Mock.Of<IDependencyProperty<int>>(),
                null,
                Mock.Of<ValueConverter<int, int>>()));
        }

        [Test]
        public void CreatePropertyBinding_ValueConverterNull_Throws()
        {
            var subject = new BindingFactory();
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => subject.CreatePropertyBinding(
                BindingType.TwoWay,
                Mock.Of<IDependencyProperty<int>>(),
                Mock.Of<INotifyingObject<int>>(),
                null));
        }

        [Test]
        public void CreatePropertyBinding_ParamsNotNull_NotNull()
        {
            var subject = new BindingFactory();
            var result = subject.CreatePropertyBinding(
                BindingType.TwoWay,
                Mock.Of<IDependencyProperty<int>>(),
                Mock.Of<INotifyingObject<int>>(),
                Mock.Of<ValueConverter<int, int>>());

            Assert.IsNotNull(result);
        }
    }
}
