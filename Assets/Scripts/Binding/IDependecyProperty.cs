using System;

namespace Assets.Scripts.Binding
{
    public interface IDependecyProperty<T>
    {
        event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;

        T GetValue();
        void SetValue(T value);
    }
}