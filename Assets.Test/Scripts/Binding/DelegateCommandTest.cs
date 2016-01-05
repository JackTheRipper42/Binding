using Assets.Scripts.Binding;
using NUnit.Framework;
using System;

namespace Assets.Test.Scripts.Binding
{
    [TestFixture]
    internal class DelegateCommandTest
    {
        [Test]
        public void Ctor_ExecuteNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DelegateCommand(null, () => true));
        }

        [Test]
        public void Ctor_CanExecuteNull_Throws()
        {
            // ReSharper disable once ObjectCreationAsStatement
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new DelegateCommand(() => { }, null));
        }

        [Test]
        public void Execute_DelegateCalled()
        {
            var called = false;
            var subject = new DelegateCommand(() => { called = true; }, () => true);

            subject.Execute();

            Assert.IsTrue(called);
        }

        [Test]
        public void CanExecute_DelegateCalled()
        {
            var called = false;
            var subject = new DelegateCommand(
                () => { },
                () =>
                {
                    called = true;
                    return true;
                });

            var result = subject.CanExecute();

            Assert.IsTrue(called);
            Assert.IsTrue(result);
        }

        [Test]
        public void RaiseCanExecuteChanged_NoHandler_DoesNotThrow()
        {
            var subject = new DelegateCommand(() => { }, () => true);

            Assert.DoesNotThrow(subject.RaiseCanExecuteChanged);
        }

        [Test]
        public void RaiseCanExecuteChanged_HandlerAttached_HandlerCalled()
        {
            var called = false;
            var subject = new DelegateCommand(() => { }, () => true);
            subject.CanExecuteChanged += (sender, args) =>
            {
                Assert.AreEqual(subject, sender);
                called = true;
            };

            subject.RaiseCanExecuteChanged();

            Assert.IsTrue(called);
        }
    }
}
