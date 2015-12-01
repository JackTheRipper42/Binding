using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Binding
{
    public class NotifingObject<T> : INotifingObject<T>
    {
        private T _value;

        public NotifingObject()
            : this(default(T))
        {
        }

        public NotifingObject([CanBeNull] T value)
        {
            _value = value;
        } 

        public event EventHandler<PropertyChangedEventArgs<T>> PropertyChanged;

        public void SetValue(T value)
        {
            if (!Equals(value, _value))
            {
                var oldValue = _value;
                _value = value;
                OnPropertyChanged(oldValue, value);
            }
        }

        public T GetValue()
        {
            return _value;
        }

        protected virtual void OnPropertyChanged(T oldValue, T newValue)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(null, new PropertyChangedEventArgs<T>(oldValue, newValue));
            }
        }
    }
}