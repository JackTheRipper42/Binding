using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Binding
{
    public sealed class DependecyProperty<T> : IDependecyProperty<T>
    {
        private readonly CoerceValueCallback<T> _coerceValueCallback;
        private readonly PropertyChangedCallback<T> _propertyChangedCallback;

        private T _value;

        public DependecyProperty()
            : this(default(T), null, null)
        {
        }

        public DependecyProperty(
            [CanBeNull] T value,
            [CanBeNull] PropertyChangedCallback<T> propertyChangedCallback,
            [CanBeNull] CoerceValueCallback<T> coerceValueCallback)
        {
            _value = value;
            _propertyChangedCallback = propertyChangedCallback;
            _coerceValueCallback = coerceValueCallback;
        }

        public event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;

        public void SetValue(T value)
        {
            if (Equals(value, _value))
            {
                return;
            }

            var oldValue = _value;
            var newValue = _coerceValueCallback != null ? _coerceValueCallback(value) : value;
            if (Equals(newValue, _value))
            {
                return;
            }

            _value = newValue;
            OnPropertyChanged(oldValue, newValue);
        }

        public T GetValue()
        {
            return _value;
        }

        private void OnPropertyChanged(T oldValue, T newValue)
        {
            var callback = _propertyChangedCallback;
            if (callback != null)
            {
                callback(oldValue, newValue);
            }
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs<T>(oldValue, newValue));
            }
        }
    }
}